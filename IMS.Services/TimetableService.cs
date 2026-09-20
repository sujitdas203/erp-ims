using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IDropdownService _dropdownService;

        public TimetableService(ITimetableDAL repo, IMasterService masterService, IDropdownService dropdownService)
        {
            _repo = repo;
            _masterService = masterService;
            _dropdownService = dropdownService;
        }

        public async Task<TimetableIndexViewModel> GetListAsync(Guid tenantId, Guid? batchId, Guid? branchId)
        {
            var items = await _repo.GetAllAsync(tenantId, batchId, branchId);
            var distinctItems = items?.GroupBy(t => t.TT_Id).Select(g => g.First()).ToList() ?? new List<Timetable>();
            return new TimetableIndexViewModel
            {
                BatchFilter = batchId,
                BranchFilter = branchId,
                Entries = distinctItems.ConvertAll(t => new TimetableListItemViewModel
                {
                    TT_Id = t.TT_Id,
                    TT_DayOfWeek = t.TT_DayOfWeek,
                    TT_StartTime = t.TT_StartTime,
                    TT_EndTime = t.TT_EndTime,
                    SubjectName = t.SubjectName ?? "-",
                    StaffName = t.StaffName ?? "-",
                    ClassroomName = t.ClassroomName ?? "-"
                })
            };
        }

        public async Task<TimetableDetailsViewModel> GetDetailsAsync(Guid id, Guid tenantId)
        {
            var t = await _repo.GetByIdAsync(id, tenantId);
            if (t == null) return null;
            return new TimetableDetailsViewModel
            {
                TT_Id = t.TT_Id,
                DayName = GetDayName(t.TT_DayOfWeek),
                TT_StartTime = t.TT_StartTime,
                TT_EndTime = t.TT_EndTime,
                SubjectName = t.SubjectName ?? "-",
                StaffName = t.StaffName ?? "-",
                ClassroomName = t.ClassroomName ?? "-",
                BatchName = t.BatchName ?? "-",
                TT_EffectiveFrom = t.TT_EffectiveFrom,
                TT_EffectiveTo = t.TT_EffectiveTo,
                TT_CreatedAt = t.TT_CreatedAt
            };
        }

        public async Task<TimetableFormViewModel> GetForEditAsync(Guid id, Guid tenantId)
        {
            var t = await _repo.GetByIdAsync(id, tenantId);
            if (t == null) return null;
            return new TimetableFormViewModel
            {
                TT_Id = t.TT_Id,
                TT_BranchId = t.TT_BranchId,
                TT_BatchId = t.TT_BatchId,
                TT_SubjectId = t.TT_SubjectId,
                TT_StaffId = t.TT_StaffId,
                TT_ClassroomId = t.TT_ClassroomId,
                TT_DayOfWeek = t.TT_DayOfWeek,
                TT_StartTime = t.TT_StartTime.ToString(@"hh\:mm"),
                TT_EndTime = t.TT_EndTime.ToString(@"hh\:mm"),
                TT_EffectiveFrom = t.TT_EffectiveFrom,
                TT_EffectiveTo = t.TT_EffectiveTo
            };
        }

        public async Task<ServiceResult> CreateAsync(TimetableFormViewModel model, Guid tenantId)
        {
            if (model == null) return ServiceResult.Fail("Form data is required.");
            if (model.TT_BranchId == Guid.Empty) return ServiceResult.Fail("Please select a Branch.");
            if (model.TT_BatchId == Guid.Empty) return ServiceResult.Fail("Please select a Batch.");
            if (model.TT_SubjectId == Guid.Empty) return ServiceResult.Fail("Please select a Subject.");
            if (model.TT_StaffId == Guid.Empty) return ServiceResult.Fail("Please select a Staff member.");
            if (model.TT_DayOfWeek < 1 || model.TT_DayOfWeek > 7) return ServiceResult.Fail("Please select a valid Day of Week (Monday to Sunday).");

            if (!TimeSpan.TryParse(model.TT_StartTime, out var start) || !TimeSpan.TryParse(model.TT_EndTime, out var end))
                return ServiceResult.Fail("Invalid time format.");

            if (end <= start)
                return ServiceResult.Fail("End time must be after start time.");

            if (model.TT_EffectiveFrom.HasValue && model.TT_EffectiveTo.HasValue && model.TT_EffectiveTo.Value < model.TT_EffectiveFrom.Value)
                return ServiceResult.Fail("Effective To date must be on or after Effective From date.");

            if (await _repo.HasConflictAsync(tenantId, model.TT_BatchId, model.TT_DayOfWeek, start, end, null))
                return ServiceResult.Fail("Time slot conflicts with an existing timetable entry.");

            try
            {
                var entity = MapToEntity(model, tenantId, Guid.NewGuid(), start, end);
                var id = await _repo.CreateAsync(entity);
                return ServiceResult.Ok("Timetable entry created successfully.", id);
            }
            catch (SqlException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while saving. Please try again.");
            }
        }

        public async Task<ServiceResult> UpdateAsync(TimetableFormViewModel model, Guid tenantId)
        {
            if (model == null || !model.TT_Id.HasValue) return ServiceResult.Fail("Timetable Id is required.");
            if (model.TT_BranchId == Guid.Empty) return ServiceResult.Fail("Please select a Branch.");
            if (model.TT_BatchId == Guid.Empty) return ServiceResult.Fail("Please select a Batch.");
            if (model.TT_SubjectId == Guid.Empty) return ServiceResult.Fail("Please select a Subject.");
            if (model.TT_StaffId == Guid.Empty) return ServiceResult.Fail("Please select a Staff member.");
            if (model.TT_DayOfWeek < 1 || model.TT_DayOfWeek > 7) return ServiceResult.Fail("Please select a valid Day of Week (Monday to Sunday).");

            if (!TimeSpan.TryParse(model.TT_StartTime, out var start) || !TimeSpan.TryParse(model.TT_EndTime, out var end))
                return ServiceResult.Fail("Invalid time format.");

            if (end <= start) return ServiceResult.Fail("End time must be after start time.");

            if (model.TT_EffectiveFrom.HasValue && model.TT_EffectiveTo.HasValue && model.TT_EffectiveTo.Value < model.TT_EffectiveFrom.Value)
                return ServiceResult.Fail("Effective To date must be on or after Effective From date.");

            if (await _repo.HasConflictAsync(tenantId, model.TT_BatchId, model.TT_DayOfWeek, start, end, model.TT_Id))
                return ServiceResult.Fail("Time slot conflicts with an existing timetable entry.");

            try
            {
                var entity = MapToEntity(model, tenantId, model.TT_Id.Value, start, end);
                var success = await _repo.UpdateAsync(entity);
                return success ? ServiceResult.Ok("Timetable entry updated successfully.", model.TT_Id) : ServiceResult.Fail("Timetable record not found.");
            }
            catch (SqlException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while saving. Please try again.");
            }
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            try
            {
                var success = await _repo.DeleteAsync(id, tenantId);
                return success ? ServiceResult.Ok("Timetable entry deleted.") : ServiceResult.Fail("Unable to delete timetable entry.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail(ex.Message ?? "Failed to delete.");
            }
        }

        public async Task<bool> CheckConflictAsync(Guid tenantId, Guid batchId, int dayOfWeek, string startTime, string endTime, Guid? excludeId)
        {
            if (!TimeSpan.TryParse(startTime, out var start) || !TimeSpan.TryParse(endTime, out var end)) return false;
            return await _repo.HasConflictAsync(tenantId, batchId, dayOfWeek, start, end, excludeId);
        }

        private static Timetable MapToEntity(TimetableFormViewModel m, Guid tenantId, Guid id, TimeSpan start, TimeSpan end) => new()
        {
            TT_Id = id,
            TT_TenantId = tenantId,
            TT_BranchId = m.TT_BranchId,
            TT_BatchId = m.TT_BatchId,
            TT_SubjectId = m.TT_SubjectId,
            TT_StaffId = m.TT_StaffId,
            TT_ClassroomId = m.TT_ClassroomId,
            TT_DayOfWeek = m.TT_DayOfWeek,
            TT_StartTime = start,
            TT_EndTime = end,
            TT_EffectiveFrom = m.TT_EffectiveFrom,
            TT_EffectiveTo = m.TT_EffectiveTo
        };

        public void PopulateDropdowns(TimetableFormViewModel vm)
        {
            vm.BranchOptions = GetDropdownSelectList("Branch", vm.TT_BranchId != Guid.Empty ? vm.TT_BranchId.ToString() : null);
            if (vm.BranchOptions.Count == 0)
                vm.BranchOptions = HardcodedMasterData.GetBranchSelectList(vm.TT_BranchId);

            vm.BatchOptions = GetDropdownSelectList("Batch", vm.TT_BatchId != Guid.Empty ? vm.TT_BatchId.ToString() : null);
            vm.SubjectOptions = GetDropdownSelectList("Subject", vm.TT_SubjectId != Guid.Empty ? vm.TT_SubjectId.ToString() : null);
            vm.StaffOptions = GetDropdownSelectList("Staff", vm.TT_StaffId != Guid.Empty ? vm.TT_StaffId.ToString() : null);
            vm.ClassroomOptions = GetDropdownSelectList("Classroom", vm.TT_ClassroomId.HasValue && vm.TT_ClassroomId != Guid.Empty ? vm.TT_ClassroomId.ToString() : null);
            vm.DayOfWeekOptions = GetDayOfWeekSelectList(vm.TT_DayOfWeek);
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
            catch { }

            try
            {
                var masterItems = _masterService?.GetAll(entityType);
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

                        list.Add(new SelectListItem { Value = id, Text = displayName, Selected = string.Equals(id, selectedValue, StringComparison.OrdinalIgnoreCase) });
                    }
                    return list;
                }
            }
            catch { }

            return new List<SelectListItem>();
        }

        public static List<SelectListItem> GetDayOfWeekSelectList(int selected = 0)
        {
            var days = new[]
            {
                new { Id = 1, Name = "Monday" },
                new { Id = 2, Name = "Tuesday" },
                new { Id = 3, Name = "Wednesday" },
                new { Id = 4, Name = "Thursday" },
                new { Id = 5, Name = "Friday" },
                new { Id = 6, Name = "Saturday" },
                new { Id = 7, Name = "Sunday" }
            };
            return days.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = (selected == 0 && d.Id == 1) || d.Id == selected
            }).ToList();
        }

        public static string GetDayName(int dayOfWeek) => dayOfWeek switch
        {
            1 => "Monday",
            2 => "Tuesday",
            3 => "Wednesday",
            4 => "Thursday",
            5 => "Friday",
            6 => "Saturday",
            7 => "Sunday",
            _ => "-"
        };
    }
}
