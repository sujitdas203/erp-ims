using System;
using System.Threading.Tasks;
using IMS.Models.MockTest;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class MockTestsController : Controller
    {
        private readonly IMockTestService _mockTestService;
        private readonly ILogger<MockTestsController> _logger;

        public MockTestsController(IMockTestService mockTestService, ILogger<MockTestsController> logger)
        {
            _mockTestService = mockTestService;
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

        public async Task<IActionResult> Index(Guid? batchId, Guid? subjectId, string? status, string? search, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _mockTestService.GetListAsync(CurrentTenantId, batchId, subjectId, status, search, page, pageSize: 10);
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _mockTestService.GetDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _mockTestService.GetEmptyFormAsync(CurrentTenantId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _mockTestService.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax(MockTestFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _mockTestService.CreateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating mock test for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(MockTestFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _mockTestService.UpdateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating mock test {Id}", model?.MT_Id);
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
                var result = await _mockTestService.ToggleActiveAsync(id, CurrentTenantId, isActive);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling active for mock test {Id}", id);
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
                var result = await _mockTestService.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting mock test {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }
    }
}
