using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionApplicationService _service;
        private readonly ILogger<AdmissionController> _logger;

        public AdmissionController(IAdmissionApplicationService service, ILogger<AdmissionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get 
            { 
                var raw = User.FindFirst("tenant_id")?.Value; 
                return Guid.TryParse(raw, out var id) && id != Guid.Empty ? id : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId; 
            }
        }

        private Guid? CurrentUserId
        {
            get { var raw = User.FindFirst("user_id")?.Value; return Guid.TryParse(raw, out var id) ? id : (Guid?)null; }
        }

        public async Task<IActionResult> Index(string searchTerm, Guid? branchId, Guid? classId, Guid? courseId,
            Guid? academicYearId, string status, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetListAsync(CurrentTenantId, searchTerm, branchId, classId, courseId, academicYearId, status, page, 10);
            return View("~/Views/AdmissionApplication/Index.cshtml", vm);
        }

        public IActionResult Create()
        {
            var vm = new AdmissionApplicationFormViewModel();
            _service.PopulateDropdowns(vm);
            return View("~/Views/AdmissionApplication/Create.cshtml", vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View("~/Views/AdmissionApplication/Edit.cshtml", vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();
            var vm = await _service.GetDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View("~/Views/AdmissionApplication/Details.cshtml", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddApplication(AdmissionApplicationFormViewModel model)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var firstError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault(m => !string.IsNullOrWhiteSpace(m));
                return Json(new { success = false, message = firstError ?? "Please fill all required fields correctly.", errors });
            }

            try 
            { 
                var r = await _service.CreateAsync(model, tenantId); 
                return Json(new { success = r.Success, message = r.Message, id = r.Id }); 
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error creating application in AdmissionController"); 
                return Json(new { success = false, message = "Something went wrong: " + ex.Message }); 
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplication(AdmissionApplicationFormViewModel model)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var firstError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault(m => !string.IsNullOrWhiteSpace(m));
                return Json(new { success = false, message = firstError ?? "Please fill all required fields correctly.", errors });
            }

            try 
            { 
                var r = await _service.UpdateAsync(model, tenantId); 
                return Json(new { success = r.Success, message = r.Message, id = r.Id }); 
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error updating application in AdmissionController"); 
                return Json(new { success = false, message = "Something went wrong: " + ex.Message }); 
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteApplication(Guid id)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            try 
            { 
                var r = await _service.DeleteAsync(id, tenantId); 
                return Json(new { success = r.Success, message = r.Message }); 
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error deleting application in AdmissionController"); 
                return Json(new { success = false, message = "Something went wrong." }); 
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(AdmissionReviewViewModel model)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            var userId = CurrentUserId ?? Guid.Empty;
            try 
            { 
                var r = await _service.ReviewAsync(model, tenantId, userId); 
                return Json(new { success = r.Success, message = r.Message }); 
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error reviewing application in AdmissionController"); 
                return Json(new { success = false, message = "Something went wrong: " + ex.Message }); 
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(AdmissionEnrollViewModel model)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            var userId = CurrentUserId ?? Guid.Empty;
            try 
            { 
                var r = await _service.EnrollStudentAsync(model, tenantId, userId); 
                return Json(new { success = r.Success, message = r.Message, studentId = r.Id }); 
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error enrolling student in AdmissionController"); 
                return Json(new { success = false, message = "Enrollment failed: " + ex.Message }); 
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAttachment(Guid id, string docType, Microsoft.AspNetCore.Http.IFormFile file)
        {
            var tenantId = CurrentTenantId != Guid.Empty ? CurrentTenantId : IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Please select a file to upload." });

            try
            {
                var r = await _service.UploadAttachmentAsync(id, docType, file, tenantId);
                return Json(new { success = r.Success, message = r.Message, url = r.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading attachment in AdmissionController for {AppId}", id);
                return Json(new { success = false, message = "Upload failed: " + ex.Message });
            }
        }
    }
}
