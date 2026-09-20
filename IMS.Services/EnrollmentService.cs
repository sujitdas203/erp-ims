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

        public EnrollmentService(IEnrollmentDAL repo, IMasterService masterService, IDropdownService dropdownService)
        {
            _repo = repo;
            _masterService = masterService;
            _dropdownService = dropdownService;
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
    }
}
