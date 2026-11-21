using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Common.Dropdown;
using IMS.Models.Entities;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IDropdownService _dropdownService;
        private readonly IStudentDAL _studentDal;

        public EnrollmentService(IEnrollmentDAL repo, IMasterService masterService, IDropdownService dropdownService, IStudentDAL studentDal)
        {
            _repo = repo;
            _masterService = masterService;
            _dropdownService = dropdownService;
            _studentDal = studentDal;
        }

        public async Task<EnrollmentIndexViewModel> GetListAsync(Guid tenantId, string? searchTerm, Guid? academicYearId,
            Guid? courseId, Guid? batchId, string? status, int page, int pageSize,
            Guid? branchId = null, Guid? classId = null, Guid? sectionId = null)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, totalCount) = await _repo.GetPagedAsync(tenantId, searchTerm, academicYearId, courseId, batchId, status, page, pageSize, branchId, classId, sectionId);

            var vm = new EnrollmentIndexViewModel
            {
                SearchTerm = searchTerm,
                AcademicYearFilter = academicYearId,
                BranchFilter = branchId,
                ClassFilter = classId,
                SectionFilter = sectionId,
                CourseFilter = courseId,
                BatchFilter = batchId,
                StatusFilter = status,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                AcademicYearOptions = GetDropdownSelectList("AcademicYear", academicYearId?.ToString()),
                BranchOptions = HardcodedMasterData.GetBranchSelectList(branchId),
                ClassOptions = HardcodedMasterData.GetClassSelectList(classId),
                SectionOptions = HardcodedMasterData.GetSectionSelectList(sectionId),
                CourseOptions = GetDropdownSelectList("Course", courseId?.ToString()),
                BatchOptions = GetDropdownSelectList("Batch", batchId?.ToString()),
                StatusOptions = HardcodedMasterData.GetEnrollmentStatusSelectList(status)
            };

            foreach (var e in items)
            {
                var className = !string.IsNullOrWhiteSpace(e.ClassName) ? e.ClassName : HardcodedMasterData.GetClassName(e.E_ClassId);
                var sectionName = !string.IsNullOrWhiteSpace(e.SectionName) ? e.SectionName : HardcodedMasterData.GetSectionName(e.E_SectionId);
                var branchName = !string.IsNullOrWhiteSpace(e.BranchName) ? e.BranchName : HardcodedMasterData.GetBranchName(e.E_BranchId);

                vm.Enrollments.Add(new EnrollmentListItemViewModel
                {
                    E_Id = e.E_Id,
                    E_EnrollmentNumber = e.E_EnrollmentNumber ?? "-",
                    StudentName = !string.IsNullOrWhiteSpace(e.StudentName) ? e.StudentName : (!string.IsNullOrWhiteSpace(e.StudentCode) ? e.StudentCode : "-"),
                    StudentCode = e.StudentCode,
                    BranchName = branchName,
                    ClassName = className,
                    SectionName = sectionName,
                    CourseName = e.CourseName ?? "-",
                    BatchName = e.BatchName ?? "-",
                    AcademicYearName = e.AcademicYearName ?? "-",
                    E_RollNumber = e.E_RollNumber,
                    E_EnrollmentType = e.E_EnrollmentType ?? "New Admission",
                    E_EnrollmentDate = e.E_EnrollmentDate,
                    E_Status = e.E_Status
                });
            }

            return vm;
        }

        public async Task<EnrollmentDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId)
        {
            var e = await _repo.GetByIdAsync(id, tenantId);
            if (e == null) return null;

            var className = !string.IsNullOrWhiteSpace(e.ClassName) ? e.ClassName : HardcodedMasterData.GetClassName(e.E_ClassId);
            var sectionName = !string.IsNullOrWhiteSpace(e.SectionName) ? e.SectionName : HardcodedMasterData.GetSectionName(e.E_SectionId);
            var branchName = !string.IsNullOrWhiteSpace(e.BranchName) ? e.BranchName : HardcodedMasterData.GetBranchName(e.E_BranchId);

            return new EnrollmentDetailsViewModel
            {
                E_Id = e.E_Id,
                E_EnrollmentNumber = e.E_EnrollmentNumber ?? "-",
                StudentName = !string.IsNullOrWhiteSpace(e.StudentName) ? e.StudentName : "-",
                StudentAdmissionNumber = e.StudentAdmissionNumber,
                StudentCode = e.StudentCode,
                StudentPhone = e.StudentPhone,
                StudentEmail = e.StudentEmail,
                BranchName = branchName,
                ClassName = className,
                SectionName = sectionName,
                CourseName = e.CourseName ?? "-",
                BatchName = e.BatchName ?? "-",
                AcademicYearName = e.AcademicYearName ?? "-",
                E_RollNumber = e.E_RollNumber ?? "-",
                E_EnrollmentType = e.E_EnrollmentType ?? "New Admission",
                E_EnrollmentDate = e.E_EnrollmentDate,
                E_Status = e.E_Status,
                E_CompletionDate = e.E_CompletionDate,
                E_Remarks = e.E_Remarks,
                E_CreatedAt = e.E_CreatedAt,
                E_UpdatedAt = e.E_UpdatedAt
            };
        }

        public async Task<EnrollmentFormViewModel?> GetForEditAsync(Guid id, Guid tenantId)
        {
            var e = await _repo.GetByIdAsync(id, tenantId);
            if (e == null) return null;

            var vm = new EnrollmentFormViewModel
            {
                E_Id = e.E_Id,
                E_BranchId = e.E_BranchId,
                E_StudentId = e.E_StudentId,
                E_AcademicYearId = e.E_AcademicYearId,
                E_ClassId = e.E_ClassId,
                E_SectionId = e.E_SectionId,
                E_CourseId = e.E_CourseId,
                E_BatchId = e.E_BatchId,
                E_EnrollmentNumber = e.E_EnrollmentNumber,
                E_RollNumber = e.E_RollNumber,
                E_EnrollmentType = e.E_EnrollmentType ?? "New Admission",
                E_EnrollmentDate = e.E_EnrollmentDate,
                E_Status = e.E_Status,
                E_CompletionDate = e.E_CompletionDate,
                E_FeeStructureId = e.E_FeeStructureId,
                E_Remarks = e.E_Remarks
            };
            PopulateDropdowns(vm);
            return vm;
        }

        public async Task<ServiceResult> CreateAsync(EnrollmentFormViewModel model, Guid tenantId)
        {
            if (model.E_StudentId == Guid.Empty)
                return ServiceResult.Fail("Please select a student.");

            if (model.E_AcademicYearId == Guid.Empty)
                return ServiceResult.Fail("Please select an academic year.");

            if (await _repo.IsDuplicateAsync(tenantId, model.E_StudentId, model.E_AcademicYearId, model.E_ClassId, model.E_BatchId, model.E_CourseId, null))
                return ServiceResult.Fail("This student is already enrolled in this class/batch for the selected academic year.");

            var entity = MapToEntity(model, tenantId, Guid.NewGuid());
            var id = await _repo.CreateAsync(entity);
            return ServiceResult.Ok("Enrollment created successfully.", id);
        }

        public async Task<ServiceResult> UpdateAsync(EnrollmentFormViewModel model, Guid tenantId)
        {
            if (!model.E_Id.HasValue || model.E_Id.Value == Guid.Empty)
                return ServiceResult.Fail("Enrollment Id is required.");

            if (model.E_StudentId == Guid.Empty)
                return ServiceResult.Fail("Please select a student.");

            if (model.E_AcademicYearId == Guid.Empty)
                return ServiceResult.Fail("Please select an academic year.");

            if (await _repo.IsDuplicateAsync(tenantId, model.E_StudentId, model.E_AcademicYearId, model.E_ClassId, model.E_BatchId, model.E_CourseId, model.E_Id))
                return ServiceResult.Fail("This student is already enrolled in this class/batch for the selected academic year.");

            var entity = MapToEntity(model, tenantId, model.E_Id.Value);
            var success = await _repo.UpdateAsync(entity);
            return success
                ? ServiceResult.Ok("Enrollment updated successfully.", model.E_Id)
                : ServiceResult.Fail("Enrollment not found or could not be updated.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(id, tenantId);
            return success
                ? ServiceResult.Ok("Enrollment deleted successfully.")
                : ServiceResult.Fail("Unable to delete enrollment.");
        }

        public void PopulateDropdowns(EnrollmentFormViewModel vm)
        {
            vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.E_BranchId);
            vm.ClassOptions = HardcodedMasterData.GetClassSelectList(vm.E_ClassId);
            vm.SectionOptions = HardcodedMasterData.GetSectionSelectList(vm.E_SectionId);
            vm.EnrollmentTypeOptions = HardcodedMasterData.GetEnrollmentTypeSelectList(vm.E_EnrollmentType);
            vm.StatusOptions = HardcodedMasterData.GetEnrollmentStatusSelectList(vm.E_Status);

            vm.StudentOptions = GetDropdownSelectList("Student", vm.E_StudentId != Guid.Empty ? vm.E_StudentId.ToString() : null);
            vm.AcademicYearOptions = GetDropdownSelectList("AcademicYear", vm.E_AcademicYearId != Guid.Empty ? vm.E_AcademicYearId.ToString() : null);
            vm.CourseOptions = GetDropdownSelectList("Course", vm.E_CourseId?.ToString());
            vm.BatchOptions = GetDropdownSelectList("Batch", vm.E_BatchId?.ToString());
            vm.FeeStructureOptions = GetDropdownSelectList("FeeStructure", vm.E_FeeStructureId?.ToString());
        }

        private List<SelectListItem> GetDropdownSelectList(string entityType, string? selectedValue = null)
        {
            try
            {
                var req = new DropdownRequestModel { EntityType = entityType, ActiveOnly = false };
                var items = _dropdownService.GetDropdown(req);
                if (items != null && items.Count > 0)
                {
                    return items.Select(i => new SelectListItem
                    {
                        Value = i.Value,
                        Text = !string.IsNullOrWhiteSpace(i.Code) && !i.Text.Contains(i.Code) ? $"{i.Text} ({i.Code})" : i.Text,
                        Selected = string.Equals(i.Value, selectedValue, StringComparison.OrdinalIgnoreCase)
                    }).ToList();
                }
            }
            catch
            {
                // Fallback to MasterService or safe empty list
            }

            try
            {
                var masterItems = _masterService.GetAll(entityType);
                if (masterItems != null && masterItems.Count > 0)
                {
                    var list = new List<SelectListItem>();
                    foreach (var item in masterItems)
                    {
                        var keyEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Id"));
                        var id = keyEntry.Value?.ToString() ?? "";

                        string? displayName = null;
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
            }
            catch
            {
                // return empty list
            }

            return new List<SelectListItem>();
        }

        private static Enrollment MapToEntity(EnrollmentFormViewModel m, Guid tenantId, Guid id) => new()
        {
            E_Id = id,
            E_TenantId = tenantId,
            E_BranchId = m.E_BranchId,
            E_StudentId = m.E_StudentId,
            E_AcademicYearId = m.E_AcademicYearId,
            E_ClassId = m.E_ClassId,
            E_SectionId = m.E_SectionId,
            E_CourseId = m.E_CourseId,
            E_BatchId = m.E_BatchId,
            E_EnrollmentNumber = string.IsNullOrWhiteSpace(m.E_EnrollmentNumber) ? null : m.E_EnrollmentNumber.Trim(),
            E_RollNumber = string.IsNullOrWhiteSpace(m.E_RollNumber) ? null : m.E_RollNumber.Trim(),
            E_EnrollmentType = m.E_EnrollmentType,
            E_EnrollmentDate = m.E_EnrollmentDate,
            E_Status = m.E_Status,
            E_CompletionDate = m.E_CompletionDate,
            E_FeeStructureId = m.E_FeeStructureId,
            E_Remarks = string.IsNullOrWhiteSpace(m.E_Remarks) ? null : m.E_Remarks.Trim()
        };

        public async Task<StudentPromotionRequestViewModel?> GetPromotionFormForStudentAsync(Guid studentId, Guid tenantId)
        {
            var student = await _studentDal.GetByIdAsync(studentId, tenantId);
            if (student == null) return null;

            var activeEnrollment = await _repo.GetActiveEnrollmentByStudentAsync(studentId, tenantId);
            var nextClassId = GetNextClassIdInSequence(student.S_ClassId) ?? student.S_ClassId ?? Guid.Empty;

            var vm = new StudentPromotionRequestViewModel
            {
                StudentId = student.S_Id,
                StudentName = $"{student.S_FirstName} {student.S_LastName}".Trim(),
                StudentCode = student.S_StudentCode,
                AdmissionNumber = student.S_AdmissionNumber,
                CurrentBranchId = student.S_BranchId,
                CurrentBranchName = HardcodedMasterData.GetBranchName(student.S_BranchId),
                CurrentAcademicYearId = activeEnrollment?.E_AcademicYearId,
                CurrentAcademicYearName = activeEnrollment?.AcademicYearName ?? "-",
                CurrentClassId = student.S_ClassId,
                CurrentClassName = HardcodedMasterData.GetClassName(student.S_ClassId),
                CurrentSectionId = student.S_SectionId,
                CurrentSectionName = HardcodedMasterData.GetSectionName(student.S_SectionId),
                CurrentRollNumber = activeEnrollment?.E_RollNumber ?? "-",
                NextClassId = nextClassId,
                NextSectionId = student.S_SectionId,
                NewRollNumber = activeEnrollment?.E_RollNumber,
                EnrollmentType = "Promotion",
                AcademicYearOptions = GetDropdownSelectList("AcademicYear"),
                ClassOptions = HardcodedMasterData.GetClassSelectList(nextClassId),
                SectionOptions = HardcodedMasterData.GetSectionSelectList(student.S_SectionId),
                EnrollmentTypeOptions = HardcodedMasterData.GetEnrollmentTypeSelectList("Promotion"),
                FeeStructureOptions = GetDropdownSelectList("FeeStructure")
            };

            return vm;
        }

        public async Task<ServiceResult> PromoteStudentAsync(StudentPromotionRequestViewModel model, Guid tenantId)
        {
            if (model.StudentId == Guid.Empty)
                return ServiceResult.Fail("Student is required.");

            if (model.NextAcademicYearId == Guid.Empty)
                return ServiceResult.Fail("Please select the destination academic year.");

            if (model.NextClassId == Guid.Empty)
                return ServiceResult.Fail("Please select the destination class / grade.");

            var currentYearId = model.CurrentAcademicYearId ?? Guid.Empty;
            if (currentYearId == Guid.Empty)
            {
                var active = await _repo.GetActiveEnrollmentByStudentAsync(model.StudentId, tenantId);
                if (active != null) currentYearId = active.E_AcademicYearId;
            }

            var (success, message, newId) = await _repo.PromoteSingleStudentAsync(
                tenantId,
                model.StudentId,
                currentYearId,
                model.NextAcademicYearId,
                model.NextClassId,
                model.NextSectionId,
                model.NewRollNumber,
                model.EnrollmentType,
                model.NextFeeStructureId,
                model.Remarks,
                model.CurrentBranchId
            );

            return success
                ? ServiceResult.Ok(message, newId)
                : ServiceResult.Fail(message);
        }

        public async Task<List<StudentEnrollmentHistoryViewModel>> GetEnrollmentHistoryAsync(Guid studentId, Guid tenantId)
        {
            return await _repo.GetEnrollmentHistoryByStudentIdAsync(studentId, tenantId);
        }

        public async Task<BulkPromotionIndexViewModel> GetBulkPromotionViewModelAsync(
            Guid tenantId, Guid? sourceBranchId, Guid? sourceAcademicYearId, Guid? sourceClassId, Guid? sourceSectionId)
        {
            var vm = new BulkPromotionIndexViewModel
            {
                SourceBranchId = sourceBranchId,
                SourceAcademicYearId = sourceAcademicYearId,
                SourceClassId = sourceClassId,
                SourceSectionId = sourceSectionId,
                BranchOptions = HardcodedMasterData.GetBranchSelectList(sourceBranchId),
                AcademicYearOptions = GetDropdownSelectList("AcademicYear", sourceAcademicYearId?.ToString()),
                ClassOptions = HardcodedMasterData.GetClassSelectList(sourceClassId),
                SectionOptions = HardcodedMasterData.GetSectionSelectList(sourceSectionId),
                FeeStructureOptions = GetDropdownSelectList("FeeStructure"),
                DefaultEnrollmentType = "Promotion"
            };

            if (sourceClassId.HasValue)
            {
                var nextClass = GetNextClassIdInSequence(sourceClassId.Value);
                vm.TargetClassId = nextClass ?? sourceClassId.Value;
                vm.TargetSectionId = sourceSectionId;
            }

            if (sourceAcademicYearId.HasValue && sourceClassId.HasValue)
            {
                vm.Students = await _repo.GetStudentsForPromotionAsync(
                    tenantId, sourceBranchId, sourceAcademicYearId.Value, sourceClassId.Value, sourceSectionId);
            }

            return vm;
        }

        public async Task<List<BulkPromotionStudentItemViewModel>> GetStudentsForPromotionAsync(
            Guid tenantId, Guid? branchId, Guid academicYearId, Guid classId, Guid? sectionId)
        {
            return await _repo.GetStudentsForPromotionAsync(tenantId, branchId, academicYearId, classId, sectionId);
        }

        public async Task<BulkPromotionResultViewModel> ProcessBulkPromotionAsync(BulkPromotionSubmitViewModel model, Guid tenantId)
        {
            var res = new BulkPromotionResultViewModel();
            if (model.Students == null || model.Students.Count == 0)
            {
                res.Success = false;
                res.Message = "No students selected for promotion.";
                return res;
            }

            var selected = model.Students.Where(s => s.IsSelected).ToList();
            if (selected.Count == 0)
            {
                res.Success = false;
                res.Message = "Please select at least one student.";
                return res;
            }

            res.TotalCount = selected.Count;

            foreach (var student in selected)
            {
                if (student.AlreadyEnrolledInNextYear)
                {
                    res.SkippedCount++;
                    res.Errors.Add($"{student.FullName} is already enrolled in the destination year.");
                    continue;
                }

                Guid targetClassId = model.TargetClassId;
                string enrollmentType = "Promotion";

                if (string.Equals(student.Action, "Repeat", StringComparison.OrdinalIgnoreCase))
                {
                    enrollmentType = "Repeat";
                    var st = await _studentDal.GetByIdAsync(student.StudentId, tenantId);
                    if (st?.S_ClassId != null) targetClassId = st.S_ClassId.Value;
                }
                else if (string.Equals(student.Action, "Leave", StringComparison.OrdinalIgnoreCase))
                {
                    var current = await _repo.GetActiveEnrollmentByStudentAsync(student.StudentId, tenantId);
                    if (current != null)
                    {
                        current.E_Status = "Withdrawn";
                        current.E_CompletionDate = DateTime.Today;
                        await _repo.UpdateAsync(current);
                    }
                    res.RetainedCount++;
                    continue;
                }

                var (success, msg, _) = await _repo.PromoteSingleStudentAsync(
                    tenantId,
                    student.StudentId,
                    model.SourceAcademicYearId,
                    model.TargetAcademicYearId,
                    targetClassId,
                    student.TargetSectionId ?? model.TargetSectionId,
                    student.NextRollNumber ?? student.CurrentRollNumber,
                    enrollmentType,
                    model.TargetFeeStructureId,
                    $"Bulk {enrollmentType} processed on {DateTime.Today:dd-MMM-yyyy}",
                    model.TargetBranchId
                );

                if (success)
                {
                    if (enrollmentType == "Promotion") res.PromotedCount++;
                    else res.RetainedCount++;
                }
                else
                {
                    res.SkippedCount++;
                    res.Errors.Add($"{student.FullName}: {msg}");
                }
            }

            res.Success = res.PromotedCount > 0 || res.RetainedCount > 0;
            res.Message = $"Promotion completed. {res.PromotedCount} promoted, {res.RetainedCount} retained, {res.SkippedCount} skipped.";
            return res;
        }

        private static Guid? GetNextClassIdInSequence(Guid? currentClassId)
        {
            if (!currentClassId.HasValue) return null;
            var classes = HardcodedMasterData.Classes;
            var idx = classes.FindIndex(c => c.Id == currentClassId.Value);
            if (idx >= 0 && idx < classes.Count - 1)
            {
                return classes[idx + 1].Id;
            }
            return null;
        }
    }
}
