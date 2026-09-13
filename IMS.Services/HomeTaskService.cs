using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Models.HomeTask;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class HomeTaskService : IHomeTaskService
    {
        private readonly IHomeTaskDAL _repo;
        private readonly IMasterService _masterService;

        public HomeTaskService(IHomeTaskDAL repo, IMasterService masterService)
        {
            _repo = repo;
            _masterService = masterService;
        }

        public async Task<HomeTaskIndexViewModel> GetListAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, total) = await _repo.GetPagedAsync(tenantId, batchId, subjectId, status, search, page, pageSize);

            return new HomeTaskIndexViewModel
            {
                BatchFilter = batchId,
                SubjectFilter = subjectId,
                StatusFilter = status,
                SearchTerm = search,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total,
                BatchOptions = GetMasterSelectList("Batch", batchId?.ToString()),
                SubjectOptions = GetMasterSelectList("Subject", subjectId?.ToString()),
                StatusOptions = new()
                {
                    new() { Value = "Active", Text = "Active", Selected = status == "Active" },
                    new() { Value = "Closed", Text = "Closed", Selected = status == "Closed" }
                },
                Tasks = items.Select(t => new HomeTaskListItemViewModel
                {
                    HT_Id = t.HT_Id,
                    BatchName = t.BatchName ?? "-",
                    SubjectName = t.SubjectName ?? "-",
                    TeacherName = t.TeacherName,
                    HT_Title = t.HT_Title,
                    HT_AssignedDate = t.HT_AssignedDate,
                    HT_DueDate = t.HT_DueDate,
                    HT_MaxMarks = t.HT_MaxMarks,
                    HT_Status = t.HT_Status,
                    HT_IsActive = t.HT_IsActive,
                    SubmissionCount = t.SubmissionCount
                }).ToList()
            };
        }

        public async Task<HomeTaskFormViewModel> GetEmptyFormAsync(Guid tenantId)
        {
            var vm = new HomeTaskFormViewModel();
            PopulateDropdowns(vm, tenantId);
            return await Task.FromResult(vm);
        }

        public async Task<HomeTaskFormViewModel?> GetForEditAsync(Guid id, Guid tenantId)
        {
            var task = await _repo.GetByIdAsync(id, tenantId);
            if (task == null) return null;

            var vm = new HomeTaskFormViewModel
            {
                HT_Id = task.HT_Id,
                HT_BatchId = task.HT_BatchId,
                HT_SubjectId = task.HT_SubjectId,
                HT_TeacherId = task.HT_TeacherId,
                HT_Title = task.HT_Title,
                HT_Description = task.HT_Description,
                HT_AssignedDate = task.HT_AssignedDate,
                HT_DueDate = task.HT_DueDate,
                HT_AttachmentUrl = task.HT_AttachmentUrl,
                HT_MaxMarks = task.HT_MaxMarks,
                HT_Status = task.HT_Status
            };

            PopulateDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<HomeTaskDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId)
        {
            return await _repo.GetDetailsByIdAsync(id, tenantId);
        }

        public async Task<ServiceResult> CreateAsync(HomeTaskFormViewModel model, Guid tenantId)
        {
            if (model.HT_BatchId == Guid.Empty) return ServiceResult.Fail("Batch is required.");
            if (model.HT_SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (string.IsNullOrWhiteSpace(model.HT_Title)) return ServiceResult.Fail("Task title is required.");
            if (string.IsNullOrWhiteSpace(model.HT_Description)) return ServiceResult.Fail("Task description is required.");
            if (model.HT_DueDate < model.HT_AssignedDate) return ServiceResult.Fail("Due date cannot be earlier than assigned date.");

            var entity = new HomeTask
            {
                HT_Id = Guid.NewGuid(),
                HT_TenantId = tenantId,
                HT_BatchId = model.HT_BatchId,
                HT_SubjectId = model.HT_SubjectId,
                HT_TeacherId = model.HT_TeacherId,
                HT_Title = model.HT_Title.Trim(),
                HT_Description = model.HT_Description.Trim(),
                HT_AssignedDate = model.HT_AssignedDate,
                HT_DueDate = model.HT_DueDate,
                HT_AttachmentUrl = model.HT_AttachmentUrl?.Trim(),
                HT_MaxMarks = model.HT_MaxMarks,
                HT_Status = string.IsNullOrWhiteSpace(model.HT_Status) ? "Active" : model.HT_Status
            };

            var id = await _repo.CreateAsync(entity);
            return ServiceResult.Ok("Home task created successfully.", id);
        }

        public async Task<ServiceResult> UpdateAsync(HomeTaskFormViewModel model, Guid tenantId)
        {
            if (!model.HT_Id.HasValue || model.HT_Id == Guid.Empty)
                return ServiceResult.Fail("Task Id is required.");
            if (model.HT_BatchId == Guid.Empty) return ServiceResult.Fail("Batch is required.");
            if (model.HT_SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (string.IsNullOrWhiteSpace(model.HT_Title)) return ServiceResult.Fail("Task title is required.");
            if (string.IsNullOrWhiteSpace(model.HT_Description)) return ServiceResult.Fail("Task description is required.");
            if (model.HT_DueDate < model.HT_AssignedDate) return ServiceResult.Fail("Due date cannot be earlier than assigned date.");

            var entity = new HomeTask
            {
                HT_Id = model.HT_Id.Value,
                HT_TenantId = tenantId,
                HT_BatchId = model.HT_BatchId,
                HT_SubjectId = model.HT_SubjectId,
                HT_TeacherId = model.HT_TeacherId,
                HT_Title = model.HT_Title.Trim(),
                HT_Description = model.HT_Description.Trim(),
                HT_AssignedDate = model.HT_AssignedDate,
                HT_DueDate = model.HT_DueDate,
                HT_AttachmentUrl = model.HT_AttachmentUrl?.Trim(),
                HT_MaxMarks = model.HT_MaxMarks,
                HT_Status = string.IsNullOrWhiteSpace(model.HT_Status) ? "Active" : model.HT_Status
            };

            var success = await _repo.UpdateAsync(entity);
            return success ? ServiceResult.Ok("Home task updated successfully.", model.HT_Id) : ServiceResult.Fail("Failed to update home task.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(id, tenantId);
            return success ? ServiceResult.Ok("Home task deleted successfully.") : ServiceResult.Fail("Unable to delete home task.");
        }

        public async Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            var success = await _repo.ToggleActiveAsync(id, tenantId, isActive);
            return success ? ServiceResult.Ok(isActive ? "Task activated." : "Task deactivated.") : ServiceResult.Fail("Unable to update status.");
        }

        public void PopulateDropdowns(HomeTaskFormViewModel vm, Guid tenantId)
        {
            vm.BatchOptions = GetMasterSelectList("Batch", vm.HT_BatchId.ToString());
            vm.SubjectOptions = GetMasterSelectList("Subject", vm.HT_SubjectId.ToString());
            //vm.TeacherOptions = GetMasterSelectList("Teacher", vm.HT_TeacherId?.ToString());
            vm.StatusOptions = new()
            {
                new() { Value = "Active", Text = "Active", Selected = vm.HT_Status == "Active" },
                new() { Value = "Closed", Text = "Closed", Selected = vm.HT_Status == "Closed" }
            };
        }

        private List<SelectListItem> GetMasterSelectList(string entityType, string? selectedValue = null)
        {
            var items = _masterService?.GetAll(entityType);
            var list = new List<SelectListItem>();
            if (items == null) return list;
            foreach (var item in items)
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
}
