using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPSTART.Areas.Admin.Models;
using ASPSTART.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ASPSTART.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class ExtendedUiController : Controller
{
  public IActionResult PerfectScrollbar() => View();
  public IActionResult TextDivider() => View();
}
