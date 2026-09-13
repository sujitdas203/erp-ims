using IMS.Helpers.Constants;
using IMS.Models.SubjectSyllabus;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class SyllabusController : Controller
    {
        private readonly ISubjectSyllabusService _syllabusService;
        private readonly ILogger<SyllabusController> _logger;

        public SyllabusController(ISubjectSyllabusService syllabusService, ILogger<SyllabusController> logger)
        {
            _syllabusService = syllabusService;
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

       // [Permission(Permissions.ViewSyllabus)]
        public async Task<IActionResult> Index(Guid? courseId, Guid? subjectId, string? status, string? search, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _syllabusService.GetListAsync(CurrentTenantId, courseId, subjectId, status, search, page, pageSize: 10);
            return View(vm);
        }

        //[Permission(Permissions.AddSyllabus)]
        public async Task<IActionResult> Create()
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _syllabusService.GetEmptyFormAsync(CurrentTenantId);
            return View(vm);
        }

        //[Permission(Permissions.EditSyllabus)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _syllabusService.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.AddSyllabus)]
        public async Task<IActionResult> CreateAjax(SyllabusFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _syllabusService.CreateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating syllabus unit for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.EditSyllabus)]
        public async Task<IActionResult> EditAjax(SyllabusFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _syllabusService.UpdateAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating syllabus unit {Id}", model?.SS_Id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.EditSyllabus)]
        public async Task<IActionResult> ToggleActiveAjax(Guid id, bool isActive)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _syllabusService.ToggleActiveAsync(id, CurrentTenantId, isActive);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling active for syllabus unit {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.EditSyllabus)]
        public async Task<IActionResult> ToggleCompletedAjax(Guid id, bool isCompleted)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _syllabusService.ToggleCompletedAsync(id, CurrentTenantId, isCompleted);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling completed for syllabus unit {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Permission(Permissions.DeleteSyllabus)]
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _syllabusService.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting syllabus unit {Id}", id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }
    }
}
