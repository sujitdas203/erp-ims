using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Entities;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace IMS.Services
{
    public class AdmissionApplicationService : IAdmissionApplicationService
    {
        private readonly IAdmissionApplicationDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IStudentService _studentService;
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly IStudentDAL _studentDAL;
        private readonly IEmailService _emailService;
        private readonly INotificationConfigDAL _configDAL;
        private readonly ILogger<AdmissionApplicationService> _logger;

        public AdmissionApplicationService(
            IAdmissionApplicationDAL repo, 
            IMasterService masterService,
            IStudentService studentService,
            IEnrollmentDAL enrollmentDAL,
            IStudentDAL studentDAL,
            IEmailService emailService,
            INotificationConfigDAL configDAL,
            ILogger<AdmissionApplicationService> logger)
        {
            _repo = repo;
            _masterService = masterService;
            _studentService = studentService;
            _enrollmentDAL = enrollmentDAL;
            _studentDAL = studentDAL;
            _emailService = emailService;
            _configDAL = configDAL;
            _logger = logger;
        }

        private static int GetStatusSortOrder(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return 1;
            return status.Trim().ToUpperInvariant() switch
            {
                "SUBMITTED" => 1,
                "UNDER_REVIEW" or "UNDER REVIEW" or "UNDERREVIEW" => 2,
                "WAITLISTED" or "WAITLIST" => 3,
                "REJECTED" => 4,
                "APPROVED" => 5,
                "ENROLLED" => 6,
                _ => 7
            };
        }

        public async Task<AdmissionApplicationIndexViewModel> GetListAsync(
            Guid tenantId, string searchTerm, Guid? branchId, Guid? classId,
            Guid? courseId, Guid? academicYearId, string status, int page, int pageSize)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            page = page < 1 ? 1 : page;
            var (items, totalCount) = await _repo.GetPagedAsync(tenantId, searchTerm, branchId, classId, courseId, academicYearId, status, page, pageSize);

            if (items != null && items.Count > 1)
            {
                items = items
                    .OrderBy(a => GetStatusSortOrder(a.AA_Status))
                    .ThenByDescending(a => a.AA_SubmittedAt ?? a.AA_CreatedAt)
                    .ToList();
            }

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
                Applications = (items ?? new List<AdmissionApplication>()).ConvertAll(a => new AdmissionApplicationListItemViewModel
                {
                    AA_Id = a.AA_Id,
                    AA_ApplicationNumber = a.AA_ApplicationNumber ?? "-",
                    AA_FirstName = a.AA_FirstName,
                    AA_MiddleName = a.AA_MiddleName,
                    AA_LastName = a.AA_LastName,
                    AA_Gender = a.AA_Gender ?? "-",
                    AA_Phone = a.AA_Phone,
                    AA_Email = a.AA_Email ?? "-",
                    AA_FatherName = a.AA_FatherName,
                    AA_MotherName = a.AA_MotherName,
                    AA_GuardianName = a.AA_GuardianName,
                    AA_StudentPhotoUrl = a.AA_StudentPhotoUrl,
                    AA_RequiresTransport = a.AA_RequiresTransport,
                    AA_RequiresHostel = a.AA_RequiresHostel,
                    BranchName = a.BranchName ?? "-",
                    ClassName = !string.IsNullOrEmpty(a.ClassName) ? a.ClassName : HardcodedMasterData.GetClassName(a.AA_ClassId),
                    CourseName = a.CourseName ?? "-",
                    AcademicYearName = a.AcademicYearName ?? "-",
                    AA_SubmittedAt = a.AA_SubmittedAt,
                    AA_Status = a.AA_Status ?? "Submitted",
                    AA_AdmittedStudentId = a.AA_AdmittedStudentId,
                    AdmittedStudentCode = a.AdmittedStudentCode
                })
            };
        }

        public async Task<AdmissionApplicationDetailsViewModel> GetDetailsAsync(Guid id, Guid tenantId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            var a = await _repo.GetByIdAsync(id, tenantId);
            if (a == null) return null;

            string? feeStructureName = null;
            if (a.AA_FeeStructureId.HasValue && a.AA_FeeStructureId != Guid.Empty)
            {
                var fs = _masterService.GetById("FeeStructure", a.AA_FeeStructureId.Value);
                if (fs != null && fs.TryGetValue("FS_Name", out var fsName))
                    feeStructureName = fsName?.ToString();
            }

            var vm = new AdmissionApplicationDetailsViewModel
            {
                AA_Id = a.AA_Id,
                AA_TenantId = a.AA_TenantId,
                AA_BranchId = a.AA_BranchId,
                AA_ApplicationNumber = a.AA_ApplicationNumber ?? "-",

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

                AA_StudentPhotoUrl = a.AA_StudentPhotoUrl,
                AA_BirthCertificateUrl = a.AA_BirthCertificateUrl,
                AA_TransferCertificateUrl = a.AA_TransferCertificateUrl,
                AA_MarksheetUrl = a.AA_MarksheetUrl,
                AA_NationalIdDocUrl = a.AA_NationalIdDocUrl,

                AA_MedicalConditions = a.AA_MedicalConditions,
                AA_RequiresTransport = a.AA_RequiresTransport,
                AA_TransportPickupPoint = a.AA_TransportPickupPoint,
                AA_RequiresHostel = a.AA_RequiresHostel,

                AA_SecondLanguage = a.AA_SecondLanguage,
                AA_MotherTongue = a.AA_MotherTongue,
                AA_HasSibling = a.AA_HasSibling,
                AA_SiblingDetails = a.AA_SiblingDetails,

                AA_FeeStructureId = a.AA_FeeStructureId,
                FeeStructureName = feeStructureName ?? "-",

                AA_Status = a.AA_Status ?? "Submitted",
                AA_SubmittedAt = a.AA_SubmittedAt,
                AA_ReviewedAt = a.AA_ReviewedAt,
                AA_ReviewedBy = a.AA_ReviewedBy,
                AA_Notes = a.AA_Notes,

                AA_AdmittedStudentId = a.AA_AdmittedStudentId,
                AA_EnrollmentId = a.AA_EnrollmentId,
                AdmittedStudentCode = a.AdmittedStudentCode,
                AdmittedStudentAdmissionNumber = a.AdmittedStudentAdmissionNumber
            };

            return vm;
        }

        public async Task<AdmissionApplicationDetailsViewModel> GetDetailsByNumberAsync(string applicationNumber, string phone = null)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber)) return null;
            var a = await _repo.GetByApplicationNumberAsync(applicationNumber, phone);
            if (a == null) return null;

            string? feeStructureName = null;
            if (a.AA_FeeStructureId.HasValue && a.AA_FeeStructureId != Guid.Empty)
            {
                var fs = _masterService.GetById("FeeStructure", a.AA_FeeStructureId.Value);
                if (fs != null && fs.TryGetValue("FS_Name", out var fsName))
                    feeStructureName = fsName?.ToString();
            }

            var vm = new AdmissionApplicationDetailsViewModel
            {
                AA_Id = a.AA_Id,
                AA_TenantId = a.AA_TenantId,
                AA_BranchId = a.AA_BranchId,
                AA_ApplicationNumber = a.AA_ApplicationNumber ?? "-",

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

                AA_StudentPhotoUrl = a.AA_StudentPhotoUrl,
                AA_BirthCertificateUrl = a.AA_BirthCertificateUrl,
                AA_TransferCertificateUrl = a.AA_TransferCertificateUrl,
                AA_MarksheetUrl = a.AA_MarksheetUrl,
                AA_NationalIdDocUrl = a.AA_NationalIdDocUrl,

                AA_MedicalConditions = a.AA_MedicalConditions,
                AA_RequiresTransport = a.AA_RequiresTransport,
                AA_TransportPickupPoint = a.AA_TransportPickupPoint,
                AA_RequiresHostel = a.AA_RequiresHostel,

                AA_SecondLanguage = a.AA_SecondLanguage,
                AA_MotherTongue = a.AA_MotherTongue,
                AA_HasSibling = a.AA_HasSibling,
                AA_SiblingDetails = a.AA_SiblingDetails,

                AA_FeeStructureId = a.AA_FeeStructureId,
                FeeStructureName = feeStructureName ?? "-",

                AA_Status = a.AA_Status ?? "Submitted",
                AA_SubmittedAt = a.AA_SubmittedAt,
                AA_ReviewedAt = a.AA_ReviewedAt,
                AA_ReviewedBy = a.AA_ReviewedBy,
                AA_Notes = a.AA_Notes,

                AA_AdmittedStudentId = a.AA_AdmittedStudentId,
                AA_EnrollmentId = a.AA_EnrollmentId,
                AdmittedStudentCode = a.AdmittedStudentCode,
                AdmittedStudentAdmissionNumber = a.AdmittedStudentAdmissionNumber
            };

            return vm;
        }

        public async Task<AdmissionApplicationFormViewModel> GetForEditAsync(Guid id, Guid tenantId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
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

                AA_StudentPhotoUrl = a.AA_StudentPhotoUrl,
                AA_BirthCertificateUrl = a.AA_BirthCertificateUrl,
                AA_TransferCertificateUrl = a.AA_TransferCertificateUrl,
                AA_MarksheetUrl = a.AA_MarksheetUrl,
                AA_NationalIdDocUrl = a.AA_NationalIdDocUrl,

                AA_MedicalConditions = a.AA_MedicalConditions,
                AA_RequiresTransport = a.AA_RequiresTransport,
                AA_TransportPickupPoint = a.AA_TransportPickupPoint,
                AA_RequiresHostel = a.AA_RequiresHostel,

                AA_SecondLanguage = a.AA_SecondLanguage,
                AA_MotherTongue = a.AA_MotherTongue,
                AA_HasSibling = a.AA_HasSibling,
                AA_SiblingDetails = a.AA_SiblingDetails,

                AA_FeeStructureId = a.AA_FeeStructureId,
                AA_Status = a.AA_Status,
                AA_Notes = a.AA_Notes
            };

            PopulateDropdowns(vm);
            return vm;
        }

        public async Task<ServiceResult> CreateAsync(AdmissionApplicationFormViewModel model, Guid tenantId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
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

            // Save file uploads if present
            await ProcessFileUploadsAsync(model);

            var entity = MapToEntity(model, tenantId, Guid.NewGuid());
            entity.AA_SubmittedAt = DateTime.UtcNow;
            var id = await _repo.CreateAsync(entity);

            // Send submission confirmation email in background
            _ = Task.Run(async () =>
            {
                try
                {
                    await SendApplicationSubmittedEmailAsync(entity, tenantId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send admission application submission email for {AppNum}", entity.AA_ApplicationNumber);
                }
            });

            return ServiceResult.Ok("Application submitted successfully.", id);
        }

        public async Task<ServiceResult> UpdateAsync(AdmissionApplicationFormViewModel model, Guid tenantId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            if (!model.AA_Id.HasValue) return ServiceResult.Fail("Id required.");
            if (await _repo.IsApplicationNumberTakenAsync(tenantId, model.AA_ApplicationNumber, model.AA_Id))
                return ServiceResult.Fail("This application number is already in use.");

            var existing = await _repo.GetByIdAsync(model.AA_Id.Value, tenantId) 
                        ?? await _repo.GetByIdAsync(model.AA_Id.Value, HardcodedMasterData.CurrentTenantId);

            // Save file uploads if present
            await ProcessFileUploadsAsync(model);

            if (existing != null)
            {
                if (string.IsNullOrEmpty(model.AA_StudentPhotoUrl)) model.AA_StudentPhotoUrl = existing.AA_StudentPhotoUrl;
                if (string.IsNullOrEmpty(model.AA_BirthCertificateUrl)) model.AA_BirthCertificateUrl = existing.AA_BirthCertificateUrl;
                if (string.IsNullOrEmpty(model.AA_TransferCertificateUrl)) model.AA_TransferCertificateUrl = existing.AA_TransferCertificateUrl;
                if (string.IsNullOrEmpty(model.AA_MarksheetUrl)) model.AA_MarksheetUrl = existing.AA_MarksheetUrl;
                if (string.IsNullOrEmpty(model.AA_NationalIdDocUrl)) model.AA_NationalIdDocUrl = existing.AA_NationalIdDocUrl;
            }

            var entityTenant = (existing != null && existing.AA_TenantId != Guid.Empty) ? existing.AA_TenantId : tenantId;
            var entity = MapToEntity(model, entityTenant, model.AA_Id.Value);
            var success = await _repo.UpdateAsync(entity);
            return success ? ServiceResult.Ok("Application updated successfully.", model.AA_Id) : ServiceResult.Fail("Application not found.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            var success = await _repo.DeleteAsync(id, tenantId);
            return success ? ServiceResult.Ok("Application deleted successfully.") : ServiceResult.Fail("Unable to delete application.");
        }

        public async Task<ServiceResult> ReviewAsync(AdmissionReviewViewModel model, Guid tenantId, Guid reviewedBy)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            var success = await _repo.ReviewAsync(model.AA_Id, model.AA_Status, model.AA_Notes, tenantId, reviewedBy);
            if (success && string.Equals(model.AA_Status, "Approved", StringComparison.OrdinalIgnoreCase))
            {
                // Send approval confirmation email in background
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await SendAdmissionApprovalEmailAsync(model.AA_Id, model.AA_Notes, tenantId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send admission approval email for application {AppId}", model.AA_Id);
                    }
                });
            }
            return success ? ServiceResult.Ok($"Application status updated to '{model.AA_Status}'.") : ServiceResult.Fail("Application not found.");
        }

        public async Task<ServiceResult> EnrollStudentAsync(AdmissionEnrollViewModel model, Guid tenantId, Guid currentUserId)
        {
            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            var app = await _repo.GetByIdAsync(model.AA_Id, tenantId);
            if (app == null) return ServiceResult.Fail("Application not found.");

            if (app.AA_Status == "Enrolled" && app.AA_AdmittedStudentId.HasValue)
            {
                return ServiceResult.Fail("This application is already enrolled as a student.");
            }

            // 1. Build StudentFormViewModel
            var studentVm = new StudentFormViewModel
            {
                S_BranchId = model.BranchId ?? app.AA_BranchId,
                S_AdmissionNumber = !string.IsNullOrWhiteSpace(model.AdmissionNumber) ? model.AdmissionNumber : app.AA_ApplicationNumber,
                S_FirstName = app.AA_FirstName,
                S_MiddleName = app.AA_MiddleName,
                S_LastName = app.AA_LastName,
                S_DateOfBirth = app.AA_DateOfBirth,
                S_Gender = app.AA_Gender,
                S_BloodGroup = app.AA_BloodGroup,
                S_Email = app.AA_Email,
                S_Phone = app.AA_Phone,
                S_AdmissionDate = model.AdmissionDate ?? DateTime.Today,
                S_Status = "Active",
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
                    G_Phone = !string.IsNullOrWhiteSpace(app.AA_FatherPhone) ? app.AA_FatherPhone : app.AA_Phone,
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
                    G_Phone = !string.IsNullOrWhiteSpace(app.AA_MotherPhone) ? app.AA_MotherPhone : app.AA_Phone,
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
                    G_Phone = !string.IsNullOrWhiteSpace(app.AA_GuardianPhone) ? app.AA_GuardianPhone : app.AA_Phone,
                    G_Email = app.AA_GuardianEmail,
                    G_Occupation = app.AA_GuardianOccupation,
                    SG_Relation = !string.IsNullOrWhiteSpace(app.AA_GuardianRelation) ? app.AA_GuardianRelation : "Guardian",
                    SG_IsPrimary = true
                });
            }

            // 2. Create Student
            var studentResult = await _studentService.CreateStudentAsync(studentVm, tenantId);
            if (!studentResult.Success)
            {
                return ServiceResult.Fail("Student creation failed: " + studentResult.Message);
            }

            Guid studentId = studentResult.Id.Value;

            // 3. Create First Academic Enrollment Record
            var enrollment = new Enrollment
            {
                E_Id = Guid.NewGuid(),
                E_TenantId = tenantId,
                E_BranchId = model.BranchId ?? app.AA_BranchId,
                E_StudentId = studentId,
                E_AcademicYearId = model.AcademicYearId ?? app.AA_AcademicYearId,
                E_ClassId = model.ClassId ?? app.AA_ClassId,
                E_SectionId = model.SectionId,
                E_CourseId = model.CourseId ?? app.AA_CourseId,
                E_BatchId = model.BatchId,
                E_RollNumber = model.RollNumber,
                E_EnrollmentNumber = "ENR-" + DateTime.Now.Year + "-" + (model.AdmissionNumber ?? app.AA_ApplicationNumber ?? new Random().Next(1000, 9999).ToString()),
                E_EnrollmentDate = model.AdmissionDate ?? DateTime.Today,
                E_EnrollmentType = "New Admission",
                E_Status = "Active",
                E_FeeStructureId = model.FeeStructureId ?? app.AA_FeeStructureId,
                E_Remarks = $"Enrolled from Admission Application #{app.AA_ApplicationNumber}",
                E_CreatedAt = DateTime.UtcNow,
                E_UpdatedAt = DateTime.UtcNow
            };

            var enrollmentId = await _enrollmentDAL.CreateAsync(enrollment);

            // 4. Update Admission Application Status & Link References
            await _repo.LinkEnrolledStudentAsync(app.AA_Id, studentId, enrollmentId, tenantId, currentUserId);

            // 5. Send Student Portal Credentials & Welcome Email in background
            _ = Task.Run(async () =>
            {
                try
                {
                    await SendStudentEnrollmentCredentialsEmailAsync(app, studentId, enrollment, tenantId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send student enrollment credentials email for student {StudentId}", studentId);
                }
            });

            return ServiceResult.Ok($"Student admitted and enrolled successfully! (Student ID: {studentId}, Enrollment No: {enrollment.E_EnrollmentNumber})", studentId);
        }

        public void PopulateDropdowns(AdmissionApplicationFormViewModel vm)
        {
            vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.AA_BranchId);
            vm.ClassOptions = HardcodedMasterData.GetClassSelectList(vm.AA_ClassId);
            vm.CourseOptions = GetMasterSelectList("Course", vm.AA_CourseId?.ToString());
            vm.AcademicYearOptions = GetMasterSelectList("AcademicYear", vm.AA_AcademicYearId?.ToString());
            vm.FeeStructureOptions = GetMasterSelectList("FeeStructure", vm.AA_FeeStructureId?.ToString());
            
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
            vm.GuardianRelationOptions = HardcodedMasterData.GetRelationSelectList(vm.AA_GuardianRelation);
        }

        private static string ResolveWebRoot()
        {
            var candidates = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "IMS.Web", "wwwroot"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                Path.Combine(AppContext.BaseDirectory, "wwwroot"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "IMS.Web", "wwwroot"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot")
            };

            foreach (var path in candidates)
            {
                try
                {
                    if (Directory.Exists(path)) return Path.GetFullPath(path);
                }
                catch { }
            }

            var fallback = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(fallback))
            {
                try { Directory.CreateDirectory(fallback); } catch { }
            }
            return fallback;
        }

        private async Task ProcessFileUploadsAsync(AdmissionApplicationFormViewModel model)
        {
            var webRoot = ResolveWebRoot();
            var uploadsFolder = Path.Combine(webRoot, "uploads", "admissions");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (model.StudentPhoto != null && model.StudentPhoto.Length > 0)
            {
                var fileName = $"photo_{Guid.NewGuid()}{Path.GetExtension(model.StudentPhoto.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.StudentPhoto.CopyToAsync(stream);
                }
                model.AA_StudentPhotoUrl = $"/uploads/admissions/{fileName}";
            }

            if (model.BirthCertificateDoc != null && model.BirthCertificateDoc.Length > 0)
            {
                var fileName = $"birthcert_{Guid.NewGuid()}{Path.GetExtension(model.BirthCertificateDoc.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.BirthCertificateDoc.CopyToAsync(stream);
                }
                model.AA_BirthCertificateUrl = $"/uploads/admissions/{fileName}";
            }

            if (model.TransferCertificateDoc != null && model.TransferCertificateDoc.Length > 0)
            {
                var fileName = $"tc_{Guid.NewGuid()}{Path.GetExtension(model.TransferCertificateDoc.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.TransferCertificateDoc.CopyToAsync(stream);
                }
                model.AA_TransferCertificateUrl = $"/uploads/admissions/{fileName}";
            }

            if (model.MarksheetDoc != null && model.MarksheetDoc.Length > 0)
            {
                var fileName = $"marksheet_{Guid.NewGuid()}{Path.GetExtension(model.MarksheetDoc.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.MarksheetDoc.CopyToAsync(stream);
                }
                model.AA_MarksheetUrl = $"/uploads/admissions/{fileName}";
            }

            if (model.NationalIdDoc != null && model.NationalIdDoc.Length > 0)
            {
                var fileName = $"nationalid_{Guid.NewGuid()}{Path.GetExtension(model.NationalIdDoc.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NationalIdDoc.CopyToAsync(stream);
                }
                model.AA_NationalIdDocUrl = $"/uploads/admissions/{fileName}";
            }
        }

        public async Task<ServiceResult> UploadAttachmentAsync(Guid id, string docType, Microsoft.AspNetCore.Http.IFormFile file, Guid tenantId)
        {
            if (file == null || file.Length == 0) return ServiceResult.Fail("No file provided.");
            if (file.Length > 5 * 1024 * 1024) return ServiceResult.Fail("File size cannot exceed 5MB.");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif", ".pdf" };
            if (string.IsNullOrEmpty(ext) || Array.IndexOf(allowedExts, ext) < 0)
                return ServiceResult.Fail("Invalid file type. Allowed formats: JPG, PNG, WEBP, AVIF, PDF.");

            tenantId = tenantId != Guid.Empty ? tenantId : HardcodedMasterData.CurrentTenantId;
            var app = await _repo.GetByIdAsync(id, tenantId);
            if (app == null) return ServiceResult.Fail("Admission application not found.");

            var webRoot = ResolveWebRoot();
            var uploadsFolder = Path.Combine(webRoot, "uploads", "admissions");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var normalizedType = (docType ?? "").ToLowerInvariant().Replace("_", "").Replace("-", "").Replace(" ", "");
            string prefix;
            if (normalizedType.Contains("photo") || normalizedType.Contains("avatar") || normalizedType.Contains("picture") || normalizedType.Contains("image"))
            {
                prefix = "photo";
            }
            else if (normalizedType.Contains("birth") || normalizedType.Contains("dob"))
            {
                prefix = "birthcert";
            }
            else if (normalizedType.Contains("tc") || normalizedType.Contains("transfer") || normalizedType.Contains("migration"))
            {
                prefix = "tc";
            }
            else if (normalizedType.Contains("mark") || normalizedType.Contains("grade") || normalizedType.Contains("score") || normalizedType.Contains("result"))
            {
                prefix = "marksheet";
            }
            else if (normalizedType.Contains("national") || normalizedType.Contains("aadhaar") || normalizedType.Contains("aadhar") || normalizedType.Contains("id") || normalizedType.Contains("nid"))
            {
                prefix = "nationalid";
            }
            else
            {
                prefix = "doc";
            }

            var fileName = $"{prefix}_{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativeUrl = $"/uploads/admissions/{fileName}";

            switch (prefix)
            {
                case "photo":
                    app.AA_StudentPhotoUrl = relativeUrl;
                    break;
                case "birthcert":
                    app.AA_BirthCertificateUrl = relativeUrl;
                    break;
                case "tc":
                    app.AA_TransferCertificateUrl = relativeUrl;
                    break;
                case "marksheet":
                    app.AA_MarksheetUrl = relativeUrl;
                    break;
                case "nationalid":
                    app.AA_NationalIdDocUrl = relativeUrl;
                    break;
                default:
                    app.AA_BirthCertificateUrl = relativeUrl;
                    break;
            }

            var updated = await _repo.UpdateAsync(app);
            if (!updated) return ServiceResult.Fail("Failed to update application with uploaded attachment.");

            return ServiceResult.Ok("Document uploaded and attached successfully.", relativeUrl);
        }

        // =========================================================================
        // Automated Email Notifications for Admissions & Student Enrollment
        // =========================================================================

        private async Task SendApplicationSubmittedEmailAsync(AdmissionApplication app, Guid tenantId)
        {
            var recipientEmail = ResolveEmail(app.AA_Email, app.AA_FatherEmail, app.AA_MotherEmail, app.AA_GuardianEmail);
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;

            var config = await _configDAL.GetConfigAsync(tenantId);
            var studentFullName = JoinName(app.AA_FirstName, app.AA_MiddleName, app.AA_LastName);
            var gradeOrCourse = ResolveGradeOrCourse(app.AA_ClassId, app.AA_CourseId);
            var branchName = HardcodedMasterData.GetBranchName(app.AA_BranchId);
            var academicYearName = ResolveAcademicYearName(app.AA_AcademicYearId);

            var subject = $"[IMS ERP] Admission Application Received: #{app.AA_ApplicationNumber} - {studentFullName}";
            var bodyHtml = $@"
<div style='font-family:-apple-system,BlinkMacSystemFont,""Segoe UI"",Roboto,Helvetica,Arial,sans-serif; max-width:600px; margin:0 auto; background:#ffffff; border:1px solid #e2e8f0; border-radius:12px; overflow:hidden; box-shadow:0 4px 6px -1px rgba(0,0,0,0.1);'>
    <div style='background:linear-gradient(135deg, #1e293b 0%, #0f172a 100%); padding:28px 24px; text-align:center;'>
        <h2 style='color:#ffffff; margin:0 0 6px 0; font-size:22px; font-weight:700;'>IMS ERP Portal</h2>
        <span style='background:#3b82f6; color:#ffffff; font-size:12px; font-weight:600; padding:4px 12px; border-radius:20px; text-transform:uppercase; letter-spacing:0.5px;'>Application Received</span>
    </div>
    <div style='padding:28px 24px;'>
        <h3 style='color:#0f172a; margin-top:0;'>Application Submitted Successfully!</h3>
        <p style='color:#475569; font-size:14px; line-height:1.6;'>
            Dear <strong>{studentFullName}</strong>,
        </p>
        <p style='color:#475569; font-size:14px; line-height:1.6;'>
            Your application has been received and registered. Our admissions team will review your credentials and contact you shortly.
        </p>
        
        <div style='background:#f8fafc; border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:20px 0;'>
            <table style='width:100%; font-size:14px; color:#334155; border-collapse:collapse;'>
                <tr>
                    <td style='padding:6px 0; font-weight:600; width:45%; color:#64748b;'>Application Reference:</td>
                    <td style='padding:6px 0; font-weight:700; color:#2563eb;'>{app.AA_ApplicationNumber}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Applicant Name:</td>
                    <td style='padding:6px 0; font-weight:600;'>{studentFullName}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Applied Grade / Course:</td>
                    <td style='padding:6px 0;'>{gradeOrCourse}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Campus / Branch:</td>
                    <td style='padding:6px 0;'>{branchName}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Academic Session:</td>
                    <td style='padding:6px 0;'>{academicYearName}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Application Status:</td>
                    <td style='padding:6px 0;'><span style='background:#fef3c7; color:#92400e; padding:3px 8px; border-radius:6px; font-weight:600; font-size:12px;'>Submitted / Under Review</span></td>
                </tr>
            </table>
        </div>
        
        <p style='color:#64748b; font-size:13px; line-height:1.6; margin-bottom:0;'>
            Please retain your Application Reference Number (<strong>{app.AA_ApplicationNumber}</strong>) for any future queries or updates.
        </p>
    </div>
    <div style='background:#f1f5f9; padding:16px 24px; text-align:center; border-top:1px solid #e2e8f0;'>
        <p style='font-size:12px; color:#94a3b8; margin:0;'>&copy; IMS ERP Admissions Portal. Automated notification.</p>
    </div>
</div>";

            await _emailService.SendEmailAsync(recipientEmail, subject, bodyHtml, config);
        }

        private async Task SendAdmissionApprovalEmailAsync(Guid applicationId, string? reviewNotes, Guid tenantId)
        {
            var app = await _repo.GetByIdAsync(applicationId, tenantId);
            if (app == null) return;

            var recipientEmail = ResolveEmail(app.AA_Email, app.AA_FatherEmail, app.AA_MotherEmail, app.AA_GuardianEmail);
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;

            var config = await _configDAL.GetConfigAsync(tenantId);
            var studentFullName = JoinName(app.AA_FirstName, app.AA_MiddleName, app.AA_LastName);
            var gradeOrCourse = ResolveGradeOrCourse(app.AA_ClassId, app.AA_CourseId);
            var branchName = HardcodedMasterData.GetBranchName(app.AA_BranchId);
            var academicYearName = ResolveAcademicYearName(app.AA_AcademicYearId);

            var subject = $"[IMS ERP] Congratulations! Admission Application #{app.AA_ApplicationNumber} Approved";
            var bodyHtml = $@"
<div style='font-family:-apple-system,BlinkMacSystemFont,""Segoe UI"",Roboto,Helvetica,Arial,sans-serif; max-width:600px; margin:0 auto; background:#ffffff; border:1px solid #e2e8f0; border-radius:12px; overflow:hidden; box-shadow:0 4px 6px -1px rgba(0,0,0,0.1);'>
    <div style='background:linear-gradient(135deg, #059669 0%, #047857 100%); padding:28px 24px; text-align:center;'>
        <h2 style='color:#ffffff; margin:0 0 6px 0; font-size:22px; font-weight:700;'>IMS ERP Admissions</h2>
        <span style='background:#ffffff; color:#047857; font-size:12px; font-weight:700; padding:4px 14px; border-radius:20px; text-transform:uppercase; letter-spacing:0.5px;'>Admission Approved &#10004;</span>
    </div>
    <div style='padding:28px 24px;'>
        <h3 style='color:#065f46; margin-top:0;'>Congratulations! Your Application is Approved</h3>
        <p style='color:#334155; font-size:14px; line-height:1.6;'>
            Dear <strong>{studentFullName}</strong> &amp; Parents,
        </p>
        <p style='color:#334155; font-size:14px; line-height:1.6;'>
            We are pleased to inform you that your application for admission has been reviewed and <strong style='color:#059669;'>APPROVED</strong> by our admissions committee.
        </p>
        
        <div style='background:#f0fdf4; border:1px solid #bbf7d0; border-radius:8px; padding:16px; margin:20px 0;'>
            <table style='width:100%; font-size:14px; color:#1e293b; border-collapse:collapse;'>
                <tr>
                    <td style='padding:6px 0; font-weight:600; width:45%; color:#64748b;'>Application Reference:</td>
                    <td style='padding:6px 0; font-weight:700; color:#059669;'>{app.AA_ApplicationNumber}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Student Name:</td>
                    <td style='padding:6px 0; font-weight:600;'>{studentFullName}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Approved Grade / Course:</td>
                    <td style='padding:6px 0; font-weight:600;'>{gradeOrCourse}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Campus / Branch:</td>
                    <td style='padding:6px 0;'>{branchName}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Academic Session:</td>
                    <td style='padding:6px 0;'>{academicYearName}</td>
                </tr>
                {(string.IsNullOrWhiteSpace(reviewNotes) ? "" : $@"
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#64748b;'>Reviewer Remarks:</td>
                    <td style='padding:6px 0; color:#0f172a;'>{reviewNotes}</td>
                </tr>")}
            </table>
        </div>

        <div style='background:#f8fafc; border-left:4px solid #059669; padding:14px 16px; margin:20px 0; border-radius:0 8px 8px 0;'>
            <h4 style='color:#0f172a; margin:0 0 8px 0; font-size:14px;'>Next Steps for Final Enrollment:</h4>
            <ol style='margin:0; padding-left:20px; color:#475569; font-size:13px; line-height:1.6;'>
                <li><strong>Document Verification:</strong> Please bring original certificates (Birth Certificate, Marksheet/Transcript, Transfer Certificate, and Identity Proof) to the campus admissions office.</li>
                <li><strong>Fee Payment &amp; Seat Confirmation:</strong> Complete the admission fee payment to finalize enrollment and receive your official Student ID &amp; Portal Login Credentials.</li>
            </ol>
        </div>

        <p style='color:#64748b; font-size:13px; line-height:1.6; margin-bottom:0;'>
            If you have any questions or require assistance, please contact our Admissions Office.
        </p>
    </div>
    <div style='background:#f1f5f9; padding:16px 24px; text-align:center; border-top:1px solid #e2e8f0;'>
        <p style='font-size:12px; color:#94a3b8; margin:0;'>&copy; IMS ERP Admissions Portal. Automated notification.</p>
    </div>
</div>";

            await _emailService.SendEmailAsync(recipientEmail, subject, bodyHtml, config);
        }

        private async Task SendStudentEnrollmentCredentialsEmailAsync(AdmissionApplication app, Guid studentId, Enrollment enrollment, Guid tenantId)
        {
            var student = await _studentDAL.GetByIdAsync(studentId, tenantId);
            if (student == null) return;

            var recipientEmail = ResolveEmail(student.S_Email, app.AA_Email, app.AA_FatherEmail, app.AA_MotherEmail, app.AA_GuardianEmail);
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;

            var config = await _configDAL.GetConfigAsync(tenantId);
            var studentFullName = JoinName(student.S_FirstName, student.S_MiddleName, student.S_LastName);
            var className = HardcodedMasterData.GetClassName(student.S_ClassId);
            var sectionName = HardcodedMasterData.GetSectionName(enrollment.E_SectionId);
            var branchName = HardcodedMasterData.GetBranchName(student.S_BranchId);
            var academicYearName = ResolveAcademicYearName(enrollment.E_AcademicYearId);

            var loginIdentifier = !string.IsNullOrWhiteSpace(student.S_Email) ? student.S_Email : student.S_StudentCode;

            var subject = $"[IMS ERP] Welcome to Campus! Your Student ID & Portal Login Credentials - {studentFullName}";
            var bodyHtml = $@"
<div style='font-family:-apple-system,BlinkMacSystemFont,""Segoe UI"",Roboto,Helvetica,Arial,sans-serif; max-width:600px; margin:0 auto; background:#ffffff; border:1px solid #e2e8f0; border-radius:12px; overflow:hidden; box-shadow:0 4px 6px -1px rgba(0,0,0,0.1);'>
    <div style='background:linear-gradient(135deg, #1d4ed8 0%, #1e40af 100%); padding:28px 24px; text-align:center;'>
        <h2 style='color:#ffffff; margin:0 0 6px 0; font-size:22px; font-weight:700;'>IMS ERP Student Portal</h2>
        <span style='background:#38bdf8; color:#0f172a; font-size:12px; font-weight:700; padding:4px 14px; border-radius:20px; text-transform:uppercase; letter-spacing:0.5px;'>Student Enrollment Confirmed &#10004;</span>
    </div>
    <div style='padding:28px 24px;'>
        <h3 style='color:#1e3a8a; margin-top:0;'>Welcome to the Student &amp; Guardian Portal!</h3>
        <p style='color:#334155; font-size:14px; line-height:1.6;'>
            Dear <strong>{studentFullName}</strong>,
        </p>
        <p style='color:#334155; font-size:14px; line-height:1.6;'>
            Congratulations! You have been successfully enrolled as a student. Below are your official student profile details and your personal portal login credentials.
        </p>

        <!-- Credentials Highlight Box -->
        <div style='background:#eff6ff; border:2px solid #3b82f6; border-radius:10px; padding:18px; margin:22px 0;'>
            <h4 style='color:#1d4ed8; margin:0 0 12px 0; font-size:15px; text-transform:uppercase; letter-spacing:0.5px;'>Your Portal Login Credentials</h4>
            <table style='width:100%; font-size:14px; color:#1e293b; border-collapse:collapse;'>
                <tr>
                    <td style='padding:6px 0; font-weight:600; width:42%; color:#475569;'>Login Username / Email:</td>
                    <td style='padding:6px 0; font-weight:700; color:#1d4ed8; font-family:monospace; font-size:14px;'>{loginIdentifier}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#475569;'>Default Password:</td>
                    <td style='padding:6px 0;'><span style='font-weight:700; color:#0f172a; font-family:monospace; font-size:14px; background:#dbeafe; padding:2px 8px; border-radius:4px;'>Welcome@123</span></td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#475569;'>Student Code / User ID:</td>
                    <td style='padding:6px 0; font-weight:700; color:#0f172a;'>{student.S_StudentCode}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#475569;'>Admission Number:</td>
                    <td style='padding:6px 0;'>{student.S_AdmissionNumber}</td>
                </tr>
                <tr>
                    <td style='padding:6px 0; font-weight:600; color:#475569;'>Enrollment Number:</td>
                    <td style='padding:6px 0;'>{enrollment.E_EnrollmentNumber}</td>
                </tr>
            </table>

            <div style='text-align:center; margin-top:18px;'>
                <a href='/Student/Login' style='display:inline-block; background:#2563eb; color:#ffffff; padding:12px 28px; border-radius:8px; text-decoration:none; font-weight:700; font-size:14px;'>
                    Log In to Student Panel &rarr;
                </a>
            </div>
            <p style='font-size:12px; color:#64748b; text-align:center; margin:10px 0 0 0;'>
                Portal Link: <strong>/Student/Login</strong>
            </p>
        </div>

        <!-- Academic Information Box -->
        <div style='background:#f8fafc; border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:20px 0;'>
            <h4 style='color:#0f172a; margin:0 0 10px 0; font-size:14px;'>Academic Allocation Details</h4>
            <table style='width:100%; font-size:13px; color:#334155; border-collapse:collapse;'>
                <tr>
                    <td style='padding:4px 0; font-weight:600; width:45%; color:#64748b;'>Enrolled Class / Grade:</td>
                    <td style='padding:4px 0; font-weight:600;'>{className}</td>
                </tr>
                <tr>
                    <td style='padding:4px 0; font-weight:600; color:#64748b;'>Section &amp; Roll No:</td>
                    <td style='padding:4px 0;'>Section {sectionName} {(string.IsNullOrWhiteSpace(enrollment.E_RollNumber) ? "" : $"(Roll #{enrollment.E_RollNumber})")}</td>
                </tr>
                <tr>
                    <td style='padding:4px 0; font-weight:600; color:#64748b;'>Campus / Branch:</td>
                    <td style='padding:4px 0;'>{branchName}</td>
                </tr>
                <tr>
                    <td style='padding:4px 0; font-weight:600; color:#64748b;'>Academic Session:</td>
                    <td style='padding:4px 0;'>{academicYearName}</td>
                </tr>
            </table>
        </div>

        <!-- Student Features -->
        <div style='margin:20px 0; color:#475569; font-size:13px; line-height:1.6;'>
            <p style='margin:0 0 6px 0; font-weight:600; color:#0f172a;'>Through your Student &amp; Guardian Portal, you can:</p>
            <ul style='margin:0; padding-left:20px;'>
                <li>Check your daily class timetable and schedules</li>
                <li>Monitor live attendance records</li>
                <li>Submit home tasks and download study materials</li>
                <li>View exam results and download digital report cards</li>
                <li>Track fee invoices and download payment receipts</li>
            </ul>
        </div>

        <p style='color:#e11d48; font-size:12px; font-weight:600; margin:16px 0 0 0;'>
            &#9888; Security Tip: For your security, please update your temporary password immediately after logging in.
        </p>
    </div>
    <div style='background:#f1f5f9; padding:16px 24px; text-align:center; border-top:1px solid #e2e8f0;'>
        <p style='font-size:12px; color:#94a3b8; margin:0;'>&copy; IMS ERP Academic Management. Automated portal dispatch.</p>
    </div>
</div>";

            await _emailService.SendEmailAsync(recipientEmail, subject, bodyHtml, config);
        }

        private static string? ResolveEmail(params string?[] emails)
        {
            foreach (var email in emails)
            {
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                    return email.Trim();
            }
            return null;
        }

        private string ResolveGradeOrCourse(Guid? classId, Guid? courseId)
        {
            if (classId.HasValue)
            {
                var name = HardcodedMasterData.GetClassName(classId.Value);
                if (!string.IsNullOrWhiteSpace(name)) return name;
            }
            if (courseId.HasValue)
            {
                var c = _masterService.GetById("Course", courseId.Value);
                if (c != null && c.TryGetValue("C_Name", out var cName) && cName != null)
                    return cName.ToString();
            }
            return "General Academic";
        }

        private string ResolveAcademicYearName(Guid? academicYearId)
        {
            if (academicYearId.HasValue)
            {
                var ay = _masterService.GetById("AcademicYear", academicYearId.Value);
                if (ay != null && ay.TryGetValue("AY_Name", out var ayName) && ayName != null)
                    return ayName.ToString();
            }
            return DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1);
        }

        private static string JoinName(string? first, string? middle, string? last)
        {
            var parts = new[] { first, middle, last }.Where(s => !string.IsNullOrWhiteSpace(s));
            var full = string.Join(" ", parts);
            return string.IsNullOrWhiteSpace(full) ? "Student" : full;
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

            AA_StudentPhotoUrl = m.AA_StudentPhotoUrl,
            AA_BirthCertificateUrl = m.AA_BirthCertificateUrl,
            AA_TransferCertificateUrl = m.AA_TransferCertificateUrl,
            AA_MarksheetUrl = m.AA_MarksheetUrl,
            AA_NationalIdDocUrl = m.AA_NationalIdDocUrl,

            AA_MedicalConditions = m.AA_MedicalConditions,
            AA_RequiresTransport = m.AA_RequiresTransport,
            AA_TransportPickupPoint = m.AA_TransportPickupPoint,
            AA_RequiresHostel = m.AA_RequiresHostel,

            AA_SecondLanguage = m.AA_SecondLanguage,
            AA_MotherTongue = m.AA_MotherTongue,
            AA_HasSibling = m.AA_HasSibling,
            AA_SiblingDetails = m.AA_SiblingDetails,

            AA_FeeStructureId = m.AA_FeeStructureId,
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
