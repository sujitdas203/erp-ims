using IMS.Helpers.Constants;
using IMS.Models.SubjectSyllabus;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                return Guid.TryParse(raw, out var id) && id != Guid.Empty ? id : HardcodedMasterData.CurrentTenantId;
            }
        }

        public async Task<IActionResult> Index(Guid? courseId, Guid? subjectId, string? status, string? search, int page = 1)
        {
            var vm = await _syllabusService.GetListAsync(CurrentTenantId, courseId, subjectId, status, search, page, pageSize: 10);
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await _syllabusService.GetEmptyFormAsync(CurrentTenantId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var vm = await _syllabusService.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjectsByCourse(Guid? courseId)
        {
            try
            {
                var subjects = await _syllabusService.GetSubjectsByCourseAsync(courseId, CurrentTenantId);
                return Json(new { success = true, data = subjects });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subjects for course {CourseId}", courseId);
                return Json(new { success = false, message = "Unable to load subjects." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax(SyllabusFormViewModel model)
        {
            var errors = ValidateSyllabus(model);
            if (errors.Count > 0)
            {
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var result = await _syllabusService.CreateAsync(model, CurrentTenantId);
                if (!result.Success)
                {
                    var customErrors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (result.Message != null && (result.Message.Contains("Unit", StringComparison.OrdinalIgnoreCase) || result.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase)))
                    {
                        customErrors["UnitNumber"] = result.Message;
                    }
                    return Json(new { success = false, message = result.Message, errors = customErrors });
                }

                return Json(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating syllabus unit for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(SyllabusFormViewModel model)
        {
            var errors = ValidateSyllabus(model);
            if (!model.SS_Id.HasValue || model.SS_Id.Value == Guid.Empty)
            {
                errors["SS_Id"] = "Syllabus ID is required.";
            }

            if (errors.Count > 0)
            {
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var result = await _syllabusService.UpdateAsync(model, CurrentTenantId);
                if (!result.Success)
                {
                    var customErrors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (result.Message != null && (result.Message.Contains("Unit", StringComparison.OrdinalIgnoreCase) || result.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase)))
                    {
                        customErrors["UnitNumber"] = result.Message;
                    }
                    return Json(new { success = false, message = result.Message, errors = customErrors });
                }

                return Json(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating syllabus unit {Id}", model?.SS_Id);
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActiveAjax(Guid id, bool isActive)
        {
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
        public async Task<IActionResult> ToggleCompletedAjax(Guid id, bool isCompleted)
        {
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
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
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

        private static Dictionary<string, string> ValidateSyllabus(SyllabusFormViewModel model)
        {
            var errors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (model == null)
            {
                errors["General"] = "Form submission is empty.";
                return errors;
            }

            if (model.CourseId == Guid.Empty)
                errors["CourseId"] = "Please select a Course.";

            if (model.SubjectId == Guid.Empty)
                errors["SubjectId"] = "Please select a Subject.";

            if (model.UnitNumber <= 0)
                errors["UnitNumber"] = "Unit Number must be greater than 0.";

            if (string.IsNullOrWhiteSpace(model.UnitTitle))
                errors["UnitTitle"] = "Unit Title is required.";

            return errors;
        }
    }
}
