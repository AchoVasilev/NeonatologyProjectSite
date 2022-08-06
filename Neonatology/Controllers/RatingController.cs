namespace Neonatology.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentService;
using Services.DoctorService;
using Services.PatientService;
using Services.RatingService;
using ViewModels.Rating;
using static Common.GlobalConstants;
using static Common.GlobalConstants.MessageConstants;

[Authorize]
public class RatingController : BaseController
{
    private const int Page = 1;
    private readonly IAppointmentService appointmentService;
    private readonly IDoctorService doctorService;
    private readonly IPatientService patientService;
    private readonly IRatingService ratingService;
    private readonly IMapper mapper;

    public RatingController(
        IAppointmentService appointmentService,
        IDoctorService doctorService,
        IPatientService patientService,
        IRatingService ratingService,
        IMapper mapper)
    {
        this.appointmentService = appointmentService;
        this.doctorService = doctorService;
        this.patientService = patientService;
        this.ratingService = ratingService;
        this.mapper = mapper;
    }

    [Authorize(Roles = PatientRoleName)]
    public IActionResult RateAppointment(int appointmentId)
        => this.View(new CreateRatingFormModel()
        {
            AppointmentId = appointmentId
        });

    [Authorize(Roles = PatientRoleName)]
    [HttpPost]
    public async Task<IActionResult> RateAppointment(CreateRatingFormModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.RedirectToAction(nameof(RateAppointment), new { model.AppointmentId });
        }

        var userId = this.User.GetId();
        var appointment = await this.appointmentService.GetUserAppointmentAsync(userId, model.AppointmentId);

        if (appointment is null)
        {
            return this.NotFound();
        }

        if (appointment.IsRated)
        {
            this.TempData["Message"] = RatedAppointment;

            return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = Page });
        }

        var doctorId = await this.doctorService.GetDoctorIdByAppointmentId(model.AppointmentId);
        if (doctorId is null)
        {
            return this.NotFound();
        }

        var patientId = await this.patientService.GetPatientIdByUserId(userId);
        if (patientId is null)
        {
            return this.NotFound();
        }

        model.PatientId = patientId;
        model.DoctorId = doctorId;

        var ratingModel = this.mapper.Map<CreateRatingModel>(model);
        var isRated = await this.ratingService.AddAsync(ratingModel);

        if (isRated.Failed)
        {
            this.TempData["Message"] = isRated.Error;

            return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = Page });
        }

        this.TempData["Message"] = string.Format(SuccessfulRating, model.Number);

        return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = Page });
    }

    [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
    public async Task<IActionResult> Approve(int appointmentId)
    {
        var isApproved = await this.ratingService.ApproveRating(appointmentId);
        var userIsAdmin = this.User.IsAdmin();

        if (isApproved.Failed)
        {
            this.TempData["Message"] = isApproved.Error;

            return userIsAdmin
                ? this.RedirectToAction("All", "Appointment", new { area = "Administration" })
                : this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = Page });
        }

        this.TempData["Message"] = SuccessfullyApprovedRating;

        return userIsAdmin
            ? this.RedirectToAction("All", "Appointment", new { area = "Administration" })
            : this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = Page });
    }

    [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
    public async Task<IActionResult> Delete(int appointmentId)
    {
        var result = await this.ratingService.DeleteRating(appointmentId);
        var userIsAdmin = this.User.IsAdmin();

        if (result.Failed)
        {
            this.TempData["Message"] = result.Error;
            return userIsAdmin
                ? this.RedirectToAction("All", "Appointment", new { area = "Administration" })
                : this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = Page });
        }

        this.TempData["Message"] = SuccessfullyApprovedRating;

        return userIsAdmin
            ? this.RedirectToAction("All", "Appointment", new { area = "Administration" })
            : this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = Page });
    }
}