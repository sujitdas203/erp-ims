using System;
using System.Threading.Tasks;
using IMS.Helpers.Constants;
using IMS.Models.HomeTask;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class HomeTasksController : Controller
    {
        private readonly IHomeTaskService _homeTaskService;
        private readonly ILogger<HomeTasksController> _logger;

        public HomeTasksController(IHomeTaskService homeTaskService, ILogger<HomeTasksController> logger)
        {
            _homeTaskService = homeTaskService;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get
            {
                var raw = User.FindFirst("tenant_id")?.Value;
                return Guid.TryParse(raw, out var id) && id != Guid.Empty ? id : HardcodedMasterData.CurrentTenantId;
            }
        }

        public async Task<IActionResult> Index(Guid? batchId, Guid? subjectId, string? status, string? search, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _homeTaskService.GetListAsync(CurrentTenantId, batchId, subjectId, status, search, page, pageSize: 10);
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _homeTaskService.GetDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _homeTaskService.GetEmptyFormAsync(CurrentTenantId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _homeTaskService.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax(HomeTaskFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _homeTaskService.CreateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating home task for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(HomeTaskFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _homeTaskService.UpdateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating home task {Id}", model?.HT_Id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActiveAjax(Guid id, bool isActive)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _homeTaskService.ToggleActiveAsync(id, CurrentTenantId, isActive);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling active for home task {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _homeTaskService.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting home task {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }
    }
}
