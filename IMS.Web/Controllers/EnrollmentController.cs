using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using IMS.Helpers.Constants;
using IMS.Web.Authorization;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _service;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService service, ILogger<EnrollmentController> logger)
        {
            _service = service;
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

        [Permission(Permissions.ViewEnrollment)]
        public async Task<IActionResult> Index(string? searchTerm, Guid? academicYearId, Guid? courseId,
            Guid? batchId, string? status, Guid? branchId, Guid? classId, Guid? sectionId, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetListAsync(CurrentTenantId, searchTerm, academicYearId, courseId, batchId, status, page, 10, branchId, classId, sectionId);
            return View(vm);
        }

        [Permission(Permissions.AddEnrollment)]
        public IActionResult Create()
        {
            var vm = new EnrollmentFormViewModel();
            _service.PopulateDropdowns(vm);
            return View(vm);
        }

        [Permission(Permissions.UpdateEnrollment)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [Permission(Permissions.ViewEnrollment)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(Permissions.AddEnrollment)]
        public async Task<IActionResult> AddEnrollment(EnrollmentFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired. Please log in again." });

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
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating enrollment");
                return Json(new { success = false, message = "An error occurred while saving the enrollment: " + ex.Message });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(Permissions.UpdateEnrollment)]
        public async Task<IActionResult> EditEnrollment(EnrollmentFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired. Please log in again." });

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
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating enrollment");
                return Json(new { success = false, message = "An error occurred while updating the enrollment: " + ex.Message });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(Permissions.DeleteEnrollment)]
        public async Task<IActionResult> DeleteEnrollment(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired. Please log in again." });
            try
            {
                var result = await _service.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting enrollment");
                return Json(new { success = false, message = "An error occurred while deleting the enrollment." });
            }
        }

        // =========================================================================
        // Student Promotion & Academic Progression Actions
        // =========================================================================

        [Permission(Permissions.AddEnrollment)]
        public async Task<IActionResult> Promote(Guid? academicYearId, Guid? branchId, Guid? classId, Guid? sectionId)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetBulkPromotionViewModelAsync(CurrentTenantId, branchId, academicYearId, classId, sectionId);
            return View(vm);
        }

        [HttpGet]
        [Permission(Permissions.ViewEnrollment)]
        public async Task<IActionResult> GetCandidates(Guid academicYearId, Guid classId, Guid? sectionId, Guid? branchId)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired." });
            try
            {
                var students = await _service.GetStudentsForPromotionAsync(CurrentTenantId, branchId, academicYearId, classId, sectionId);
                return Json(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching promotion candidates");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(Permissions.AddEnrollment)]
        public async Task<IActionResult> ProcessBulkPromotion([FromBody] BulkPromotionSubmitViewModel model)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired. Please log in again." });
            if (model == null || model.Students == null || model.Students.Count == 0)
                return Json(new { success = false, message = "No students selected for promotion." });

            try
            {
                var result = await _service.ProcessBulkPromotionAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, details = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bulk promotion");
                return Json(new { success = false, message = "Failed to process promotion: " + ex.Message });
            }
        }

        [HttpGet]
        [Permission(Permissions.AddEnrollment)]
        public async Task<IActionResult> GetStudentPromotionForm(Guid studentId)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired." });
            try
            {
                var vm = await _service.GetPromotionFormForStudentAsync(studentId, CurrentTenantId);
                if (vm == null) return Json(new { success = false, message = "Student not found." });
                return Json(new { success = true, data = vm });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading promotion form for student {StudentId}", studentId);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(Permissions.AddEnrollment)]
        public async Task<IActionResult> PromoteStudent(StudentPromotionRequestViewModel model)
        {
            if (CurrentTenantId == Guid.Empty) return Json(new { success = false, message = "Session expired. Please log in again." });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value!.Errors.First().ErrorMessage);
                return Json(new { success = false, message = "Please fill in all required promotion fields.", errors });
            }

            try
            {
                var result = await _service.PromoteStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error promoting student {StudentId}", model.StudentId);
                return Json(new { success = false, message = "An error occurred while promoting the student: " + ex.Message });
            }
        }
    }
}
