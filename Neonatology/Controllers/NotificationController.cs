namespace Neonatology.Controllers;

using System.Threading.Tasks;
using Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.NotificationService;
using static Common.GlobalConstants.NotificationConstants;
using static Common.WebConstants.RouteTemplates;

[Authorize]
public class NotificationController : BaseController
{
    private readonly INotificationService notificationService;
    private readonly IHubContext<NotificationHub> notificationHub;
    private readonly UserManager<ApplicationUser> userManager;

    public NotificationController(
        INotificationService notificationService,
        IHubContext<NotificationHub> notificationHub,
        UserManager<ApplicationUser> userManager)
    {
        this.notificationService = notificationService;
        this.notificationHub = notificationHub;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userName = this.User.Identity.Name;
        var userId = this.User.GetId();

        var model = await this.notificationService
            .GetUserNotifications(userName, userId, NotificationOnClick, NotificationSkip);

        return this.View(model);
    }

    [HttpPost]
    [Route(NotificationEditStatus)]
    public async Task<bool> EditStatus(string newStatus, string id)
    {
        var user = await this.userManager.GetUserAsync(this.User);
        var isEdited = await this.notificationService.EditStatus(user.Id, newStatus, id);
        await this.ChangeNotificationCounter(isEdited, user);

        return isEdited;
    }

    [HttpPost]
    [Route(NotificationDelete)]
    public async Task<bool> DeleteNotification(string id)
    {
        var currentUser = await this.userManager.GetUserAsync(this.User);
        var isDeleted = await this.notificationService.DeleteNotification(currentUser.Id, id);

        await this.ChangeNotificationCounter(isDeleted, currentUser);

        return isDeleted;
    }

    [HttpGet]
    [Route(NotificationGetMore)]
    public async Task<IActionResult> GetMoreNotifications(int skip, int take)
    {
        var currentUser = await this.userManager.GetUserAsync(this.User);
        var result = await this.notificationService
            .GetUserNotifications(currentUser.UserName, currentUser.Id, take, skip);

        return new JsonResult(new { newNotifications = result.Notifications, hasMore = result.IsLessThanDefaultCount });
    }

    private async Task ChangeNotificationCounter(bool isForChange, ApplicationUser user)
    {
        if (isForChange)
        {
            var count = await this.notificationService.GetUserNotificationsCount(user.UserName);
            await this.notificationHub.Clients.User(user.Id).SendAsync("ReceiveNotification", count, false);
        }
    }
}