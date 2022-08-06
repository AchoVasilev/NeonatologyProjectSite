namespace Neonatology.Areas.Administration.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Common.Constants.GlobalConstants;

[Area(("Administration"))]
[Authorize(Roles = AdministratorRoleName)]
public class BaseController : Controller
{
}