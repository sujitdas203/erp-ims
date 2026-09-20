using IMS.Helpers.Constants;
using IMS.Models.Announcement;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly ILogger<AnnouncementsController> _logger;

        public AnnouncementsController(IAnnouncementService announcementService, ILogger<AnnouncementsController> logger)
        {
            _announcementService = announcementService;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get
            {
                var raw = User.FindFirst("tenant_id")?.Value;
                return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
            }
        }

        private Guid CurrentUserId
        {
            get
            {
                var raw = User.FindFirst("user_id")?.Value;
                return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
            }
        }

        //[Permission(Permissions.ViewAnnouncement)]
        public async Task<IActionResult> Index(Guid? branchId, string? status, string? search, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _announcementService.GetListAsync(CurrentTenantId, branchId, status, search, page, pageSize: 10);
            return View(vm);
        }

        //[Permission(Permissions.AddAnnouncement)]
        public IActionResult Create()
        {
            var vm = new AnnouncementFormViewModel { BranchOptions = HardcodedMasterData.GetBranchSelectList() };
            return View(vm);
        }

        //[Permission(Permissions.EditAnnouncement)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _announcementService.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.AddAnnouncement)]
        public async Task<IActionResult> CreateAjax(AnnouncementFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty || CurrentUserId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _announcementService.CreateAsync(model, CurrentTenantId, CurrentUserId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating announcement for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.EditAnnouncement)]
        public async Task<IActionResult> EditAjax(AnnouncementFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _announcementService.UpdateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating announcement {Id}", model?.ANN_Id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.EditAnnouncement)]
        public async Task<IActionResult> ToggleActiveAjax(Guid id, bool isActive)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _announcementService.ToggleActiveAsync(id, CurrentTenantId, isActive);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling announcement {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.DeleteAnnouncement)]
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _announcementService.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting announcement {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }
    }
}
