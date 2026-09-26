using System;
using System.Threading.Tasks;
using IMS.Models.Backup;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IMS.Web.Controllers
{
    [Route("DataBackup")]
    public class DataBackupController : Controller
    {
        private readonly IDataBackupService _backupService;
        private readonly ILogger<DataBackupController> _logger;

        public DataBackupController(IDataBackupService backupService, ILogger<DataBackupController> logger)
        {
            _backupService = backupService;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get
            {
                var raw = User.FindFirst("tenant_id")?.Value;
                if (Guid.TryParse(raw, out var id) && id != Guid.Empty)
                    return id;
                return new Guid("4150d676-e932-49af-a9d3-dc673fc1891d");
            }
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _backupService.GetDashboardModelAsync(CurrentTenantId);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading DataBackup dashboard");
                var fallbackModel = new DataBackupIndexViewModel
                {
                    CurrentFilter = new DataBackupFilterViewModel { Module = "Students" }
                };
                return View(fallbackModel);
            }
        }

        [HttpPost("Preview")]
        public async Task<IActionResult> Preview([FromBody] DataBackupFilterViewModel filter)
        {
            if (filter == null)
                return BadRequest(new { success = false, message = "Invalid filter criteria." });

            var result = await _backupService.GetPreviewAsync(filter, CurrentTenantId);
            return Json(result);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export([FromForm] DataBackupFilterViewModel filter)
        {
            if (filter == null || string.IsNullOrWhiteSpace(filter.Module))
            {
                TempData["ErrorMessage"] = "Please select a valid dataset to export.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var exportResult = await _backupService.ExportModuleAsync(filter, CurrentTenantId);
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting module {Module}", filter?.Module);
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("ExportAll")]
        public async Task<IActionResult> ExportAll([FromForm] DataBackupFilterViewModel filter)
        {
            try
            {
                filter ??= new DataBackupFilterViewModel();
                var exportResult = await _backupService.ExportCompleteBackupZipAsync(filter, CurrentTenantId);
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting complete system backup");
                TempData["ErrorMessage"] = $"Full backup failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
