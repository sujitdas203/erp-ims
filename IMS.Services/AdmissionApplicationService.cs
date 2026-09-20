using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Entities;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class AdmissionApplicationService : IAdmissionApplicationService
    {
        private readonly IAdmissionApplicationDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IStudentService _studentService;

        public AdmissionApplicationService(
            IAdmissionApplicationDAL repo, 
            IMasterService masterService,
            IStudentService studentService)
        {
            _repo = repo;
            _masterService = masterService;
            _studentService = studentService;
        }

        public async Task<AdmissionApplicationIndexViewModel> GetListAsync(
            Guid tenantId, string searchTerm, Guid? branchId, Guid? classId,
            Guid? courseId, Guid? academicYearId, string status, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            var (items, totalCount) = await _repo.GetPagedAsync(tenantId, searchTerm, branchId, classId, courseId, academicYearId, status, page, pageSize);

            return new AdmissionApplicationIndexViewModel
            {
                SearchTerm = searchTerm,
                BranchFilter = branchId,
                ClassFilter = classId,
                CourseFilter = courseId,
                AcademicYearFilter = academicYearId,
                StatusFilter = status,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                BranchOptions = HardcodedMasterData.GetBranchSelectList(branchId),
                ClassOptions = HardcodedMasterData.GetClassSelectList(classId),
                CourseOptions = GetMasterSelectList("Course", courseId?.ToString()),
                AcademicYearOptions = GetMasterSelectList("AcademicYear", academicYearId?.ToString()),
                StatusOptions = GetStatusSelectList(status),
                Applications = items.ConvertAll(a => new AdmissionApplicationListItemViewModel
                {
                    AA_Id = a.AA_Id,
                    AA_ApplicationNumber = a.AA_ApplicationNumber,
                    AA_FirstName = a.AA_FirstName,
                    AA_MiddleName = a.AA_MiddleName,
                    AA_LastName = a.AA_LastName,
                    AA_Gender = a.AA_Gender,
                    AA_Phone = a.AA_Phone,
                    AA_Email = a.AA_Email,
                    AA_FatherName = a.AA_FatherName,
                    AA_MotherName = a.AA_MotherName,
                    AA_GuardianName = a.AA_GuardianName,
                    BranchName = a.BranchName ?? "-",
                    ClassName = !string.IsNullOrEmpty(a.ClassName) ? a.ClassName : HardcodedMasterData.GetClassName(a.AA_ClassId),
                    CourseName = a.CourseName ?? "-",
                    AcademicYearName = a.AcademicYearName ?? "-",
                    AA_SubmittedAt = a.AA_SubmittedAt,
                    AA_Status = a.AA_Status
                })
            };
        }

        public async Task<AdmissionApplicationDetailsViewModel> GetDetailsAsync(Guid id, Guid tenantId)
        {
            var a = await _repo.GetByIdAsync(id, tenantId);
            if (a == null) return null;
            return new AdmissionApplicationDetailsViewModel
            {
                AA_Id = a.AA_Id,
                AA_TenantId = a.AA_TenantId,
                AA_BranchId = a.AA_BranchId,
                AA_ApplicationNumber = a.AA_ApplicationNumber,

                AA_FirstName = a.AA_FirstName,
                AA_MiddleName = a.AA_MiddleName,
                AA_LastName = a.AA_LastName,
                AA_DateOfBirth = a.AA_DateOfBirth,
                AA_Gender = a.AA_Gender,
                AA_BloodGroup = a.AA_BloodGroup,
                AA_Nationality = a.AA_Nationality,
                AA_Category = a.AA_Category,
                AA_Religion = a.AA_Religion,
                AA_AadhaarNumber = a.AA_AadhaarNumber,
                AA_EmergencyContactName = a.AA_EmergencyContactName,
                AA_EmergencyContactPhone = a.AA_EmergencyContactPhone,
                AA_Email = a.AA_Email,
                AA_Phone = a.AA_Phone,

                AA_FatherName = a.AA_FatherName,
                AA_FatherPhone = a.AA_FatherPhone,
                AA_FatherOccupation = a.AA_FatherOccupation,
                AA_FatherEmail = a.AA_FatherEmail,

                AA_MotherName = a.AA_MotherName,
                AA_MotherPhone = a.AA_MotherPhone,
                AA_MotherOccupation = a.AA_MotherOccupation,
                AA_MotherEmail = a.AA_MotherEmail,

                AA_GuardianName = a.AA_GuardianName,
                AA_GuardianPhone = a.AA_GuardianPhone,
                AA_GuardianRelation = a.AA_GuardianRelation,
                AA_GuardianEmail = a.AA_GuardianEmail,
                AA_GuardianOccupation = a.AA_GuardianOccupation,

                AA_AddressLine1 = a.AA_AddressLine1,
                AA_AddressLine2 = a.AA_AddressLine2,
                AA_City = a.AA_City,
                AA_State = a.AA_State,
                AA_PostalCode = a.AA_PostalCode,
                AA_Country = a.AA_Country,

                AA_PreviousSchool = a.AA_PreviousSchool,
                AA_PreviousBoard = a.AA_PreviousBoard,
                AA_PreviousGrade = a.AA_PreviousGrade,
                AA_PreviousMarks = a.AA_PreviousMarks,
                AA_TransferCertificateNumber = a.AA_TransferCertificateNumber,

                AA_ClassId = a.AA_ClassId,
                AA_CourseId = a.AA_CourseId,
                AA_AcademicYearId = a.AA_AcademicYearId,
                BranchName = a.BranchName ?? "-",
                ClassName = !string.IsNullOrEmpty(a.ClassName) ? a.ClassName : HardcodedMasterData.GetClassName(a.AA_ClassId),
                CourseName = a.CourseName ?? "-",
                AcademicYearName = a.AcademicYearName ?? "-",

                AA_Status = a.AA_Status,
                AA_SubmittedAt = a.AA_SubmittedAt,
                AA_ReviewedAt = a.AA_ReviewedAt,
                AA_ReviewedBy = a.AA_ReviewedBy,
                AA_Notes = a.AA_Notes,
                AA_CreatedAt = a.AA_CreatedAt,
                AA_UpdatedAt = a.AA_UpdatedAt
            };
        }

        public async Task<AdmissionApplicationFormViewModel> GetForEditAsync(Guid id, Guid tenantId)
        {
            var a = await _repo.GetByIdAsync(id, tenantId);
            if (a == null) return null;
            var vm = new AdmissionApplicationFormViewModel
            {
                AA_Id = a.AA_Id,
                AA_BranchId = a.AA_BranchId,
                AA_ApplicationNumber = a.AA_ApplicationNumber,

                AA_FirstName = a.AA_FirstName,
                AA_MiddleName = a.AA_MiddleName,
                AA_LastName = a.AA_LastName,
                AA_DateOfBirth = a.AA_DateOfBirth,
                AA_Gender = a.AA_Gender,
                AA_BloodGroup = a.AA_BloodGroup,
                AA_Nationality = a.AA_Nationality,
                AA_Category = a.AA_Category,
                AA_Religion = a.AA_Religion,
                AA_AadhaarNumber = a.AA_AadhaarNumber,
                AA_EmergencyContactName = a.AA_EmergencyContactName,
                AA_EmergencyContactPhone = a.AA_EmergencyContactPhone,
                AA_Email = a.AA_Email,
                AA_Phone = a.AA_Phone,

                AA_FatherName = a.AA_FatherName,
                AA_FatherPhone = a.AA_FatherPhone,
                AA_FatherOccupation = a.AA_FatherOccupation,
                AA_FatherEmail = a.AA_FatherEmail,

                AA_MotherName = a.AA_MotherName,
                AA_MotherPhone = a.AA_MotherPhone,
                AA_MotherOccupation = a.AA_MotherOccupation,
                AA_MotherEmail = a.AA_MotherEmail,

                AA_GuardianName = a.AA_GuardianName,
                AA_GuardianPhone = a.AA_GuardianPhone,
                AA_GuardianRelation = a.AA_GuardianRelation,
                AA_GuardianEmail = a.AA_GuardianEmail,
                AA_GuardianOccupation = a.AA_GuardianOccupation,

                AA_AddressLine1 = a.AA_AddressLine1,
                AA_AddressLine2 = a.AA_AddressLine2,
                AA_City = a.AA_City,
                AA_State = a.AA_State,
                AA_PostalCode = a.AA_PostalCode,
                AA_Country = a.AA_Country,

                AA_PreviousSchool = a.AA_PreviousSchool,
                AA_PreviousBoard = a.AA_PreviousBoard,
                AA_PreviousGrade = a.AA_PreviousGrade,
                AA_PreviousMarks = a.AA_PreviousMarks,
                AA_TransferCertificateNumber = a.AA_TransferCertificateNumber,

                AA_ClassId = a.AA_ClassId,
                AA_CourseId = a.AA_CourseId,
                AA_AcademicYearId = a.AA_AcademicYearId,
                AA_Status = a.AA_Status,
                AA_Notes = a.AA_Notes
            };
            PopulateDropdowns(vm);
            return vm;
        }

        public async Task<ServiceResult> CreateAsync(AdmissionApplicationFormViewModel model, Guid tenantId)
        {
            if (!string.IsNullOrWhiteSpace(model.AA_ApplicationNumber) &&
                await _repo.IsApplicationNumberTakenAsync(tenantId, model.AA_ApplicationNumber, null))
                return ServiceResult.Fail("This application number is already in use.");

            if (!model.AA_AcademicYearId.HasValue || model.AA_AcademicYearId == Guid.Empty)
            {
                var years = _masterService.GetAll("AcademicYear");
                var activeYear = years?.FirstOrDefault(y => (y.TryGetValue("AY_IsCurrent", out var curr) && (curr?.ToString() == "True" || curr?.ToString() == "1")) || (y.TryGetValue("AY_IsActive", out var act) && act?.ToString() == "True"))
                              ?? years?.FirstOrDefault();
                if (activeYear != null && activeYear.TryGetValue("AY_Id", out var yId) && Guid.TryParse(yId?.ToString(), out var gYear))
                {
                    model.AA_AcademicYearId = gYear;
                }
                else
                {
                    model.AA_AcademicYearId = new Guid("33333333-3333-3333-3333-333333333303");
                }
            }

            if (!model.AA_BranchId.HasValue || model.AA_BranchId == Guid.Empty)
            {
                var branches = _masterService.GetAll("Branch");
                if (branches != null && branches.Count > 0 && branches[0].TryGetValue("B_Id", out var bId) && Guid.TryParse(bId?.ToString(), out var gBranch))
                {
                    model.AA_BranchId = gBranch;
                }
                else
                {
                    model.AA_BranchId = HardcodedMasterData.Branches[0].Id;
                }
            }

            var entity = MapToEntity(model, tenantId, Guid.NewGuid());
            entity.AA_SubmittedAt = DateTime.UtcNow;
            var id = await _repo.CreateAsync(entity);
            return ServiceResult.Ok("Application submitted successfully.", id);
        }

        public async Task<ServiceResult> UpdateAsync(AdmissionApplicationFormViewModel model, Guid tenantId)
        {
            if (!model.AA_Id.HasValue) return ServiceResult.Fail("Id required.");
            if (await _repo.IsApplicationNumberTakenAsync(tenantId, model.AA_ApplicationNumber, model.AA_Id))
                return ServiceResult.Fail("This application number is already in use.");

            var entity = MapToEntity(model, tenantId, model.AA_Id.Value);
            var success = await _repo.UpdateAsync(entity);
            return success ? ServiceResult.Ok("Application updated successfully.", model.AA_Id) : ServiceResult.Fail("Application not found.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(id, tenantId);
            return success ? ServiceResult.Ok("Application deleted successfully.") : ServiceResult.Fail("Unable to delete application.");
        }

        public async Task<ServiceResult> ReviewAsync(AdmissionReviewViewModel model, Guid tenantId, Guid reviewedBy)
        {
            var success = await _repo.ReviewAsync(model.AA_Id, model.AA_Status, model.AA_Notes, tenantId, reviewedBy);
            return success ? ServiceResult.Ok($"Application status updated to '{model.AA_Status}'.") : ServiceResult.Fail("Application not found.");
        }

        public async Task<ServiceResult> EnrollStudentAsync(AdmissionEnrollViewModel model, Guid tenantId, Guid currentUserId)
        {
            var app = await _repo.GetByIdAsync(model.AA_Id, tenantId);
            if (app == null) return ServiceResult.Fail("Application not found.");

            // Build StudentFormViewModel
            var studentVm = new StudentFormViewModel
            {
                S_BranchId = app.AA_BranchId,
                S_AdmissionNumber = !string.IsNullOrWhiteSpace(model.AdmissionNumber) ? model.AdmissionNumber : app.AA_ApplicationNumber,
                S_FirstName = app.AA_FirstName,
                S_MiddleName = app.AA_MiddleName,
                S_LastName = app.AA_LastName,
                S_DateOfBirth = app.AA_DateOfBirth,
                S_Gender = app.AA_Gender,
                S_BloodGroup = app.AA_BloodGroup,
                S_Email = app.AA_Email,
                S_Phone = app.AA_Phone,
                S_AdmissionDate = DateTime.Today,
                S_Status = "Admitted",
                S_ClassId = model.ClassId ?? app.AA_ClassId,
                S_SectionId = model.SectionId,
                S_BatchId = model.BatchId,
                S_AddressLine1 = app.AA_AddressLine1,
                S_AddressLine2 = app.AA_AddressLine2,
                S_City = app.AA_City,
                S_State = app.AA_State,
                S_PostalCode = app.AA_PostalCode,
                S_Country = app.AA_Country,
                Guardians = new List<GuardianRowViewModel>()
            };

            // Add Father Guardian if present
            if (!string.IsNullOrWhiteSpace(app.AA_FatherName))
            {
                studentVm.Guardians.Add(new GuardianRowViewModel
                {
                    G_FirstName = app.AA_FatherName,
                    G_Phone = app.AA_FatherPhone,
                    G_Email = app.AA_FatherEmail,
                    G_Occupation = app.AA_FatherOccupation,
                    SG_Relation = "Father",
                    SG_IsPrimary = true
                });
            }

            // Add Mother Guardian if present
            if (!string.IsNullOrWhiteSpace(app.AA_MotherName))
            {
                studentVm.Guardians.Add(new GuardianRowViewModel
                {
                    G_FirstName = app.AA_MotherName,
                    G_Phone = app.AA_MotherPhone,
                    G_Email = app.AA_MotherEmail,
                    G_Occupation = app.AA_MotherOccupation,
                    SG_Relation = "Mother",
                    SG_IsPrimary = studentVm.Guardians.Count == 0
                });
            }

            // Add Guardian if present and neither father nor mother added
            if (!string.IsNullOrWhiteSpace(app.AA_GuardianName) && studentVm.Guardians.Count == 0)
            {
                studentVm.Guardians.Add(new GuardianRowViewModel
                {
                    G_FirstName = app.AA_GuardianName,
                    G_Phone = app.AA_GuardianPhone,
                    G_Email = app.AA_GuardianEmail,
                    G_Occupation = app.AA_GuardianOccupation,
                    SG_Relation = !string.IsNullOrWhiteSpace(app.AA_GuardianRelation) ? app.AA_GuardianRelation : "Guardian",
                    SG_IsPrimary = true
                });
            }

            var studentResult = await _studentService.CreateStudentAsync(studentVm, tenantId);
            if (!studentResult.Success)
            {
                return ServiceResult.Fail("Student creation failed: " + studentResult.Message);
            }

            // Update application status to Enrolled
            await _repo.ReviewAsync(app.AA_Id, "Enrolled", "Auto-enrolled as student on " + DateTime.UtcNow.ToString("dd-MMM-yyyy"), tenantId, currentUserId);

            return ServiceResult.Ok("Student successfully enrolled and admitted!", studentResult.Id);
        }

        public void PopulateDropdowns(AdmissionApplicationFormViewModel vm)
        {
            vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.AA_BranchId);
            vm.ClassOptions = HardcodedMasterData.GetClassSelectList(vm.AA_ClassId);
            vm.CourseOptions = GetMasterSelectList("Course", vm.AA_CourseId?.ToString());
            vm.AcademicYearOptions = GetMasterSelectList("AcademicYear", vm.AA_AcademicYearId?.ToString());
            
            vm.GenderOptions = new()
            {
                new("Male", "Male"), new("Female", "Female"), new("Other", "Other")
            };

            vm.BloodGroupOptions = HardcodedMasterData.GetBloodGroupSelectList(vm.AA_BloodGroup);
            
            vm.CategoryOptions = new()
            {
                new("General", "General"),
                new("OBC", "OBC"),
                new("SC", "SC"),
                new("ST", "ST"),
                new("EWS", "EWS"),
                new("Other", "Other")
            };

            vm.StatusOptions = GetStatusSelectList(vm.AA_Status);
        }

        private List<SelectListItem> GetMasterSelectList(string entityType, string selectedValue = null)
        {
            var items = _masterService.GetAll(entityType);
            var list = new List<SelectListItem>();
            if (items == null) return list;
            foreach (var item in items)
            {
                var keyEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Id"));
                var id = keyEntry.Value?.ToString() ?? "";

                string displayName = null;
                var nameEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Name"));
                if (nameEntry.Value != null) displayName = nameEntry.Value.ToString();

                if (string.IsNullOrEmpty(displayName))
                {
                    var firstName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_FirstName")).Value?.ToString() ?? "";
                    var lastName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_LastName")).Value?.ToString() ?? "";
                    displayName = $"{firstName} {lastName}".Trim();
                }

                if (string.IsNullOrEmpty(displayName))
                    displayName = item.Values.ElementAtOrDefault(1)?.ToString() ?? id;

                list.Add(new SelectListItem { Value = id, Text = displayName, Selected = id == selectedValue });
            }
            return list;
        }

        private static AdmissionApplication MapToEntity(AdmissionApplicationFormViewModel m, Guid tenantId, Guid id) => new()
        {
            AA_Id = id != Guid.Empty ? id : Guid.NewGuid(),
            AA_TenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId,
            AA_BranchId = m.AA_BranchId.HasValue && m.AA_BranchId.Value != Guid.Empty ? m.AA_BranchId.Value : HardcodedMasterData.Branches[0].Id,
            AA_AcademicYearId = m.AA_AcademicYearId.HasValue && m.AA_AcademicYearId.Value != Guid.Empty ? m.AA_AcademicYearId.Value : new Guid("33333333-3333-3333-3333-333333333303"),
            AA_ApplicationNumber = m.AA_ApplicationNumber,

            AA_FirstName = m.AA_FirstName,
            AA_MiddleName = m.AA_MiddleName,
            AA_LastName = m.AA_LastName,
            AA_DateOfBirth = m.AA_DateOfBirth,
            AA_Gender = m.AA_Gender,
            AA_BloodGroup = m.AA_BloodGroup,
            AA_Nationality = m.AA_Nationality,
            AA_Category = m.AA_Category,
            AA_Religion = m.AA_Religion,
            AA_AadhaarNumber = m.AA_AadhaarNumber,
            AA_EmergencyContactName = m.AA_EmergencyContactName,
            AA_EmergencyContactPhone = m.AA_EmergencyContactPhone,
            AA_Email = m.AA_Email,
            AA_Phone = m.AA_Phone,

            AA_FatherName = m.AA_FatherName,
            AA_FatherPhone = m.AA_FatherPhone,
            AA_FatherOccupation = m.AA_FatherOccupation,
            AA_FatherEmail = m.AA_FatherEmail,

            AA_MotherName = m.AA_MotherName,
            AA_MotherPhone = m.AA_MotherPhone,
            AA_MotherOccupation = m.AA_MotherOccupation,
            AA_MotherEmail = m.AA_MotherEmail,

            AA_GuardianName = m.AA_GuardianName,
            AA_GuardianPhone = m.AA_GuardianPhone,
            AA_GuardianRelation = m.AA_GuardianRelation,
            AA_GuardianEmail = m.AA_GuardianEmail,
            AA_GuardianOccupation = m.AA_GuardianOccupation,

            AA_AddressLine1 = m.AA_AddressLine1,
            AA_AddressLine2 = m.AA_AddressLine2,
            AA_City = m.AA_City,
            AA_State = m.AA_State,
            AA_PostalCode = m.AA_PostalCode,
            AA_Country = m.AA_Country,

            AA_PreviousSchool = m.AA_PreviousSchool,
            AA_PreviousBoard = m.AA_PreviousBoard,
            AA_PreviousGrade = m.AA_PreviousGrade,
            AA_PreviousMarks = m.AA_PreviousMarks,
            AA_TransferCertificateNumber = m.AA_TransferCertificateNumber,

            AA_ClassId = m.AA_ClassId,
            AA_CourseId = m.AA_CourseId,
            AA_Status = m.AA_Status ?? "Submitted",
            AA_Notes = m.AA_Notes
        };

        private static List<SelectListItem> GetStatusSelectList(string selected = null)
        {
            var statuses = new[] { "Submitted", "UnderReview", "Approved", "Rejected", "Waitlisted", "Enrolled" };
            return new List<SelectListItem>(
                Array.ConvertAll(statuses, s => new SelectListItem { Value = s, Text = s, Selected = s == selected }));
        }
    }
}
