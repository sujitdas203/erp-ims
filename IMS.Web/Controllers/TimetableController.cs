using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Helpers.Constants;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class TimetableController : Controller
    {
        private readonly ITimetableService _service;
        private readonly ILogger<TimetableController> _logger;

        public TimetableController(ITimetableService service, ILogger<TimetableController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get
            {
                var raw = User.FindFirst("tenant_id")?.Value;
                return Guid.TryParse(raw, out var id) && id != Guid.Empty ? id : HardcodedMasterData.CurrentTenantId;
            }
        }

        public async Task<IActionResult> Index(Guid? batchId, Guid? branchId)
        {
            var vm = await _service.GetListAsync(CurrentTenantId, batchId, branchId);
            return View(vm);
        }

        public IActionResult Create(Guid? batchId)
        {
            var vm = new TimetableFormViewModel
            {
                TT_BatchId = batchId ?? Guid.Empty,
                TT_DayOfWeek = 1
            };
            _service.PopulateDropdowns(vm);
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var vm = await _service.GetForEditAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            _service.PopulateDropdowns(vm);
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _service.GetDetailsAsync(id, CurrentTenantId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CheckConflict(Guid batchId, int dayOfWeek, string startTime, string endTime, Guid? excludeId)
        {
            var hasConflict = await _service.CheckConflictAsync(CurrentTenantId, batchId, dayOfWeek, startTime, endTime, excludeId);
            return Json(new { conflict = hasConflict });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimetable(TimetableFormViewModel model)
        {
            var errors = ValidateTimetable(model);
            if (errors.Count > 0)
            {
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var r = await _service.CreateAsync(model, CurrentTenantId);
                if (!r.Success)
                {
                    var customErrors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (r.Message != null && r.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase))
                    {
                        customErrors["TT_StartTime"] = r.Message;
                        customErrors["TT_EndTime"] = r.Message;
                    }
                    else if (r.Message != null && r.Message.Contains("Day", StringComparison.OrdinalIgnoreCase))
                    {
                        customErrors["TT_DayOfWeek"] = r.Message;
                    }
                    return Json(new { success = false, message = r.Message, errors = customErrors });
                }
                return Json(new { success = true, message = r.Message, id = r.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating timetable");
                return Json(new { success = false, message = ex.Message ?? "Something went wrong. Please try again." });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimetable(TimetableFormViewModel model)
        {
            var errors = ValidateTimetable(model);
            if (!model.TT_Id.HasValue || model.TT_Id.Value == Guid.Empty)
            {
                errors["TT_Id"] = "Timetable ID is required.";
            }

            if (errors.Count > 0)
            {
                return Json(new { success = false, message = "Please correct the highlighted fields.", errors });
            }

            try
            {
                var r = await _service.UpdateAsync(model, CurrentTenantId);
                if (!r.Success)
                {
                    var customErrors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (r.Message != null && r.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase))
                    {
                        customErrors["TT_StartTime"] = r.Message;
                        customErrors["TT_EndTime"] = r.Message;
                    }
                    else if (r.Message != null && r.Message.Contains("Day", StringComparison.OrdinalIgnoreCase))
                    {
                        customErrors["TT_DayOfWeek"] = r.Message;
                    }
                    return Json(new { success = false, message = r.Message, errors = customErrors });
                }
                return Json(new { success = true, message = r.Message, id = r.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating timetable");
                return Json(new { success = false, message = ex.Message ?? "Something went wrong. Please try again." });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTimetable(Guid id)
        {
            try
            {
                var r = await _service.DeleteAsync(id, CurrentTenantId);
                return Json(new { success = r.Success, message = r.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting timetable");
                return Json(new { success = false, message = ex.Message ?? "Something went wrong. Please try again." });
            }
        }

        private static Dictionary<string, string> ValidateTimetable(TimetableFormViewModel model)
        {
            var errors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (model == null)
            {
                errors["General"] = "Form submission is empty.";
                return errors;
            }

            if (model.TT_BranchId == Guid.Empty)
                errors["TT_BranchId"] = "Please select a Branch.";

            if (model.TT_BatchId == Guid.Empty)
                errors["TT_BatchId"] = "Please select a Batch.";

            if (model.TT_SubjectId == Guid.Empty)
                errors["TT_SubjectId"] = "Please select a Subject.";

            if (model.TT_StaffId == Guid.Empty)
                errors["TT_StaffId"] = "Please select a Staff member.";

            if (model.TT_DayOfWeek < 1 || model.TT_DayOfWeek > 7)
                errors["TT_DayOfWeek"] = "Please select a Day of Week (Monday to Sunday).";

            if (string.IsNullOrWhiteSpace(model.TT_StartTime) || !TimeSpan.TryParse(model.TT_StartTime, out var start))
                errors["TT_StartTime"] = "Please enter a valid Start Time.";

            if (string.IsNullOrWhiteSpace(model.TT_EndTime) || !TimeSpan.TryParse(model.TT_EndTime, out var end))
                errors["TT_EndTime"] = "Please enter a valid End Time.";
            else if (TimeSpan.TryParse(model.TT_StartTime, out var s) && end <= s)
                errors["TT_EndTime"] = "End Time must be after Start Time.";

            if (model.TT_EffectiveFrom.HasValue && model.TT_EffectiveTo.HasValue && model.TT_EffectiveTo.Value < model.TT_EffectiveFrom.Value)
                errors["TT_EffectiveTo"] = "Effective To must be on or after Effective From.";

            return errors;
        }
    }
}
