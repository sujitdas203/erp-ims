using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Helpers.Constants
{
    public static class HardcodedMasterData
    {
        // TODO: replace with real tenant resolution (auth claims / session)
        public static readonly Guid CurrentTenantId = new("11111111-1111-1111-1111-111111111111");

        public static readonly List<(Guid Id, string Name)> Branches = new()
        {
            (new Guid("22222222-2222-2222-2222-222222222201"), "Main Campus"),
            (new Guid("22222222-2222-2222-2222-222222222202"), "North Branch"),
            (new Guid("22222222-2222-2222-2222-222222222203"), "South Branch"),
            (new Guid("22222222-2222-2222-2222-222222222204"), "East Wing"),
            (new Guid("22222222-2222-2222-2222-222222222205"), "West Campus"),
        };

        // K-12 scoped class list (Nursery to 12th Grade)
        public static readonly List<(Guid Id, string Name)> Classes = new()
        {
            (new Guid("33333333-3333-3333-3333-333333333301"), "Nursery"),
            (new Guid("33333333-3333-3333-3333-333333333302"), "LKG"),
            (new Guid("33333333-3333-3333-3333-333333333303"), "UKG"),
            (new Guid("33333333-3333-3333-3333-333333333304"), "Class 1"),
            (new Guid("33333333-3333-3333-3333-333333333305"), "Class 2"),
            (new Guid("33333333-3333-3333-3333-333333333306"), "Class 3"),
            (new Guid("33333333-3333-3333-3333-333333333307"), "Class 4"),
            (new Guid("33333333-3333-3333-3333-333333333308"), "Class 5"),
            (new Guid("33333333-3333-3333-3333-333333333309"), "Class 6"),
            (new Guid("33333333-3333-3333-3333-333333333310"), "Class 7"),
            (new Guid("33333333-3333-3333-3333-333333333311"), "Class 8"),
            (new Guid("33333333-3333-3333-3333-333333333312"), "Class 9"),
            (new Guid("33333333-3333-3333-3333-333333333313"), "Class 10"),
            (new Guid("33333333-3333-3333-3333-333333333314"), "Class 11"),
            (new Guid("33333333-3333-3333-3333-333333333315"), "Class 12"),
        };

        public static readonly List<(Guid Id, string Name)> Sections = new()
        {
            (new Guid("44444444-4444-4444-4444-444444444401"), "A"),
            (new Guid("44444444-4444-4444-4444-444444444402"), "B"),
            (new Guid("44444444-4444-4444-4444-444444444403"), "C"),
            (new Guid("44444444-4444-4444-4444-444444444404"), "D"),
            (new Guid("44444444-4444-4444-4444-444444444405"), "E"),
        };

        public static readonly List<string> Genders = new() { "Male", "Female", "Other" };

        public static readonly List<string> BloodGroups = new()
        {
            "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };

        public static readonly List<string> GuardianRelations = new()
        {
            "Father", "Mother", "Guardian", "Grandfather", "Grandmother", "Other"
        };

        public static readonly List<string> StudentStatuses = new()
        {
            "Admitted", "Active", "Inactive", "Transferred", "Alumni", "Dropped"
        };

        public static readonly List<string> EnrollmentTypes = new()
        {
            "New Admission", "Promotion", "Transfer", "Re-admission"
        };

        public static readonly List<string> EnrollmentStatuses = new()
        {
            "Active", "Completed", "Withdrawn", "Transferred", "Suspended"
        };

        // ---------- SelectList builders ----------

        public static List<SelectListItem> GetBranchSelectList(Guid? selected = null) =>
            Branches.ConvertAll(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name, Selected = selected == b.Id });

        public static List<SelectListItem> GetClassSelectList(Guid? selected = null) =>
            Classes.ConvertAll(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = selected == c.Id });

        public static List<SelectListItem> GetSectionSelectList(Guid? selected = null) =>
            Sections.ConvertAll(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name, Selected = selected == s.Id });

        public static List<SelectListItem> GetGenderSelectList(string? selected = null) =>
            Genders.ConvertAll(g => new SelectListItem { Value = g, Text = g, Selected = g == selected });

        public static List<SelectListItem> GetBloodGroupSelectList(string? selected = null) =>
            BloodGroups.ConvertAll(b => new SelectListItem { Value = b, Text = b, Selected = b == selected });

        public static List<SelectListItem> GetRelationSelectList(string? selected = null) =>
            GuardianRelations.ConvertAll(r => new SelectListItem { Value = r, Text = r, Selected = r == selected });

        public static List<SelectListItem> GetStatusSelectList(string? selected = null) =>
            StudentStatuses.ConvertAll(s => new SelectListItem { Value = s, Text = s, Selected = s == selected });

        public static List<SelectListItem> GetEnrollmentTypeSelectList(string? selected = null) =>
            EnrollmentTypes.ConvertAll(t => new SelectListItem { Value = t, Text = t, Selected = t == selected });

        public static List<SelectListItem> GetEnrollmentStatusSelectList(string? selected = null) =>
            EnrollmentStatuses.ConvertAll(s => new SelectListItem { Value = s, Text = s, Selected = s == selected });

        // ---------- Name lookups (Id -> display name, for list/detail views) ----------

        public static string GetBranchName(Guid? branchId) =>
            branchId.HasValue ? (Branches.Find(b => b.Id == branchId.Value).Name ?? "-") : "-";

        public static string GetClassName(Guid? classId) =>
            classId.HasValue ? (Classes.Find(c => c.Id == classId.Value).Name ?? "-") : "-";

        public static string GetSectionName(Guid? sectionId) =>
            sectionId.HasValue ? (Sections.Find(s => s.Id == sectionId.Value).Name ?? "-") : "-";
    }
}
