using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPSTART.Areas.Admin.Models;
using ASPSTART.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ASPSTART.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class PagesController : Controller
{
  public IActionResult AccountSettings() => View();
  public IActionResult AccountSettingsConnections() => View();
  public IActionResult AccountSettingsNotifications() => View();
  public IActionResult MiscError() => View();
  public IActionResult MiscUnderMaintenance() => View();
}
