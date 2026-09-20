using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class CourseSubjectController : Controller
    {
        private readonly ICourseSubjectService _service;
        private readonly ILogger<CourseSubjectController> _logger;

        public CourseSubjectController(ICourseSubjectService service, ILogger<CourseSubjectController> logger)
        {
            _service = service;
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

        public async Task<IActionResult> Index(Guid? courseId)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetListAsync(CurrentTenantId, courseId);
            return View(vm);
        }

        public async Task<IActionResult> Create(Guid? courseId)
        {
            var vm = new CourseSubjectFormViewModel { CS_CourseId = courseId ?? Guid.Empty };
            if (courseId.HasValue && courseId.Value != Guid.Empty)
            {
                vm.CS_SequenceNo = await _service.GetNextSequenceNoAsync(courseId.Value, CurrentTenantId);
            }
            _service.PopulateDropdowns(vm, CurrentTenantId);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetNextSequence(Guid courseId)
        {
            if (CurrentTenantId == Guid.Empty || courseId == Guid.Empty)
                return Json(new { success = false, nextSequence = 1 });

            try
            {
                var seq = await _service.GetNextSequenceNoAsync(courseId, CurrentTenantId);
                return Json(new { success = true, nextSequence = seq });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching next sequence for course {CourseId}", courseId);
                return Json(new { success = false, nextSequence = 1 });
            }
        }

        public async Task<IActionResult> Edit(Guid courseId, Guid subjectId)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetForEditAsync(courseId, subjectId, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourseSubject(CourseSubjectFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please log in again." });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.First().ErrorMessage
                    );
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var result = await _service.CreateAsync(model, CurrentTenantId);
                if (!result.Success)
                {
                    var fieldErrors = new Dictionary<string, string>();
                    if (result.Message != null && result.Message.Contains("Sequence number", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldErrors["CS_SequenceNo"] = result.Message;
                    }
                    else if (result.Message != null && result.Message.Contains("already assigned", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldErrors["CS_SubjectId"] = result.Message;
                    }
                    else if (result.Message != null && result.Message.Contains("Pass Marks", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldErrors["CS_PassMarks"] = result.Message;
                    }
                    return Json(new { success = false, message = result.Message, errors = fieldErrors });
                }

                return Json(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course subject");
                var msg = ex.Message ?? "An error occurred while saving the subject.";
                var fieldErrors = new Dictionary<string, string>();
                if (msg.Contains("UQ_CourseSubjects_CS_Course_SequenceNo", StringComparison.OrdinalIgnoreCase))
                {
                    msg = "This Sequence Number is already used for another subject in this course. Please choose a different sequence number.";
                    fieldErrors["CS_SequenceNo"] = msg;
                }
                return Json(new { success = false, message = msg, errors = fieldErrors });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourseSubject(CourseSubjectFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please log in again." });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.First().ErrorMessage
                    );
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var result = await _service.UpdateAsync(model, CurrentTenantId);
                if (!result.Success)
                {
                    var fieldErrors = new Dictionary<string, string>();
                    if (result.Message != null && result.Message.Contains("Sequence number", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldErrors["CS_SequenceNo"] = result.Message;
                    }
                    else if (result.Message != null && result.Message.Contains("Pass Marks", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldErrors["CS_PassMarks"] = result.Message;
                    }
                    return Json(new { success = false, message = result.Message, errors = fieldErrors });
                }

                return Json(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course subject");
                var msg = ex.Message ?? "An error occurred while updating the subject.";
                var fieldErrors = new Dictionary<string, string>();
                if (msg.Contains("UQ_CourseSubjects_CS_Course_SequenceNo", StringComparison.OrdinalIgnoreCase))
                {
                    msg = "This Sequence Number is already used for another subject in this course. Please choose a different sequence number.";
                    fieldErrors["CS_SequenceNo"] = msg;
                }
                return Json(new { success = false, message = msg, errors = fieldErrors });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseSubject(Guid courseId, Guid subjectId)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired." });
            try
            {
                var result = await _service.DeleteAsync(courseId, subjectId, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course subject");
                return Json(new { success = false, message = ex.Message ?? "Something went wrong." });
            }
        }
    }
}
