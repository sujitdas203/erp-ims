using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Entities;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentDAL _repo;
        private readonly IMasterService _masterService;

        public StudentService(IStudentDAL repo, IMasterService masterService)
        {
            _repo = repo;
            _masterService = masterService;
        }

        public async Task<StudentIndexViewModel> GetStudentListAsync(
            Guid tenantId, string searchTerm, string status, Guid? branchId, Guid? classId, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, totalCount) = await _repo.GetPagedAsync(tenantId, searchTerm, status, branchId, classId, pageNumber, pageSize);

            var vm = new StudentIndexViewModel
            {
                SearchTerm = searchTerm,
                StatusFilter = status,
                BranchFilter = branchId,
                ClassFilter = classId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                BranchOptions = HardcodedMasterData.GetBranchSelectList(branchId),
                StatusOptions = HardcodedMasterData.GetStatusSelectList(status),
                ClassOptions = HardcodedMasterData.GetClassSelectList(classId),
                BatchOptions = GetMasterSelectList("Batch")
            };

            foreach (var s in items)
            {
                if (classId.HasValue && s.S_ClassId != classId) continue;

                string batchName = "-";
                if (s.S_BatchId.HasValue)
                {
                    var bObj = _masterService.GetById("Batch", s.S_BatchId.Value);
                    if (bObj != null && bObj.TryGetValue("BT_Name", out var bName) && bName != null)
                        batchName = bName.ToString();
                }

                vm.Students.Add(new StudentListItemViewModel
                {
                    S_Id = s.S_Id,
                    S_StudentCode = s.S_StudentCode,
                    S_AdmissionNumber = s.S_AdmissionNumber,
                    FullName = JoinName(s.S_FirstName, s.S_MiddleName, s.S_LastName),
                    S_Gender = s.S_Gender,
                    S_Email = s.S_Email,
                    S_Phone = s.S_Phone,
                    S_DateOfBirth = s.S_DateOfBirth,
                    S_BloodGroup = s.S_BloodGroup,
                    FullAddress = JoinAddress(s),
                    BranchName = HardcodedMasterData.GetBranchName(s.S_BranchId),
                    ClassName = HardcodedMasterData.GetClassName(s.S_ClassId),
                    SectionName = HardcodedMasterData.GetSectionName(s.S_SectionId),
                    BatchName = batchName,
                    S_AdmissionDate = s.S_AdmissionDate,
                    S_Status = s.S_Status
                });
            }

            return vm;
        }

        public async Task<StudentDetailsViewModel> GetStudentDetailsAsync(Guid id, Guid tenantId)
        {
            var s = await _repo.GetByIdAsync(id, tenantId);
            if (s == null) return null;

            var guardianRecords = await _repo.GetGuardiansByStudentIdAsync(id);
            var feeSummary = await _repo.GetFeeSummaryAsync(id, tenantId);
            var attendanceSummary = await _repo.GetAttendanceSummaryAsync(id, tenantId);
            var leaveSummary = await _repo.GetLeaveSummaryAsync(id, tenantId);
            var enrollmentHistory = await _repo.GetEnrollmentHistoryAsync(id, tenantId);

            string batchName = "-";
            if (s.S_BatchId.HasValue)
            {
                var bObj = _masterService.GetById("Batch", s.S_BatchId.Value);
                if (bObj != null && bObj.TryGetValue("BT_Name", out var bName) && bName != null)
                    batchName = bName.ToString();
            }

            string feeStatus = "No Invoices";
            if (feeSummary.TotalInvoiced > 0)
            {
                if (feeSummary.BalanceAmount <= 0) feeStatus = "Paid";
                else if (feeSummary.TotalPaid > 0) feeStatus = "Partial";
                else feeStatus = "Unpaid";
            }

            var vm = new StudentDetailsViewModel
            {
                S_Id = s.S_Id,
                S_TenantId = s.S_TenantId,
                S_StudentCode = s.S_StudentCode,
                S_AdmissionNumber = s.S_AdmissionNumber,
                FullName = JoinName(s.S_FirstName, s.S_MiddleName, s.S_LastName),
                S_FirstName = s.S_FirstName,
                S_MiddleName = s.S_MiddleName,
                S_LastName = s.S_LastName,
                S_DateOfBirth = s.S_DateOfBirth,
                S_Gender = s.S_Gender,
                S_Email = s.S_Email,
                S_Phone = s.S_Phone,
                S_BranchId = s.S_BranchId,
                BranchName = HardcodedMasterData.GetBranchName(s.S_BranchId),
                S_ClassId = s.S_ClassId,
                ClassName = HardcodedMasterData.GetClassName(s.S_ClassId),
                S_SectionId = s.S_SectionId,
                SectionName = HardcodedMasterData.GetSectionName(s.S_SectionId),
                S_BatchId = s.S_BatchId,
                BatchName = batchName,
                S_BloodGroup = s.S_BloodGroup,
                FullAddress = JoinAddress(s),
                S_AddressLine1 = s.S_AddressLine1,
                S_AddressLine2 = s.S_AddressLine2,
                S_City = s.S_City,
                S_State = s.S_State,
                S_PostalCode = s.S_PostalCode,
                S_Country = s.S_Country,
                S_AdmissionDate = s.S_AdmissionDate,
                S_Status = s.S_Status,
                S_CreatedAt = s.S_CreatedAt,
                S_UpdatedAt = s.S_UpdatedAt,
                Guardians = guardianRecords.Select(MapGuardianRecordToRow).ToList(),

                // Financial
                TotalFeeInvoiced = feeSummary.TotalInvoiced,
                TotalFeePaid = feeSummary.TotalPaid,
                TotalFeeBalance = feeSummary.BalanceAmount,
                FeeStatus = feeStatus,
                RecentInvoices = feeSummary.RecentInvoices,

                // Attendance
                TotalAttendanceSessions = attendanceSummary.TotalSessions,
                PresentSessions = attendanceSummary.PresentSessions,
                AbsentSessions = attendanceSummary.AbsentSessions,
                AttendancePercentage = attendanceSummary.Percentage,

                // Leaves
                TotalLeavesTaken = leaveSummary.TotalLeaves,
                RecentLeaves = leaveSummary.RecentLeaves,

                // Academic History
                EnrollmentHistory = enrollmentHistory
            };

            PopulateDetailsDropdowns(vm);
            return vm;
        }

        public async Task<StudentFormViewModel> GetStudentForEditAsync(Guid id, Guid tenantId)
        {
            var s = await _repo.GetByIdAsync(id, tenantId);
            if (s == null) return null;

            var guardianRecords = await _repo.GetGuardiansByStudentIdAsync(id);

            var vm = new StudentFormViewModel
            {
                S_Id = s.S_Id,
                S_BranchId = s.S_BranchId,
                S_StudentCode = s.S_StudentCode,
                S_AdmissionNumber = s.S_AdmissionNumber,
                S_FirstName = s.S_FirstName,
                S_MiddleName = s.S_MiddleName,
                S_LastName = s.S_LastName,
                S_DateOfBirth = s.S_DateOfBirth,
                S_Gender = s.S_Gender,
                S_Email = s.S_Email,
                S_Phone = s.S_Phone,
                S_AdmissionDate = s.S_AdmissionDate,
                S_Status = s.S_Status,
                S_ClassId = s.S_ClassId,
                S_SectionId = s.S_SectionId,
                S_BatchId = s.S_BatchId,
                S_BloodGroup = s.S_BloodGroup,
                S_AddressLine1 = s.S_AddressLine1,
                S_AddressLine2 = s.S_AddressLine2,
                S_City = s.S_City,
                S_State = s.S_State,
                S_PostalCode = s.S_PostalCode,
                S_Country = s.S_Country,
                Guardians = guardianRecords.Select(MapGuardianRecordToRow).ToList()
            };

            PopulateDropdowns(vm);
            return vm;
        }

        public async Task<ServiceResult> CreateStudentAsync(StudentFormViewModel model, Guid tenantId)
        {
            // Blank admission number => DB auto-generates a guaranteed-unique one
            // (see SP_Students_Create); only check uniqueness if admin typed one in.
            if (!string.IsNullOrWhiteSpace(model.S_AdmissionNumber) &&
                await _repo.IsAdmissionNumberTakenAsync(tenantId, model.S_AdmissionNumber, null))
                return ServiceResult.Fail("This admission number is already in use.");

            var studentCode = string.IsNullOrWhiteSpace(model.S_StudentCode)
                ? GenerateStudentCode()
                : model.S_StudentCode;

            if (await _repo.IsStudentCodeTakenAsync(tenantId, studentCode, null))
                return ServiceResult.Fail("This student code is already in use.");

            var entity = MapToEntity(model, tenantId, Guid.NewGuid());
            entity.S_StudentCode = studentCode;

            var guardianInputs = MapGuardians(model.Guardians);

            var id = await _repo.CreateStudentWithGuardiansAsync(entity, guardianInputs);
            return ServiceResult.Ok("Student created successfully.", id);
        }

        public async Task<ServiceResult> UpdateStudentAsync(StudentFormViewModel model, Guid tenantId)
        {
            if (!model.S_Id.HasValue)
                return ServiceResult.Fail("Student Id is required for update.");

            var existing = await _repo.GetByIdAsync(model.S_Id.Value, tenantId);
            if (existing == null)
                return ServiceResult.Fail("Student not found or already removed.");

            var effectiveTenantId = existing.S_TenantId != Guid.Empty ? existing.S_TenantId : tenantId;

            // Preserve student code and admission number if not provided
            var studentCode = string.IsNullOrWhiteSpace(model.S_StudentCode) ? existing.S_StudentCode : model.S_StudentCode;
            var admissionNumber = string.IsNullOrWhiteSpace(model.S_AdmissionNumber) ? existing.S_AdmissionNumber : model.S_AdmissionNumber;

            if (!string.IsNullOrWhiteSpace(admissionNumber) &&
                await _repo.IsAdmissionNumberTakenAsync(effectiveTenantId, admissionNumber, model.S_Id))
                return ServiceResult.Fail("This admission number is already in use.");

            if (!string.IsNullOrWhiteSpace(studentCode) &&
                await _repo.IsStudentCodeTakenAsync(effectiveTenantId, studentCode, model.S_Id))
                return ServiceResult.Fail("This student code is already in use.");

            model.S_StudentCode = studentCode;
            model.S_AdmissionNumber = admissionNumber;

            var entity = MapToEntity(model, effectiveTenantId, model.S_Id.Value);
            var guardianInputs = MapGuardians(model.Guardians);

            var success = await _repo.UpdateStudentWithGuardiansAsync(entity, guardianInputs);
            return success
                ? ServiceResult.Ok("Student updated successfully.", model.S_Id)
                : ServiceResult.Fail("Unable to update student. Please check your details and try again.");
        }

        public async Task<ServiceResult> QuickUpdateStudentAsync(QuickStudentUpdateModel model, Guid tenantId)
        {
            if (model == null || model.S_Id == Guid.Empty)
                return ServiceResult.Fail("Student Id is required.");

            var existing = await _repo.GetByIdAsync(model.S_Id, tenantId);
            if (existing == null)
                return ServiceResult.Fail("Student not found or already removed.");

            // Update only the supplied fields
            if (!string.IsNullOrWhiteSpace(model.S_FirstName)) existing.S_FirstName = model.S_FirstName.Trim();
            if (model.S_MiddleName != null) existing.S_MiddleName = model.S_MiddleName.Trim();
            if (!string.IsNullOrWhiteSpace(model.S_LastName)) existing.S_LastName = model.S_LastName.Trim();
            if (!string.IsNullOrWhiteSpace(model.S_Status)) existing.S_Status = model.S_Status.Trim();
            if (model.S_Phone != null) existing.S_Phone = model.S_Phone.Trim();
            if (model.S_Email != null) existing.S_Email = model.S_Email.Trim();
            if (model.S_DateOfBirth.HasValue) existing.S_DateOfBirth = model.S_DateOfBirth;
            if (!string.IsNullOrWhiteSpace(model.S_Gender)) existing.S_Gender = model.S_Gender.Trim();
            if (!string.IsNullOrWhiteSpace(model.S_BloodGroup)) existing.S_BloodGroup = model.S_BloodGroup.Trim();
            if (model.S_BranchId.HasValue && model.S_BranchId != Guid.Empty) existing.S_BranchId = model.S_BranchId.Value;
            if (model.S_ClassId.HasValue) existing.S_ClassId = model.S_ClassId.Value != Guid.Empty ? model.S_ClassId : null;
            if (model.S_SectionId.HasValue) existing.S_SectionId = model.S_SectionId.Value != Guid.Empty ? model.S_SectionId : null;
            if (model.S_BatchId.HasValue) existing.S_BatchId = model.S_BatchId.Value != Guid.Empty ? model.S_BatchId : null;
            if (model.S_AdmissionDate.HasValue) existing.S_AdmissionDate = model.S_AdmissionDate;
            if (model.S_AddressLine1 != null) existing.S_AddressLine1 = model.S_AddressLine1.Trim();
            if (model.S_AddressLine2 != null) existing.S_AddressLine2 = model.S_AddressLine2.Trim();
            if (model.S_City != null) existing.S_City = model.S_City.Trim();
            if (model.S_State != null) existing.S_State = model.S_State.Trim();
            if (model.S_PostalCode != null) existing.S_PostalCode = model.S_PostalCode.Trim();
            if (model.S_Country != null) existing.S_Country = model.S_Country.Trim();

            var success = await _repo.QuickUpdateAsync(existing);
            return success
                ? ServiceResult.Ok("Student updated successfully.", existing.S_Id)
                : ServiceResult.Fail("Unable to update student.");
        }

        public async Task<ServiceResult> DeleteStudentAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.SoftDeleteAsync(id, tenantId);
            return success
                ? ServiceResult.Ok("Student removed successfully.")
                : ServiceResult.Fail("Unable to remove student.");
        }

        public void PopulateDetailsDropdowns(StudentDetailsViewModel vm)
        {
            vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.S_BranchId);
            vm.ClassOptions = HardcodedMasterData.GetClassSelectList(vm.S_ClassId);
            vm.SectionOptions = HardcodedMasterData.GetSectionSelectList(vm.S_SectionId);
            vm.BatchOptions = GetMasterSelectList("Batch", vm.S_BatchId?.ToString());
            vm.StatusOptions = HardcodedMasterData.GetStatusSelectList(vm.S_Status);
            vm.GenderOptions = HardcodedMasterData.GetGenderSelectList(vm.S_Gender);
            vm.BloodGroupOptions = HardcodedMasterData.GetBloodGroupSelectList(vm.S_BloodGroup);
        }

        // ---------- helpers ----------

        public void PopulateDropdowns(StudentFormViewModel vm)
        {
            vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.S_BranchId);
            vm.GenderOptions = HardcodedMasterData.GetGenderSelectList(vm.S_Gender);
            vm.StatusOptions = HardcodedMasterData.GetStatusSelectList(vm.S_Status);
            vm.ClassOptions = HardcodedMasterData.GetClassSelectList(vm.S_ClassId);
            vm.SectionOptions = HardcodedMasterData.GetSectionSelectList(vm.S_SectionId);
            vm.BatchOptions = GetMasterSelectList("Batch", vm.S_BatchId?.ToString());
            vm.BloodGroupOptions = HardcodedMasterData.GetBloodGroupSelectList(vm.S_BloodGroup);
            vm.RelationOptions = HardcodedMasterData.GetRelationSelectList();
        }

        private List<SelectListItem> GetMasterSelectList(string entityType, string selectedValue = null)
        {
            var items = _masterService?.GetAll(entityType);
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

        private static Student MapToEntity(StudentFormViewModel m, Guid tenantId, Guid id) => new()
        {
            S_Id = id,
            S_TenantId = tenantId,
            S_BranchId = m.S_BranchId,
            S_StudentCode = m.S_StudentCode,
            S_AdmissionNumber = m.S_AdmissionNumber,
            S_FirstName = m.S_FirstName,
            S_MiddleName = m.S_MiddleName,
            S_LastName = m.S_LastName,
            S_DateOfBirth = m.S_DateOfBirth,
            S_Gender = m.S_Gender,
            S_Email = m.S_Email,
            S_Phone = m.S_Phone,
            S_AdmissionDate = m.S_AdmissionDate,
            S_Status = m.S_Status,
            S_ClassId = m.S_ClassId,
            S_SectionId = m.S_SectionId,
            S_BatchId = m.S_BatchId,
            S_BloodGroup = m.S_BloodGroup,
            S_AddressLine1 = m.S_AddressLine1,
            S_AddressLine2 = m.S_AddressLine2,
            S_City = m.S_City,
            S_State = m.S_State,
            S_PostalCode = m.S_PostalCode,
            S_Country = m.S_Country
        };

        private static List<GuardianLinkInput> MapGuardians(List<GuardianRowViewModel> rows) =>
            (rows ?? new List<GuardianRowViewModel>())
                .Where(r => r.ExistingGuardianId.HasValue || !string.IsNullOrWhiteSpace(r.FirstName))
                .Select(r => new GuardianLinkInput
                {
                    ExistingGuardianId = r.ExistingGuardianId,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Phone = r.Phone,
                    Email = r.Email,
                    Occupation = r.Occupation,
                    Relation = r.Relation,
                    IsPrimary = r.IsPrimary
                })
                .ToList();

        private static GuardianRowViewModel MapGuardianRecordToRow(StudentGuardianRecord r) => new()
        {
            ExistingGuardianId = r.SG_GuardianId,
            FirstName = r.G_FirstName,
            LastName = r.G_LastName,
            Phone = r.G_Phone,
            Email = r.G_Email,
            Occupation = r.G_Occupation,
            Relation = r.SG_Relation,
            IsPrimary = r.SG_IsPrimary
        };

        private static string JoinName(string first, string middle, string last) =>
            string.Join(" ", new[] { first, middle, last }.Where(p => !string.IsNullOrWhiteSpace(p)));

        private static string JoinAddress(Student s)
        {
            var parts = new[] { s.S_AddressLine1, s.S_AddressLine2, s.S_City, s.S_State, s.S_PostalCode, s.S_Country }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            var joined = string.Join(", ", parts);
            return string.IsNullOrWhiteSpace(joined) ? "-" : joined;
        }

        private static string GenerateStudentCode()
        {
            var suffix = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
            return $"STU-{DateTime.UtcNow:yyMMdd}-{suffix}";
        }
    }
}
