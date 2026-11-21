using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Helpers.Constants;
using IMS.Models.ViewModels;
using IMS.Services;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IGuardianService _guardianService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            IStudentService studentService,
            IGuardianService guardianService,
            ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _guardianService = guardianService;
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

        private string CurrentUserId => User.FindFirst("user_id")?.Value;
        private string CurrentUserType => User.FindFirst("roles")?.Value;
        private bool IsAdmin => string.Equals(CurrentUserType, AppRoles.TenantAdmin, StringComparison.OrdinalIgnoreCase);

        [Permission(Permissions.ViewStudent)]
        public async Task<IActionResult> Index(string searchTerm, string status, Guid? branchId, Guid? classId, int page = 1)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _studentService.GetStudentListAsync(
                CurrentTenantId, searchTerm, status, branchId, classId, page, pageSize: 10);

            return View(vm);
        }
        [Permission(Permissions.ViewStudent)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _studentService.GetStudentDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }
        [Permission(Permissions.AddStudent)]
        public IActionResult Create()
        {
            var vm = new StudentFormViewModel();
            _studentService.PopulateDropdowns(vm);
            return View(vm);
        }
        [Permission(Permissions.UpdateStudent)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _studentService.GetStudentForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.AddStudent)]
        public async Task<IActionResult> AddStudent(StudentFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _studentService.CreateStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student for tenant {TenantId}", CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong while saving the student. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.UpdateStudent)]
        public async Task<IActionResult> EditStudent(StudentFormViewModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            try
            {
                var result = await _studentService.UpdateStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student {StudentId} for tenant {TenantId}", model?.S_Id, CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong while saving the student. Please try again." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.UpdateStudent)]
        public async Task<IActionResult> QuickUpdate([FromBody] QuickStudentUpdateModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            if (model == null || model.S_Id == Guid.Empty)
                return Json(new { success = false, message = "Invalid student data." });

            try
            {
                var result = await _studentService.QuickUpdateStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in QuickUpdate for student {StudentId}", model.S_Id);
                return Json(new { success = false, message = "Something went wrong while updating the student." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.UpdateStudent)]
        public async Task<IActionResult> QuickUpdateForm(QuickStudentUpdateModel model)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            if (model == null || model.S_Id == Guid.Empty)
                return Json(new { success = false, message = "Invalid student data." });

            try
            {
                var result = await _studentService.QuickUpdateStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message, id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in QuickUpdateForm for student {StudentId}", model.S_Id);
                return Json(new { success = false, message = "Something went wrong while updating the student." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.UpdateStudent)]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            if (id == Guid.Empty || string.IsNullOrWhiteSpace(status))
                return Json(new { success = false, message = "Invalid student or status." });

            try
            {
                var model = new QuickStudentUpdateModel { S_Id = id, S_Status = status.Trim() };
                var result = await _studentService.QuickUpdateStudentAsync(model, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for student {StudentId}", id);
                return Json(new { success = false, message = "Something went wrong while updating status." });
            }
        }

        [HttpGet]
        [Permission(Permissions.ViewStudent)]
        public async Task<IActionResult> GetQuickDetails(Guid id)
        {
            if (CurrentTenantId == Guid.Empty) return Unauthorized();

            var vm = await _studentService.GetStudentDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return Json(new
            {
                s_Id = vm.S_Id,
                fullName = vm.FullName,
                studentCode = vm.S_StudentCode,
                admissionNumber = vm.S_AdmissionNumber,
                status = vm.S_Status,
                branchName = vm.BranchName,
                className = vm.ClassName,
                sectionName = vm.SectionName,
                batchName = vm.BatchName,
                admissionDate = vm.S_AdmissionDate?.ToString("dd MMM yyyy"),
                dateOfBirth = vm.S_DateOfBirth?.ToString("dd MMM yyyy"),
                gender = vm.S_Gender,
                bloodGroup = vm.S_BloodGroup,
                email = vm.S_Email,
                phone = vm.S_Phone,
                fullAddress = vm.FullAddress,
                totalFeeInvoiced = vm.TotalFeeInvoiced,
                totalFeePaid = vm.TotalFeePaid,
                totalFeeBalance = vm.TotalFeeBalance,
                feeStatus = vm.FeeStatus,
                attendancePercentage = vm.AttendancePercentage,
                totalLeavesTaken = vm.TotalLeavesTaken,
                guardiansCount = vm.Guardians.Count
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.DeleteStudent)]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            if (CurrentTenantId == Guid.Empty)
                return Json(new { success = false, message = "Your session has expired. Please sign in again." });

            if (!IsAdmin)
                return Json(new { success = false, message = "Only an administrator can remove a student." });

            try
            {
                var result = await _studentService.DeleteStudentAsync(id, CurrentTenantId);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student {StudentId} for tenant {TenantId}", id, CurrentTenantId);
                return Json(new { success = false, message = "Something went wrong while removing the student. Please try again." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchGuardians(string term)
        {
            if (CurrentTenantId == Guid.Empty) return Json(Array.Empty<object>());

            try
            {
                var results = await _guardianService.SearchAsync(CurrentTenantId, term);
                return Json(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching guardians for tenant {TenantId}", CurrentTenantId);
                return Json(Array.Empty<object>());
            }
        }
    }
}
