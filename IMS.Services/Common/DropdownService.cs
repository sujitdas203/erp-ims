using System;
using System.Collections.Generic;
using System.Linq;
using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Common.Dropdown;
using IMS.Services.Interfaces;

namespace IMS.Services
{
    /// <summary>
    /// Generic Dropdown Service
    /// Responsible for validating requests and
    /// delegating dropdown retrieval to the DAL.
    /// </summary>
    public class DropdownService : IDropdownService
    {
        private readonly IDropdownDAL _dropdownDAL;

        public DropdownService(IDropdownDAL dropdownDAL)
        {
            _dropdownDAL = dropdownDAL;
        }

        /// <summary>
        /// Returns dropdown items for the specified entity.
        /// </summary>
        public List<DropdownItemModel> GetDropdown(DropdownRequestModel request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.EntityType))
                throw new ArgumentException("EntityType is required.");

            // Special-case Class and Section to return from Master data if DB table is not present
            if (string.Equals(request.EntityType, "Class", StringComparison.OrdinalIgnoreCase))
            {
                return HardcodedMasterData.Classes.Select(c => new DropdownItemModel
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Code = c.Name,
                    IsActive = true
                }).ToList();
            }

            if (string.Equals(request.EntityType, "Section", StringComparison.OrdinalIgnoreCase))
            {
                return HardcodedMasterData.Sections.Select(s => new DropdownItemModel
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    Code = s.Name,
                    IsActive = true
                }).ToList();
            }

            if (string.Equals(request.EntityType, "TCStatus", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var tcConfig = DropdownConfigRegistry.GetByEntityType(request.EntityType);
                    if (tcConfig != null)
                    {
                        var dbItems = _dropdownDAL.GetDropdown(tcConfig, request);
                        if (dbItems != null && dbItems.Count > 0) return dbItems;
                    }
                }
                catch { }

                return new List<string> { "Submitted", "Pending Clearances", "Under Review", "Approved", "Issued", "Rejected" }
                    .Select(s => new DropdownItemModel { Value = s, Text = s, Code = s, IsActive = true }).ToList();
            }

            if (string.Equals(request.EntityType, "ActiveStatus", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var actConfig = DropdownConfigRegistry.GetByEntityType(request.EntityType);
                    if (actConfig != null)
                    {
                        var dbItems = _dropdownDAL.GetDropdown(actConfig, request);
                        if (dbItems != null && dbItems.Count > 0) return dbItems;
                    }
                }
                catch { }

                return new List<string> { "Pending", "Approved", "Rejected", "Cancelled" }
                    .Select(s => new DropdownItemModel { Value = s, Text = s, Code = s, IsActive = true }).ToList();
            }

            if (string.Equals(request.EntityType, "DayOfWeek", StringComparison.OrdinalIgnoreCase))
            {
                return new List<DropdownItemModel>
                {
                    new() { Value = "1", Text = "Monday", Code = "MON", IsActive = true },
                    new() { Value = "2", Text = "Tuesday", Code = "TUE", IsActive = true },
                    new() { Value = "3", Text = "Wednesday", Code = "WED", IsActive = true },
                    new() { Value = "4", Text = "Thursday", Code = "THU", IsActive = true },
                    new() { Value = "5", Text = "Friday", Code = "FRI", IsActive = true },
                    new() { Value = "6", Text = "Saturday", Code = "SAT", IsActive = true },
                    new() { Value = "7", Text = "Sunday", Code = "SUN", IsActive = true }
                };
            }

            var config = DropdownConfigRegistry.GetByEntityType(request.EntityType);

            if (config == null)
                throw new Exception(
                    $"Dropdown configuration not found for EntityType '{request.EntityType}'.");

            return _dropdownDAL.GetDropdown(config, request);
        }
    }
}