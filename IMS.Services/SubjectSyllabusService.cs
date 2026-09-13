using IMS.DAL.Interfaces;
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

        public SubjectSyllabusService(ISubjectSyllabusDAL repo)
        {
            _repo = repo;
        }

        public async Task<SyllabusIndexViewModel> GetListAsync(
            Guid tenantId, Guid? courseId, Guid? subjectId, string? status, string? search, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, total) = await _repo.GetPagedAsync(tenantId, courseId, subjectId, status, search, page, pageSize);
            var courses = await _repo.GetCourseOptionsAsync(tenantId);
            var subjects = await _repo.GetSubjectOptionsAsync(tenantId);

            return new SyllabusIndexViewModel
            {
                CourseFilter = courseId,
                SubjectFilter = subjectId,
                StatusFilter = status,
                Search = search,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total,
                CourseOptions = courses.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == courseId }).ToList(),
                SubjectOptions = subjects.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name, Selected = s.Id == subjectId }).ToList(),
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
                    CourseName = s.CourseName,
                    SubjectName = s.SubjectName,
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
            if (string.IsNullOrWhiteSpace(model.UnitTitle)) return ServiceResult.Fail("Unit title is required.");
            if (model.CourseId == Guid.Empty) return ServiceResult.Fail("Course is required.");
            if (model.SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (model.UnitNumber <= 0) return ServiceResult.Fail("Unit number must be greater than zero.");

            try
            {
                var entity = new SubjectSyllabus
                {
                    SS_Id = Guid.NewGuid(),
                    SS_TenantId = tenantId,
                    SS_CourseId = model.CourseId,
                    SS_SubjectId = model.SubjectId,
                    SS_UnitNumber = model.UnitNumber,
                    SS_UnitTitle = model.UnitTitle,
                    SS_Description = model.Description,
                    SS_TotalHours = model.TotalHours,
                    SS_FileUrl = model.FileUrl
                };

                var id = await _repo.CreateAsync(entity);
                return ServiceResult.Ok("Syllabus unit added successfully.", id);
            }
            catch (SqlException ex)
            {
                // THROW 53001 (duplicate unit number) surfaces here
                return ServiceResult.Fail(ex.Message);
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while saving. Please try again.");
            }
        }

        public async Task<ServiceResult> UpdateAsync(SyllabusFormViewModel model, Guid tenantId)
        {
            if (!model.SS_Id.HasValue) return ServiceResult.Fail("Syllabus Id is required.");
            if (string.IsNullOrWhiteSpace(model.UnitTitle)) return ServiceResult.Fail("Unit title is required.");
            if (model.CourseId == Guid.Empty) return ServiceResult.Fail("Course is required.");
            if (model.SubjectId == Guid.Empty) return ServiceResult.Fail("Subject is required.");
            if (model.UnitNumber <= 0) return ServiceResult.Fail("Unit number must be greater than zero.");

            try
            {
                var entity = new SubjectSyllabus
                {
                    SS_Id = model.SS_Id.Value,
                    SS_TenantId = tenantId,
                    SS_CourseId = model.CourseId,
                    SS_SubjectId = model.SubjectId,
                    SS_UnitNumber = model.UnitNumber,
                    SS_UnitTitle = model.UnitTitle,
                    SS_Description = model.Description,
                    SS_TotalHours = model.TotalHours,
                    SS_FileUrl = model.FileUrl
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

        private async Task PopulateDropdownsAsync(SyllabusFormViewModel vm, Guid tenantId)
        {
            var courses = await _repo.GetCourseOptionsAsync(tenantId);
            var subjects = await _repo.GetSubjectOptionsAsync(tenantId);

            vm.CourseOptions = courses.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == vm.CourseId }).ToList();
            vm.SubjectOptions = subjects.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.Name} ({s.Code})", Selected = s.Id == vm.SubjectId }).ToList();
        }
    }
}
