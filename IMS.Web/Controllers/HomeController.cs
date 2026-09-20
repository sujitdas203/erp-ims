using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Diagnostics;
using IMS.Helpers.Constants;
using IMS.Services;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;
using IMS.Web.Models;

namespace IMS.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserSessionService _sessionService;
        private readonly IRedisService _redis;

        public HomeController(IUserSessionService sessionService, IRedisService redis)
        {
            _sessionService = sessionService;
            _redis = redis;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(); 
            }
            return RedirectToAction("Dashboard");
        }

        //[Authorize(Roles = AppRoles.TenantAdmin)]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = User.Identity.Name;
            var roles = await _redis.GetUserFieldAsync(user, "roles");
            ViewBag.RedisRoles = roles;
            return View();
        }

        //[Permission(Permissions.ReadVendor)]
        public IActionResult AdminPage()
        {
            return Content("Only TENANT_ADMIN can see this");
        }

        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/Home/Dashboard"
            });
        }
       
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            await _sessionService.RemoveUserSessionAsync(username);

            return SignOut(new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") },OpenIdConnectDefaults.AuthenticationScheme,CookieAuthenticationDefaults.AuthenticationScheme);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult About() => View();

        [AllowAnonymous]
        public IActionResult Academics() => View();

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Admission()
        {
            var vm = new IMS.Models.ViewModels.AdmissionApplicationFormViewModel();
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyAdmission(
            IMS.Models.ViewModels.AdmissionApplicationFormViewModel model,
            [FromServices] IAdmissionApplicationService admissionService,
            [FromServices] INotificationService notificationService,
            [FromServices] ILogger<HomeController> logger)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest" || 
                         Request.ContentType?.Contains("application/json") == true ||
                         Request.Headers["Accept"].ToString().Contains("application/json");

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            k => k.Key,
                            v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault(m => !string.IsNullOrWhiteSpace(m));
                    if (isAjax) return Json(new { success = false, message = firstError ?? "Please fill all required fields correctly.", errors });
                    return View("Admission", model);
                }

                if (string.IsNullOrWhiteSpace(model.AA_FirstName) || string.IsNullOrWhiteSpace(model.AA_LastName))
                {
                    if (isAjax) return Json(new { success = false, message = "First Name and Last Name are required." });
                    ModelState.AddModelError(string.Empty, "First Name and Last Name are required.");
                    return View("Admission", model);
                }

                if (string.IsNullOrWhiteSpace(model.AA_Phone))
                {
                    if (isAjax) return Json(new { success = false, message = "Contact Mobile Number is required." });
                    ModelState.AddModelError(string.Empty, "Contact Mobile Number is required.");
                    return View("Admission", model);
                }

                if (string.IsNullOrWhiteSpace(model.AA_ApplicationNumber))
                {
                    model.AA_ApplicationNumber = "APP-" + DateTime.UtcNow.ToString("yy") + "-" + new Random().Next(1000, 9999);
                }

                model.AA_Status = "Submitted";
                var tenantId = HardcodedMasterData.CurrentTenantId;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var raw = User.FindFirst("tenant_id")?.Value;
                    if (Guid.TryParse(raw, out var tId) && tId != Guid.Empty) tenantId = tId;
                }

                var result = await admissionService.CreateAsync(model, tenantId);

                if (result.Success)
                {
                    try
                    {
                        // Trigger operational notification safely
                        await notificationService.RaiseNotificationAsync(
                            tenantId,
                            "ADMISSION_SUBMITTED",
                            $"New Admission Application: {model.AA_FirstName} {model.AA_LastName}",
                            $"Application #{model.AA_ApplicationNumber} submitted for review.",
                            "/AdmissionApplication",
                            "TENANT_ADMIN",
                            null,
                            model.AA_Email
                        );
                    }
                    catch (Exception notifEx)
                    {
                        logger.LogWarning(notifEx, "Notification delivery skipped for {AppNum}", model.AA_ApplicationNumber);
                    }

                    if (isAjax)
                    {
                        return Json(new { 
                            success = true, 
                            message = $"Your application has been received! Reference Number: {model.AA_ApplicationNumber}", 
                            applicationNumber = model.AA_ApplicationNumber 
                        });
                    }

                    TempData["AdmissionSuccess"] = $"Your application has been received! Your application reference number is: {model.AA_ApplicationNumber}. Our admissions office will contact you soon.";
                    return RedirectToAction(nameof(Admission));
                }

                if (isAjax) return Json(new { success = false, message = result.Message ?? "Failed to save application." });
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to submit application. Please try again.");
                return View("Admission", model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error submitting public admission application");
                if (isAjax) return Json(new { success = false, message = "An error occurred while saving: " + ex.Message });
                ModelState.AddModelError(string.Empty, "An error occurred while saving: " + ex.Message);
                return View("Admission", model);
            }
        }

        [AllowAnonymous]
        public IActionResult Notices() => View();

        [AllowAnonymous]
        public IActionResult Gallery() => View();

        [AllowAnonymous]
        public IActionResult Calendar() => View();

        [AllowAnonymous]
        public IActionResult Contact() => View();
    }
}
