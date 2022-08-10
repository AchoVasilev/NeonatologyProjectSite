namespace Neonatology.Web.Areas.Administration.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Common.Constants.GlobalConstants;

[Area((AdministrationAreaName))]
[Authorize(Roles = AdministratorRoleName)]
public class BaseController : Controller
{
}