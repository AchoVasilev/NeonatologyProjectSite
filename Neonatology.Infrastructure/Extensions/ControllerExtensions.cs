namespace Neonatology.Infrastructure.Extensions;

using Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions
{
    public static IActionResult ViewOrNotFound(this Controller controller, object model)
    {
        if (model is null)
        {
            return controller.NotFound();
        }

        return controller.View(model);
    }
}