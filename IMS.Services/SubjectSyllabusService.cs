using IMS.DAL.Interfaces;
using IMS.Models.Common.Dropdown;
using IMS.Models.SubjectSyllabus;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services
{
    public class SubjectSyllabusService : ISubjectSyllabusService
    {
        private readonly ISubjectSyllabusDAL _repo;
        private readonly IDropdownService _dropdownService;
        private readonly IMasterService _masterService;
        private readonly ICourseSubjectDAL _courseSubjectDAL;

        public SubjectSyllabusService(
            ISubjectSyllabusDAL repo,
            IDropdownService dropdownService,
            IMasterService masterService,
            ICourseSubjectDAL courseSubjectDAL)
        {
            _repo = repo;
            _dropdownService = dropdownService;
            _masterService = masterService;
            _courseSubjectDAL = courseSubjectDAL;
        }

        public async Task<SyllabusIndexViewModel> GetListAsync(
            Guid tenantId, Guid? courseId, Guid? subjectId, string? status, string? search, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, total) = await _repo.GetPagedAsync(tenantId, courseId, subjectId, status, search, page, pageSize);
            var courses = await GetCoursesAsync(tenantId);
            var subjects = await GetSubjectsByCourseAsync(courseId, tenantId);

            return new SyllabusIndexViewModel
            {
                CourseFilter = courseId,
                SubjectFilter = subjectId,
                StatusFilter = status,
                Search = search,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total,
                CourseOptions = courses.Select(c => new SelectListItem
                {
                    Value = c.Value,
                    Text = c.Text,
                    Selected = courseId.HasValue && string.Equals(c.Value, courseId.Value.ToString(), StringComparison.OrdinalIgnoreCase)
                }).ToList(),
                SubjectOptions = subjects.Select(s => new SelectListItem
                {
                    Value = s.Value,
                    Text = s.Text,
                    Selected = subjectId.HasValue && string.Equals(s.Value, subjectId.Value.ToString(), StringComparison.OrdinalIgnoreCase)
                }).ToList(),
                StatusOptions = new()
                {
                    new() { Value = "Active", Text = "Active", Selected = status == "Active" },
                    new() { Value = "Inactive", Text = "Inactive", Selected = status == "Inactive" },
                    new() { Value = "Completed", Text = "Completed", Selected = status == "Completed" },
                    new() { Value = "Pending", Text = "Pending", Selected = status == "Pending" }
                },
                Units = items.Select(s => new SyllabusListItemViewModel
                {
                    SS_Id = s.SS_Id,
                    CourseName = !string.IsNullOrWhiteSpace(s.CourseName) ? s.CourseName : "Course",
                    SubjectName = !string.IsNullOrWhiteSpace(s.SubjectName) ? s.SubjectName : "Subject",
                    UnitNumber = s.SS_UnitNumber,
                    UnitTitle = s.SS_UnitTitle,
                    TotalHours = s.SS_TotalHours,
                    IsCompleted = s.SS_IsCompleted,
                    IsActive = s.SS_IsActive,
                    CreatedAt = s.SS_CreatedAt
                }).ToList()
            };
        }

        public async Task<SyllabusFormViewModel> GetEmptyFormAsync(Guid tenantId)
        {
            var vm = new SyllabusFormViewModel();
            await PopulateDropdownsAsync(vm, tenantId);
            return vm;
        }

        public async Task<SyllabusFormViewModel?> GetForEditAsync(Guid id, Guid tenantId)
        {
            var s = await _repo.GetByIdAsync(id, tenantId);
            if (s == null) return null;

            var vm = new SyllabusFormViewModel
            {
                SS_Id = s.SS_Id,
                CourseId = s.SS_CourseId,
                SubjectId = s.SS_SubjectId,
                UnitNumber = s.SS_UnitNumber,
                UnitTitle = s.SS_UnitTitle,
                Description = s.SS_Description,
                TotalHours = s.SS_TotalHours,
                FileUrl = s.SS_FileUrl
            };
            await PopulateDropdownsAsync(vm, tenantId);
            return vm;
        }

        public async Task<ServiceResult> CreateAsync(SyllabusFormViewModel model, Guid tenantId)
        {
            if (model == null) return ServiceResult.Fail("Form data is missing.");
            if (model.CourseId == Guid.Empty) return ServiceResult.Fail("Please select a valid Course.");
            if (model.SubjectId == Guid.Empty) return ServiceResult.Fail("Please select a valid Subject.");
            if (model.UnitNumber <= 0) return ServiceResult.Fail("Unit number must be greater than zero.");
            if (string.IsNullOrWhiteSpace(model.UnitTitle)) return ServiceResult.Fail("Unit title is required.");

            try
            {
                var entity = new SubjectSyllabus
                {
                    SS_Id = Guid.NewGuid(),
                    SS_TenantId = tenantId,
                    SS_CourseId = model.CourseId,
                    SS_SubjectId = model.SubjectId,
                    SS_UnitNumber = model.UnitNumber,
                    SS_UnitTitle = model.UnitTitle.Trim(),
                    SS_Description = model.Description?.Trim(),
                    SS_TotalHours = model.TotalHours,
                    SS_FileUrl = model.FileUrl?.Trim()
                };

                var id = await _repo.CreateAsync(entity);
                return ServiceResult.Ok("Syllabus unit added successfully.", id);
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

        public async Task<ServiceResult> UpdateAsync(SyllabusFormViewModel model, Guid tenantId)
        {
            if (model == null || !model.SS_Id.HasValue || model.SS_Id.Value == Guid.Empty)
                return ServiceResult.Fail("Syllabus Id is required.");
            if (model.CourseId == Guid.Empty) return ServiceResult.Fail("Please select a valid Course.");
            if (model.SubjectId == Guid.Empty) return ServiceResult.Fail("Please select a valid Subject.");
            if (model.UnitNumber <= 0) return ServiceResult.Fail("Unit number must be greater than zero.");
            if (string.IsNullOrWhiteSpace(model.UnitTitle)) return ServiceResult.Fail("Unit title is required.");

            try
            {
                var entity = new SubjectSyllabus
                {
                    SS_Id = model.SS_Id.Value,
                    SS_TenantId = tenantId,
                    SS_CourseId = model.CourseId,
                    SS_SubjectId = model.SubjectId,
                    SS_UnitNumber = model.UnitNumber,
                    SS_UnitTitle = model.UnitTitle.Trim(),
                    SS_Description = model.Description?.Trim(),
                    SS_TotalHours = model.TotalHours,
                    SS_FileUrl = model.FileUrl?.Trim()
                };

                var success = await _repo.UpdateAsync(entity);
                return success ? ServiceResult.Ok("Syllabus unit updated successfully.") : ServiceResult.Fail("Unit not found.");
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

        public async Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            try
            {
                var success = await _repo.ToggleActiveAsync(id, tenantId, isActive);
                return success ? ServiceResult.Ok(isActive ? "Unit activated." : "Unit deactivated.") : ServiceResult.Fail("Unit not found.");
            }
            catch (Exception) { return ServiceResult.Fail("Something went wrong. Please try again."); }
        }

        public async Task<ServiceResult> ToggleCompletedAsync(Guid id, Guid tenantId, bool isCompleted)
        {
            try
            {
                var success = await _repo.ToggleCompletedAsync(id, tenantId, isCompleted);
                return success ? ServiceResult.Ok(isCompleted ? "Marked as completed." : "Marked as pending.") : ServiceResult.Fail("Unit not found.");
            }
            catch (Exception) { return ServiceResult.Fail("Something went wrong. Please try again."); }
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            try
            {
                var success = await _repo.DeleteAsync(id, tenantId);
                return success ? ServiceResult.Ok("Syllabus unit deleted.") : ServiceResult.Fail("Unit not found.");
            }
            catch (Exception) { return ServiceResult.Fail("Something went wrong while deleting. Please try again."); }
        }

        public async Task<List<SelectListItem>> GetCoursesAsync(Guid tenantId)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 1. Try DropdownService
            try
            {
                var req = new DropdownRequestModel { EntityType = "Course", ActiveOnly = false };
                var items = _dropdownService.GetDropdown(req);
                if (items != null)
                {
                    foreach (var itm in items)
                    {
                        if (!string.IsNullOrWhiteSpace(itm.Value) && !result.ContainsKey(itm.Value))
                        {
                            var text = !string.IsNullOrWhiteSpace(itm.Code) && !itm.Text.Contains(itm.Code) ? $"{itm.Text} ({itm.Code})" : itm.Text;
                            result[itm.Value] = text;
                        }
                    }
                }
            }
            catch { }

            // 2. Try MasterService
            try
            {
                var masterItems = _masterService.GetAll("Course");
                if (masterItems != null)
                {
                    foreach (var item in masterItems)
                    {
                        var keyEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Id"));
                        var id = keyEntry.Value?.ToString() ?? "";
                        if (!string.IsNullOrWhiteSpace(id) && !result.ContainsKey(id))
                        {
                            string? displayName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Name")).Value?.ToString();
                            result[id] = displayName ?? id;
                        }
                    }
                }
            }
            catch { }

            // 3. Try Repo direct query
            try
            {
                var repoCourses = await _repo.GetCourseOptionsAsync(tenantId);
                if (repoCourses != null)
                {
                    foreach (var c in repoCourses)
                    {
                        var id = c.Id.ToString();
                        if (!result.ContainsKey(id))
                        {
                            result[id] = c.Name;
                        }
                    }
                }
            }
            catch { }

            return result.Select(r => new SelectListItem { Value = r.Key, Text = r.Value }).OrderBy(r => r.Text).ToList();
        }

        public async Task<List<SelectListItem>> GetSubjectsByCourseAsync(Guid? courseId, Guid tenantId)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 1. If courseId provided, first get subjects mapped to that course from CourseSubjects_CS
            if (courseId.HasValue && courseId.Value != Guid.Empty)
            {
                try
                {
                    var courseSubjects = await _courseSubjectDAL.GetByCourseIdAsync(courseId.Value, tenantId);
                    if (courseSubjects != null && courseSubjects.Count > 0)
                    {
                        foreach (var cs in courseSubjects)
                        {
                            var id = cs.CS_SubjectId.ToString();
                            if (!result.ContainsKey(id))
                            {
                                result[id] = !string.IsNullOrWhiteSpace(cs.SubjectName) ? cs.SubjectName : "Subject";
                            }
                        }
                    }
                }
                catch { }
            }

            // 2. Populate / merge with all system subjects
            try
            {
                var req = new DropdownRequestModel { EntityType = "Subject", ActiveOnly = false };
                var items = _dropdownService.GetDropdown(req);
                if (items != null)
                {
                    foreach (var itm in items)
                    {
                        if (!string.IsNullOrWhiteSpace(itm.Value) && !result.ContainsKey(itm.Value))
                        {
                            var text = !string.IsNullOrWhiteSpace(itm.Code) && !itm.Text.Contains(itm.Code) ? $"{itm.Text} ({itm.Code})" : itm.Text;
                            result[itm.Value] = text;
                        }
                    }
                }
            }
            catch { }

            try
            {
                var masterItems = _masterService.GetAll("Subject");
                if (masterItems != null)
                {
                    foreach (var item in masterItems)
                    {
                        var keyEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Id"));
                        var id = keyEntry.Value?.ToString() ?? "";
                        if (!string.IsNullOrWhiteSpace(id) && !result.ContainsKey(id))
                        {
                            string? displayName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Name")).Value?.ToString();
                            result[id] = displayName ?? id;
                        }
                    }
                }
            }
            catch { }

            try
            {
                var repoSubjects = await _repo.GetSubjectOptionsAsync(tenantId);
                if (repoSubjects != null)
                {
                    foreach (var s in repoSubjects)
                    {
                        var id = s.Id.ToString();
                        if (!result.ContainsKey(id))
                        {
                            var text = !string.IsNullOrWhiteSpace(s.Code) ? $"{s.Name} ({s.Code})" : s.Name;
                            result[id] = text;
                        }
                    }
                }
            }
            catch { }

            return result.Select(r => new SelectListItem { Value = r.Key, Text = r.Value }).OrderBy(r => r.Text).ToList();
        }

        private async Task PopulateDropdownsAsync(SyllabusFormViewModel vm, Guid tenantId)
        {
            var courses = await GetCoursesAsync(tenantId);
            var subjects = await GetSubjectsByCourseAsync(vm.CourseId != Guid.Empty ? vm.CourseId : null, tenantId);

            vm.CourseOptions = courses.Select(c => new SelectListItem
            {
                Value = c.Value,
                Text = c.Text,
                Selected = string.Equals(c.Value, vm.CourseId.ToString(), StringComparison.OrdinalIgnoreCase)
            }).ToList();

            vm.SubjectOptions = subjects.Select(s => new SelectListItem
            {
                Value = s.Value,
                Text = s.Text,
                Selected = string.Equals(s.Value, vm.SubjectId.ToString(), StringComparison.OrdinalIgnoreCase)
            }).ToList();
        }
    }
}
