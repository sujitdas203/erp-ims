using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Announcement;
using IMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementDAL _repo;

        public AnnouncementService(IAnnouncementDAL repo)
        {
            _repo = repo;
        }

        public async Task<AnnouncementIndexViewModel> GetListAsync(
            Guid tenantId, Guid? branchId, string? status, string? search, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var (items, total) = await _repo.GetPagedAsync(tenantId, branchId, status, search, page, pageSize);

            return new AnnouncementIndexViewModel
            {
                BranchFilter = branchId,
                StatusFilter = status,
                Search = search,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total,
                BranchOptions = HardcodedMasterData.GetBranchSelectList(branchId),
                StatusOptions = new()
                {
                    new() { Value = "Draft", Text = "Draft", Selected = status == "Draft" },
                    new() { Value = "Scheduled", Text = "Scheduled", Selected = status == "Scheduled" },
                    new() { Value = "Active", Text = "Active", Selected = status == "Active" },
                    new() { Value = "Expired", Text = "Expired", Selected = status == "Expired" },
                    new() { Value = "Inactive", Text = "Inactive", Selected = status == "Inactive" }
                },
                Announcements = items.Select(a => new AnnouncementListItemViewModel
                {
                    ANN_Id = a.ANN_Id,
                    Title = a.ANN_Title,
                    BranchName = a.ANN_BranchId.HasValue ? HardcodedMasterData.GetBranchName(a.ANN_BranchId.Value) : "All Branches",
                    Status = a.ComputedStatus,
                    PublishedAt = a.ANN_PublishedAt,
                    ExpiresAt = a.ANN_ExpiresAt,
                    IsActive = a.ANN_IsActive,
                    CreatedAt = a.ANN_CreatedAt
                }).ToList()
            };
        }

        public async Task<AnnouncementFormViewModel?> GetForEditAsync(Guid id, Guid tenantId)
        {
            var a = await _repo.GetByIdAsync(id, tenantId);
            if (a == null) return null;

            return new AnnouncementFormViewModel
            {
                ANN_Id = a.ANN_Id,
                BranchId = a.ANN_BranchId,
                Title = a.ANN_Title,
                Content = a.ANN_Content,
                PublishedAt = a.ANN_PublishedAt,
                ExpiresAt = a.ANN_ExpiresAt,
                IsActive = a.ANN_IsActive,
                BranchOptions = HardcodedMasterData.GetBranchSelectList(a.ANN_BranchId)
            };
        }

        public async Task<ServiceResult> CreateAsync(AnnouncementFormViewModel model, Guid tenantId, Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(model.Title)) return ServiceResult.Fail("Title is required.");
            if (string.IsNullOrWhiteSpace(model.Content)) return ServiceResult.Fail("Content is required.");

            try
            {
                var entity = new Announcement
                {
                    ANN_Id = Guid.NewGuid(),
                    ANN_TenantId = tenantId,
                    ANN_BranchId = model.BranchId,
                    ANN_Title = model.Title,
                    ANN_Content = model.Content,
                    ANN_PublishedAt = model.PublishedAt,
                    ANN_ExpiresAt = model.ExpiresAt,
                    ANN_CreatedBy = createdBy
                };

                var id = await _repo.CreateAsync(entity);
                return ServiceResult.Ok("Announcement created successfully.", id);
            }
            catch (SqlException ex)
            {
                // THROW 52001 inside SP_Announcements_Create (expiry before publish) surfaces here
                return ServiceResult.Fail(ex.Message);
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while saving the announcement. Please try again.");
            }
        }

        public async Task<ServiceResult> UpdateAsync(AnnouncementFormViewModel model, Guid tenantId)
        {
            if (!model.ANN_Id.HasValue) return ServiceResult.Fail("Announcement Id is required.");
            if (string.IsNullOrWhiteSpace(model.Title)) return ServiceResult.Fail("Title is required.");
            if (string.IsNullOrWhiteSpace(model.Content)) return ServiceResult.Fail("Content is required.");

            try
            {
                var entity = new Announcement
                {
                    ANN_Id = model.ANN_Id.Value,
                    ANN_TenantId = tenantId,
                    ANN_BranchId = model.BranchId,
                    ANN_Title = model.Title,
                    ANN_Content = model.Content,
                    ANN_PublishedAt = model.PublishedAt,
                    ANN_ExpiresAt = model.ExpiresAt
                };

                var success = await _repo.UpdateAsync(entity);
                return success
                    ? ServiceResult.Ok("Announcement updated successfully.")
                    : ServiceResult.Fail("Announcement not found.");
            }
            catch (SqlException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while saving the announcement. Please try again.");
            }
        }

        public async Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            try
            {
                var success = await _repo.ToggleActiveAsync(id, tenantId, isActive);
                return success
                    ? ServiceResult.Ok(isActive ? "Announcement activated." : "Announcement deactivated.")
                    : ServiceResult.Fail("Announcement not found.");
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong. Please try again.");
            }
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId)
        {
            try
            {
                var success = await _repo.DeleteAsync(id, tenantId);
                return success
                    ? ServiceResult.Ok("Announcement deleted.")
                    : ServiceResult.Fail("Announcement not found.");
            }
            catch (Exception)
            {
                return ServiceResult.Fail("Something went wrong while deleting. Please try again.");
            }
        }
    }
}
