using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Models.MockTest;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class MockTestService : IMockTestService
    {
        private readonly IMockTestDAL _repo;
        private readonly IMasterService _masterService;

        public MockTestService(IMockTestDAL repo, IMasterService masterService)
        {
            _repo = repo;
            _masterService = masterService;
        }

        public async Task<MockTestIndexViewModel> GetListAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, total) = await _repo.GetPagedAsync(tenantId, batchId, subjectId, status, search, page, pageSize);

            return new MockTestIndexViewModel
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
                    new() { Value = "Scheduled", Text = "Scheduled", Selected = status == "Scheduled" },
                    new() { Value = "Ongoing", Text = "Ongoing", Selected = status == "Ongoing" },
                    new() { Value = "Completed", Text = "Completed", Selected = status == "Completed" },
                    new() { Value = "Cancelled", Text = "Cancelled", Selected = status == "Cancelled" }
                },
                Tests = items.Select(t => new MockTestListItemViewModel
                {
                    MT_Id = t.MT_Id,
                    BatchName = t.BatchName ?? "-",
                    SubjectName = t.SubjectName ?? "-",
                    MT_Title = t.MT_Title,
                    MT_TestDate = t.MT_TestDate,
                    MT_DurationMinutes = t.MT_DurationMinutes,
                    MT_TotalMarks = t.MT_TotalMarks,
                    MT_PassMarks = t.MT_PassMarks,
                    MT_Status = t.MT_Status,
                    MT_IsActive = t.MT_IsActive,
                    ResultCount = t.ResultCount,
                    AvgScore = t.AvgScore
                }).ToList()
            };
        }

        public async Task<MockTestFormViewModel> GetEmptyFormAsync(Guid tenantId)
        {
            var vm = new MockTestFormViewModel();
            PopulateDropdowns(vm, tenantId);
            return await Task.FromResult(vm);
        }

        public async Task<MockTestFormViewModel?> GetForEditAsync(Guid id, Guid tenantId)
        {
            var test = await _repo.GetByIdAsync(id, tenantId);
            if (test == null) return null;

            var vm = new MockTestFormViewModel
            {
                MT_Id = test.MT_Id,
                MT_BatchId = test.MT_BatchId,
                MT_SubjectId = test.MT_SubjectId,
                MT_Title = test.MT_Title,
                MT_Description = test.MT_Description,
                MT_TestDate = test.MT_TestDate,
                MT_DurationMinutes = test.MT_DurationMinutes,
                MT_TotalMarks = test.MT_TotalMarks,
                MT_PassMarks = test.MT_PassMarks,
                MT_Status = test.MT_Status
            };

            PopulateDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<MockTestDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId)
        {
            return await _repo.GetDetailsByIdAsync(id, tenantId);
        }

        public async Task<ServiceResult> CreateAsync(MockTestFormViewModel model, Guid tenantId)
        {
            if (model.MT_BatchId == Guid.Empty) return ServiceResult.Fail("Batch is required.");
            if (model.MT_SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (string.IsNullOrWhiteSpace(model.MT_Title)) return ServiceResult.Fail("Test title is required.");
            if (model.MT_DurationMinutes <= 0) return ServiceResult.Fail("Duration must be greater than zero minutes.");
            if (model.MT_TotalMarks <= 0) return ServiceResult.Fail("Total marks must be greater than zero.");
            if (model.MT_PassMarks < 0 || model.MT_PassMarks > model.MT_TotalMarks)
                return ServiceResult.Fail("Pass marks must be between 0 and total marks.");

            var entity = new MockTest
            {
                MT_Id = Guid.NewGuid(),
                MT_TenantId = tenantId,
                MT_BatchId = model.MT_BatchId,
                MT_SubjectId = model.MT_SubjectId,
                MT_Title = model.MT_Title.Trim(),
                MT_Description = model.MT_Description?.Trim(),
                MT_TestDate = model.MT_TestDate,
                MT_DurationMinutes = model.MT_DurationMinutes,
                MT_TotalMarks = model.MT_TotalMarks,
                MT_PassMarks = model.MT_PassMarks,
                MT_Status = string.IsNullOrWhiteSpace(model.MT_Status) ? "Scheduled" : model.MT_Status
            };

            var id = await _repo.CreateAsync(entity);
            return ServiceResult.Ok("Mock test created successfully.", id);
        }

        public async Task<ServiceResult> UpdateAsync(MockTestFormViewModel model, Guid tenantId)
        {
            if (!model.MT_Id.HasValue || model.MT_Id == Guid.Empty)
                return ServiceResult.Fail("Test Id is required.");
            if (model.MT_BatchId == Guid.Empty) return ServiceResult.Fail("Batch is required.");
            if (model.MT_SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (string.IsNullOrWhiteSpace(model.MT_Title)) return ServiceResult.Fail("Test title is required.");
            if (model.MT_DurationMinutes <= 0) return ServiceResult.Fail("Duration must be greater than zero minutes.");
            if (model.MT_TotalMarks <= 0) return ServiceResult.Fail("Total marks must be greater than zero.");
            if (model.MT_PassMarks < 0 || model.MT_PassMarks > model.MT_TotalMarks)
                return ServiceResult.Fail("Pass marks must be between 0 and total marks.");

            var entity = new MockTest
            {
                MT_Id = model.MT_Id.Value,
                MT_TenantId = tenantId,
                MT_BatchId = model.MT_BatchId,
                MT_SubjectId = model.MT_SubjectId,
                MT_Title = model.MT_Title.Trim(),
                MT_Description = model.MT_Description?.Trim(),
                MT_TestDate = model.MT_TestDate,
                MT_DurationMinutes = model.MT_DurationMinutes,
                MT_TotalMarks = model.MT_TotalMarks,
                MT_PassMarks = model.MT_PassMarks,
                MT_Status = string.IsNullOrWhiteSpace(model.MT_Status) ? "Scheduled" : model.MT_Status
            };

            var success = await _repo.UpdateAsync(entity);
            return success ? ServiceResult.Ok("Mock test updated successfully.", model.MT_Id) : ServiceResult.Fail("Failed to update mock test.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(id, tenantId);
            return success ? ServiceResult.Ok("Mock test deleted successfully.") : ServiceResult.Fail("Unable to delete mock test.");
        }

        public async Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            var success = await _repo.ToggleActiveAsync(id, tenantId, isActive);
            return success ? ServiceResult.Ok(isActive ? "Test activated." : "Test deactivated.") : ServiceResult.Fail("Unable to update status.");
        }

        public void PopulateDropdowns(MockTestFormViewModel vm, Guid tenantId)
        {
            vm.BatchOptions = GetMasterSelectList("Batch", vm.MT_BatchId.ToString());
            vm.SubjectOptions = GetMasterSelectList("Subject", vm.MT_SubjectId.ToString());
            vm.StatusOptions = new()
            {
                new() { Value = "Scheduled", Text = "Scheduled", Selected = vm.MT_Status == "Scheduled" },
                new() { Value = "Ongoing", Text = "Ongoing", Selected = vm.MT_Status == "Ongoing" },
                new() { Value = "Completed", Text = "Completed", Selected = vm.MT_Status == "Completed" },
                new() { Value = "Cancelled", Text = "Cancelled", Selected = vm.MT_Status == "Cancelled" }
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
