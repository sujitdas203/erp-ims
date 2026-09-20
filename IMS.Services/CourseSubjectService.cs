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
    public class CourseSubjectService : ICourseSubjectService
    {
        private readonly ICourseSubjectDAL _repo;
        private readonly IMasterService _masterService;
        private readonly IDropdownService _dropdownService;

        public CourseSubjectService(ICourseSubjectDAL repo, IMasterService masterService, IDropdownService dropdownService)
        {
            _repo = repo;
            _masterService = masterService;
            _dropdownService = dropdownService;
        }

        public async Task<CourseSubjectIndexViewModel> GetListAsync(Guid tenantId, Guid? courseId)
        {
            var items = courseId.HasValue
                ? await _repo.GetByCourseIdAsync(courseId.Value, tenantId)
                : await _repo.GetAllAsync(tenantId);

            return new CourseSubjectIndexViewModel
            {
                CourseFilter = courseId,
                CourseOptions = GetDropdownSelectList("Course", courseId?.ToString()),
                Items = items.ConvertAll(s => new CourseSubjectListItemViewModel
                {
                    CS_CourseId = s.CS_CourseId,
                    CS_SubjectId = s.CS_SubjectId,
                    CourseName = s.CourseName ?? "-",
                    SubjectName = s.SubjectName ?? "-",
                    CS_SequenceNo = s.CS_SequenceNo,
                    CS_IsMandatory = s.CS_IsMandatory,
                    CS_MaxMarks = s.CS_MaxMarks,
                    CS_PassMarks = s.CS_PassMarks
                })
            };
        }

        public async Task<CourseSubjectFormViewModel?> GetForEditAsync(Guid courseId, Guid subjectId, Guid tenantId)
        {
            var item = await _repo.GetByIdAsync(courseId, subjectId, tenantId);
            if (item == null) return null;

            var vm = new CourseSubjectFormViewModel
            {
                CS_CourseId = item.CS_CourseId,
                CS_SubjectId = item.CS_SubjectId,
                CS_SequenceNo = item.CS_SequenceNo,
                CS_IsMandatory = item.CS_IsMandatory,
                CS_MaxMarks = item.CS_MaxMarks,
                CS_PassMarks = item.CS_PassMarks
            };
            PopulateDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<int> GetNextSequenceNoAsync(Guid courseId, Guid tenantId)
        {
            if (courseId == Guid.Empty) return 1;
            return await _repo.GetNextSequenceNoAsync(courseId, tenantId);
        }

        public async Task<ServiceResult> CreateAsync(CourseSubjectFormViewModel model, Guid tenantId)
        {
            if (model == null)
                return ServiceResult.Fail("Invalid form data.");

            if (model.CS_CourseId == Guid.Empty)
                return ServiceResult.Fail("Please select a Course.");

            if (model.CS_SubjectId == Guid.Empty)
                return ServiceResult.Fail("Please select a Subject.");

            if (await _repo.ExistsAsync(model.CS_CourseId, model.CS_SubjectId, tenantId))
                return ServiceResult.Fail("This subject is already assigned to this course.");

            if (model.CS_SequenceNo <= 0)
            {
                model.CS_SequenceNo = await _repo.GetNextSequenceNoAsync(model.CS_CourseId, tenantId);
            }
            else
            {
                if (await _repo.SequenceExistsAsync(model.CS_CourseId, model.CS_SequenceNo, null, tenantId))
                {
                    var nextSeq = await _repo.GetNextSequenceNoAsync(model.CS_CourseId, tenantId);
                    return ServiceResult.Fail($"Sequence number {model.CS_SequenceNo} is already assigned in this course. Next available sequence is {nextSeq}.");
                }
            }

            if (model.CS_PassMarks.HasValue && model.CS_MaxMarks.HasValue && model.CS_PassMarks.Value > model.CS_MaxMarks.Value)
                return ServiceResult.Fail("Pass Marks cannot exceed Maximum Marks.");

            var entity = MapToEntity(model, tenantId);
            var success = await _repo.CreateAsync(entity);
            return success
                ? ServiceResult.Ok("Subject added to course successfully.")
                : ServiceResult.Fail("Failed to add subject to course.");
        }

        public async Task<ServiceResult> UpdateAsync(CourseSubjectFormViewModel model, Guid tenantId)
        {
            if (model == null || model.CS_CourseId == Guid.Empty || model.CS_SubjectId == Guid.Empty)
                return ServiceResult.Fail("Invalid Course or Subject.");

            if (model.CS_SequenceNo <= 0)
            {
                model.CS_SequenceNo = await _repo.GetNextSequenceNoAsync(model.CS_CourseId, tenantId);
            }
            else
            {
                if (await _repo.SequenceExistsAsync(model.CS_CourseId, model.CS_SequenceNo, model.CS_SubjectId, tenantId))
                {
                    return ServiceResult.Fail($"Sequence number {model.CS_SequenceNo} is already assigned to another subject in this course.");
                }
            }

            if (model.CS_PassMarks.HasValue && model.CS_MaxMarks.HasValue && model.CS_PassMarks.Value > model.CS_MaxMarks.Value)
                return ServiceResult.Fail("Pass Marks cannot exceed Maximum Marks.");

            var entity = MapToEntity(model, tenantId);
            var success = await _repo.UpdateAsync(entity, tenantId);
            return success
                ? ServiceResult.Ok("Course subject updated successfully.")
                : ServiceResult.Fail("Record not found or update failed.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid courseId, Guid subjectId, Guid tenantId)
        {
            var success = await _repo.DeleteAsync(courseId, subjectId, tenantId);
            return success
                ? ServiceResult.Ok("Subject removed from course.")
                : ServiceResult.Fail("Unable to remove subject from course.");
        }

        public void PopulateDropdowns(CourseSubjectFormViewModel vm, Guid tenantId)
        {
            vm.CourseOptions = GetDropdownSelectList("Course", vm.CS_CourseId != Guid.Empty ? vm.CS_CourseId.ToString() : null);
            vm.SubjectOptions = GetDropdownSelectList("Subject", vm.CS_SubjectId != Guid.Empty ? vm.CS_SubjectId.ToString() : null);
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
                            displayName = item.Values.ElementAtOrDefault(1)?.ToString() ?? id;

                        list.Add(new SelectListItem { Value = id, Text = displayName, Selected = id == selectedValue });
                    }
                    return list;
                }
            }
            catch { }

            return new List<SelectListItem>();
        }

        private static CourseSubject MapToEntity(CourseSubjectFormViewModel m, Guid tenantId) => new()
        {
            CS_CourseId = m.CS_CourseId,
            CS_SubjectId = m.CS_SubjectId,
            CS_SequenceNo = m.CS_SequenceNo,
            CS_IsMandatory = m.CS_IsMandatory,
            CS_MaxMarks = m.CS_MaxMarks,
            CS_PassMarks = m.CS_PassMarks
        };
    }
}
