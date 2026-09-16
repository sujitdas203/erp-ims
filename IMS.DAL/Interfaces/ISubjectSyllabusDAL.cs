using IMS.Models.SubjectSyllabus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Interfaces
{
    public interface ISubjectSyllabusDAL
    {
        Task<Guid> CreateAsync(SubjectSyllabus s);
        Task<bool> UpdateAsync(SubjectSyllabus s);
        Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
        Task<bool> ToggleCompletedAsync(Guid id, Guid tenantId, bool isCompleted);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
        Task<SubjectSyllabus?> GetByIdAsync(Guid id, Guid tenantId);

        Task<(List<SubjectSyllabus> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? courseId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize);

        Task<List<(Guid Id, string Name)>> GetCourseOptionsAsync(Guid tenantId);
        Task<List<(Guid Id, string Name, string Code)>> GetSubjectOptionsAsync(Guid tenantId);
    }
}
