using IMS.Models.SubjectSyllabus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface ISubjectSyllabusService
    {
        Task<SyllabusIndexViewModel> GetListAsync(
            Guid tenantId, Guid? courseId, Guid? subjectId, string? status, string? search, int page, int pageSize);

        Task<SyllabusFormViewModel> GetEmptyFormAsync(Guid tenantId);
        Task<SyllabusFormViewModel?> GetForEditAsync(Guid id, Guid tenantId);

        Task<ServiceResult> CreateAsync(SyllabusFormViewModel model, Guid tenantId);
        Task<ServiceResult> UpdateAsync(SyllabusFormViewModel model, Guid tenantId);
        Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
        Task<ServiceResult> ToggleCompletedAsync(Guid id, Guid tenantId, bool isCompleted);
        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);
        Task<List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>> GetCoursesAsync(Guid tenantId);
        Task<List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>> GetSubjectsByCourseAsync(Guid? courseId, Guid tenantId);
    }
}
