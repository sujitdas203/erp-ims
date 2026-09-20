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
    public class StaffService : IStaffService
    {
        private readonly IStaffDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IDropdownService _dropdownService;

        public StaffService(IStaffDAL repo, IMasterService masterService, IDropdownService dropdownService)
        {
            _repo = repo;
            _masterService = masterService;
            _dropdownService = dropdownService;
        }

        public async Task<StaffIndexViewModel> GetStaffListAsync(
            Guid tenantId, string searchTerm, Guid? branchId, string status,
            int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, totalCount) = await _repo.GetPagedAsync(
                tenantId, searchTerm, branchId, status, pageNumber, pageSize);

            var vm = new StaffIndexViewModel
            {
                SearchTerm = searchTerm,
                BranchFilter = branchId,
                StatusFilter = status,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                BranchOptions = GetDropdownSelectList("Branch", branchId?.ToString()),
                StatusOptions = GetStaffStatusSelectList(status)
            };

            foreach (var s in items)
            {
                vm.StaffList.Add(new StaffListItemViewModel
                {
                    ST_Id = s.ST_Id,
                    ST_EmployeeCode = s.ST_EmployeeCode,
                    ST_FirstName = s.ST_FirstName,
                    ST_LastName = s.ST_LastName,
                    ST_Gender = s.ST_Gender,
                    ST_Email = s.ST_Email,
                    ST_Phone = s.ST_Phone,
                    BranchName = HardcodedMasterData.GetBranchName(s.ST_BranchId),
                    DepartmentName = s.DepartmentName ?? "-",
                    DesignationName = s.DesignationName ?? "-",
                    ST_JoiningDate = s.ST_JoiningDate,
                    ST_Status = s.ST_Status
                });
            }

            return vm;
        }

        public async Task<StaffDetailsViewModel> GetStaffDetailsAsync(Guid id, Guid tenantId)
        {
            var s = await _repo.GetByIdAsync(id, tenantId);
            if (s == null) return null;

            return new StaffDetailsViewModel
            {
                ST_Id = s.ST_Id,
                ST_EmployeeCode = s.ST_EmployeeCode,
                ST_FirstName = s.ST_FirstName,
                ST_LastName = s.ST_LastName,
                ST_Gender = s.ST_Gender,
                ST_DateOfBirth = s.ST_DateOfBirth,
                ST_BloodGroup = s.ST_BloodGroup,
                ST_Qualification = s.ST_Qualification,
                ST_ExperienceYears = s.ST_ExperienceYears,
                ST_Address = s.ST_Address,
                ST_EmergencyContact = s.ST_EmergencyContact,
                ST_BasicSalary = s.ST_BasicSalary,
                ST_Email = s.ST_Email,
                ST_Phone = s.ST_Phone,
                BranchName = HardcodedMasterData.GetBranchName(s.ST_BranchId),
                DepartmentName = s.DepartmentName ?? "-",
                DesignationName = s.DesignationName ?? "-",
                ST_JoiningDate = s.ST_JoiningDate,
                ST_Status = s.ST_Status,
                ST_CreatedAt = s.ST_CreatedAt,
                ST_UpdatedAt = s.ST_UpdatedAt
            };
        }

        public async Task<StaffFormViewModel> GetStaffForEditAsync(Guid id, Guid tenantId)
        {
            var s = await _repo.GetByIdAsync(id, tenantId);
            if (s == null) return null;

            var vm = new StaffFormViewModel
            {
                ST_Id = s.ST_Id,
                ST_BranchId = s.ST_BranchId,
                ST_DepartmentId = s.ST_DepartmentId,
                ST_DesignationId = s.ST_DesignationId,
                ST_EmployeeCode = s.ST_EmployeeCode,
                ST_FirstName = s.ST_FirstName,
                ST_LastName = s.ST_LastName,
                ST_Gender = s.ST_Gender,
                ST_DateOfBirth = s.ST_DateOfBirth,
                ST_BloodGroup = s.ST_BloodGroup,
                ST_Qualification = s.ST_Qualification,
                ST_ExperienceYears = s.ST_ExperienceYears,
                ST_Address = s.ST_Address,
                ST_EmergencyContact = s.ST_EmergencyContact,
                ST_BasicSalary = s.ST_BasicSalary,
                ST_Email = s.ST_Email,
                ST_Phone = s.ST_Phone,
                ST_JoiningDate = s.ST_JoiningDate,
                ST_Status = s.ST_Status
            };

            PopulateDropdowns(vm);
            return vm;
        }

        public async Task<ServiceResult> CreateStaffAsync(StaffFormViewModel model, Guid tenantId)
        {
            if (model == null) return ServiceResult.Fail("Form data is required.");
            if (model.ST_BranchId == Guid.Empty) return ServiceResult.Fail("Please select a Branch.");
            if (string.IsNullOrWhiteSpace(model.ST_EmployeeCode)) return ServiceResult.Fail("Employee code is required.");
            if (string.IsNullOrWhiteSpace(model.ST_FirstName)) return ServiceResult.Fail("First name is required.");
            if (string.IsNullOrWhiteSpace(model.ST_LastName)) return ServiceResult.Fail("Last name is required.");

            if (!string.IsNullOrWhiteSpace(model.ST_EmployeeCode) &&
                await _repo.IsEmployeeCodeTakenAsync(tenantId, model.ST_EmployeeCode.Trim(), null))
                return ServiceResult.Fail("This employee code is already in use.");

            var entity = MapToEntity(model, tenantId, Guid.NewGuid());
            var id = await _repo.CreateAsync(entity);
            return ServiceResult.Ok("Staff member created successfully.", id);
        }

        public async Task<ServiceResult> UpdateStaffAsync(StaffFormViewModel model, Guid tenantId)
        {
            if (model == null) return ServiceResult.Fail("Form data is required.");
            if (!model.ST_Id.HasValue || model.ST_Id.Value == Guid.Empty)
                return ServiceResult.Fail("Staff Id is required for update.");
            if (model.ST_BranchId == Guid.Empty) return ServiceResult.Fail("Please select a Branch.");
            if (string.IsNullOrWhiteSpace(model.ST_EmployeeCode)) return ServiceResult.Fail("Employee code is required.");
            if (string.IsNullOrWhiteSpace(model.ST_FirstName)) return ServiceResult.Fail("First name is required.");
            if (string.IsNullOrWhiteSpace(model.ST_LastName)) return ServiceResult.Fail("Last name is required.");

            if (await _repo.IsEmployeeCodeTakenAsync(tenantId, model.ST_EmployeeCode.Trim(), model.ST_Id))
                return ServiceResult.Fail("This employee code is already in use.");

            var entity = MapToEntity(model, tenantId, model.ST_Id.Value);
            var success = await _repo.UpdateAsync(entity);
            return success
                ? ServiceResult.Ok("Staff member updated successfully.", model.ST_Id)
                : ServiceResult.Fail("Staff member not found.");
        }

        public async Task<ServiceResult> DeleteStaffAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(id, tenantId);
            return success
                ? ServiceResult.Ok("Staff member deleted successfully.")
                : ServiceResult.Fail("Unable to delete staff member.");
        }

        public void PopulateDropdowns(StaffFormViewModel vm)
        {
            vm.BranchOptions = GetDropdownSelectList("Branch", vm.ST_BranchId.ToString());
            vm.DepartmentOptions = GetDropdownSelectList("Department", vm.ST_DepartmentId?.ToString());
            vm.DesignationOptions = GetDropdownSelectList("Designation", vm.ST_DesignationId?.ToString());
            vm.GenderOptions = GetDropdownSelectList("Gender", vm.ST_Gender);
            vm.BloodGroupOptions = GetDropdownSelectList("BloodGroup", vm.ST_BloodGroup);
            vm.StatusOptions = GetStaffStatusSelectList(vm.ST_Status);
        }

        private List<SelectListItem> GetDropdownSelectList(string entityType, string selectedValue = null)
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
            catch { }

            return GetMasterSelectList(entityType, selectedValue);
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
                var nameEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Name"));
                var displayName = nameEntry.Value?.ToString() ?? id;
                list.Add(new SelectListItem { Value = id, Text = displayName, Selected = id == selectedValue });
            }
            return list;
        }

        private static Staff MapToEntity(StaffFormViewModel m, Guid tenantId, Guid id) => new()
        {
            ST_Id = id,
            ST_TenantId = tenantId,
            ST_BranchId = m.ST_BranchId,
            ST_DepartmentId = m.ST_DepartmentId,
            ST_DesignationId = m.ST_DesignationId,
            ST_EmployeeCode = m.ST_EmployeeCode?.Trim() ?? string.Empty,
            ST_FirstName = m.ST_FirstName?.Trim() ?? string.Empty,
            ST_LastName = m.ST_LastName?.Trim() ?? string.Empty,
            ST_Gender = m.ST_Gender?.Trim(),
            ST_DateOfBirth = m.ST_DateOfBirth,
            ST_BloodGroup = m.ST_BloodGroup?.Trim(),
            ST_Qualification = m.ST_Qualification?.Trim(),
            ST_ExperienceYears = m.ST_ExperienceYears,
            ST_Address = m.ST_Address?.Trim(),
            ST_EmergencyContact = m.ST_EmergencyContact?.Trim(),
            ST_BasicSalary = m.ST_BasicSalary,
            ST_Email = m.ST_Email?.Trim(),
            ST_Phone = m.ST_Phone?.Trim(),
            ST_JoiningDate = m.ST_JoiningDate,
            ST_Status = string.IsNullOrWhiteSpace(m.ST_Status) ? "Active" : m.ST_Status
        };

        private static List<SelectListItem> GetStaffStatusSelectList(string selected = null)
        {
            var statuses = new[] { "Active", "Inactive", "On Leave", "Terminated" };
            return new List<SelectListItem>(
                Array.ConvertAll(statuses, s => new SelectListItem { Value = s, Text = s, Selected = s == selected }));
        }
    }
}
