using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Models.Backup;
using IMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IMS.Services.Backup
{
    public class DataBackupService : IDataBackupService
    {
        private readonly IDataBackupDAL _backupDAL;
        private readonly ILogger<DataBackupService> _logger;

        public DataBackupService(IDataBackupDAL backupDAL, ILogger<DataBackupService> logger)
        {
            _backupDAL = backupDAL;
            _logger = logger;
        }

        public Task<DataBackupIndexViewModel> GetDashboardModelAsync(Guid tenantId)
        {
            var modules = _backupDAL.GetAvailableModules(tenantId);
            var totalCount = modules.Sum(m => m.EstimatedCount);

            var model = new DataBackupIndexViewModel
            {
                AvailableModules = modules,
                CurrentFilter = new DataBackupFilterViewModel
                {
                    Module = modules.FirstOrDefault()?.Code ?? "Students"
                },
                TotalSystemRecords = totalCount,
                LastBackupDate = DateTime.UtcNow
            };

            return Task.FromResult(model);
        }

        public Task<DataBackupPreviewResult> GetPreviewAsync(DataBackupFilterViewModel filter, Guid tenantId)
        {
            try
            {
                var dt = _backupDAL.GetModuleData(filter.Module, filter, tenantId);
                var columns = new List<string>();
                foreach (DataColumn c in dt.Columns)
                {
                    columns.Add(c.ColumnName);
                }

                var sampleRows = new List<Dictionary<string, object>>();
                int take = Math.Min(dt.Rows.Count, 15);
                for (int i = 0; i < take; i++)
                {
                    var row = dt.Rows[i];
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn c in dt.Columns)
                    {
                        dict[c.ColumnName] = row[c] != DBNull.Value ? row[c] : "";
                    }
                    sampleRows.Add(dict);
                }

                var modules = _backupDAL.GetAvailableModules(tenantId);
                var modInfo = modules.FirstOrDefault(m => string.Equals(m.Code, filter.Module, StringComparison.OrdinalIgnoreCase));

                return Task.FromResult(new DataBackupPreviewResult
                {
                    Success = true,
                    ModuleName = modInfo?.DisplayName ?? filter.Module,
                    TotalRecords = dt.Rows.Count,
                    ColumnNames = columns,
                    SampleRows = sampleRows
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting data backup preview for module {Module}", filter?.Module);
                return Task.FromResult(new DataBackupPreviewResult
                {
                    Success = false,
                    Message = ex.Message,
                    ModuleName = filter?.Module ?? "Unknown",
                    TotalRecords = 0
                });
            }
        }

        public Task<DataExportFileResult> ExportModuleAsync(DataBackupFilterViewModel filter, Guid tenantId)
        {
            try
            {
                var dt = _backupDAL.GetModuleData(filter.Module, filter, tenantId);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var cleanModule = (filter.Module ?? "Data").Replace(" ", "_");

                var format = filter.Format?.ToLowerInvariant() ?? "csv";

                if (format == "excel" || format == "xlsx" || format == "xls")
                {
                    var xmlBytes = GenerateExcelXml(dt, filter.Module);
                    return Task.FromResult(new DataExportFileResult
                    {
                        FileBytes = xmlBytes,
                        ContentType = "application/vnd.ms-excel",
                        FileName = $"IMS_{cleanModule}_Export_{timestamp}.xls",
                        RecordCount = dt.Rows.Count
                    });
                }
                else
                {
                    var csvBytes = GenerateCsv(dt);
                    return Task.FromResult(new DataExportFileResult
                    {
                        FileBytes = csvBytes,
                        ContentType = "text/csv; charset=utf-8",
                        FileName = $"IMS_{cleanModule}_Export_{timestamp}.csv",
                        RecordCount = dt.Rows.Count
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting module {Module}", filter?.Module);
                throw;
            }
        }

        public Task<DataExportFileResult> ExportCompleteBackupZipAsync(DataBackupFilterViewModel filter, Guid tenantId)
        {
            try
            {
                var modules = _backupDAL.GetAvailableModules(tenantId);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var isExcel = string.Equals(filter.Format, "excel", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(filter.Format, "xlsx", StringComparison.OrdinalIgnoreCase);

                using var memoryStream = new MemoryStream();
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    int totalRecords = 0;
                    var manifestEntries = new List<string>();

                    foreach (var mod in modules)
                    {
                        try
                        {
                            var modFilter = new DataBackupFilterViewModel
                            {
                                Module = mod.Code,
                                AcademicYearId = filter.AcademicYearId,
                                BranchId = filter.BranchId,
                                CourseId = filter.CourseId,
                                BatchId = filter.BatchId,
                                StartDate = filter.StartDate,
                                EndDate = filter.EndDate,
                                Status = filter.Status
                            };

                            var dt = _backupDAL.GetModuleData(mod.Code, modFilter, tenantId);
                            totalRecords += dt.Rows.Count;
                            manifestEntries.Add($"- {mod.DisplayName} ({mod.Code}): {dt.Rows.Count} records");

                            var entryName = isExcel
                                ? $"{mod.Code}_{timestamp}.xls"
                                : $"{mod.Code}_{timestamp}.csv";

                            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                            using var entryStream = entry.Open();

                            byte[] contentBytes = isExcel
                                ? GenerateExcelXml(dt, mod.DisplayName)
                                : GenerateCsv(dt);

                            entryStream.Write(contentBytes, 0, contentBytes.Length);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error extracting module {Module} for zip backup", mod.Code);
                            manifestEntries.Add($"- {mod.DisplayName} ({mod.Code}): FAILED ({ex.Message})");
                        }
                    }

                    // Add a Readme / Summary text file inside ZIP
                    var readmeEntry = archive.CreateEntry("BACKUP_MANIFEST.txt", CompressionLevel.Fastest);
                    using (var writer = new StreamWriter(readmeEntry.Open(), Encoding.UTF8))
                    {
                        writer.WriteLine("=================================================");
                        writer.WriteLine("ERP-IMS SYSTEM DATA BACKUP ARCHIVE");
                        writer.WriteLine($"Export Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        writer.WriteLine($"Total Records Extracted: {totalRecords}");
                        writer.WriteLine($"Total Datasets: {modules.Count}");
                        writer.WriteLine("=================================================");
                        foreach (var entry in manifestEntries)
                        {
                            writer.WriteLine(entry);
                        }
                    }
                }

                memoryStream.Position = 0;
                var zipBytes = memoryStream.ToArray();

                return Task.FromResult(new DataExportFileResult
                {
                    FileBytes = zipBytes,
                    ContentType = "application/zip",
                    FileName = $"ERP_IMS_Full_Database_Backup_{timestamp}.zip",
                    RecordCount = modules.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting complete zip backup");
                throw;
            }
        }

        private byte[] GenerateCsv(DataTable dt)
        {
            var sb = new StringBuilder();

            // Append UTF-8 BOM so Excel opens CSV with proper encoding
            var bom = new byte[] { 0xEF, 0xBB, 0xBF };

            // Header row
            var columnNames = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                columnNames.Add(EscapeCsvCell(col.ColumnName));
            }
            sb.AppendLine(string.Join(",", columnNames));

            // Data rows
            foreach (DataRow row in dt.Rows)
            {
                var rowValues = new List<string>();
                foreach (DataColumn col in dt.Columns)
                {
                    var val = row[col];
                    rowValues.Add(EscapeCsvCell(val != DBNull.Value ? val?.ToString() ?? "" : ""));
                }
                sb.AppendLine(string.Join(",", rowValues));
            }

            var textBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var combinedBytes = new byte[bom.Length + textBytes.Length];
            Buffer.BlockCopy(bom, 0, combinedBytes, 0, bom.Length);
            Buffer.BlockCopy(textBytes, 0, combinedBytes, bom.Length, textBytes.Length);

            return combinedBytes;
        }

        private string EscapeCsvCell(string cell)
        {
            if (string.IsNullOrEmpty(cell)) return "";

            bool mustQuote = cell.Contains(",") || cell.Contains("\"") || cell.Contains("\r") || cell.Contains("\n");
            if (mustQuote)
            {
                return "\"" + cell.Replace("\"", "\"\"") + "\"";
            }
            return cell;
        }

        private byte[] GenerateExcelXml(DataTable dt, string sheetName)
        {
            var cleanSheetName = string.IsNullOrWhiteSpace(sheetName) ? "Data" : sheetName.Replace("/", "-").Replace("\\", "-");
            if (cleanSheetName.Length > 31) cleanSheetName = cleanSheetName.Substring(0, 31);

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");

            sb.AppendLine("<Styles>");
            sb.AppendLine("<Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Center\"/><Font ss:FontName=\"Segoe UI\" ss:Size=\"10\"/></Style>");
            sb.AppendLine("<Style ss:ID=\"HeaderStyle\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#CBD5E1\"/></Borders><Font ss:FontName=\"Segoe UI\" ss:Size=\"10\" ss:Color=\"#FFFFFF\" ss:Bold=\"1\"/><Interior ss:Color=\"#2563EB\" ss:Pattern=\"Solid\"/></Style>");
            sb.AppendLine("<Style ss:ID=\"RowEven\"><Alignment ss:Vertical=\"Center\"/><Interior ss:Color=\"#F8FAFC\" ss:Pattern=\"Solid\"/></Style>");
            sb.AppendLine("<Style ss:ID=\"RowOdd\"><Alignment ss:Vertical=\"Center\"/><Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/></Style>");
            sb.AppendLine("</Styles>");

            sb.AppendLine($"<Worksheet ss:Name=\"{System.Security.SecurityElement.Escape(cleanSheetName)}\">");
            sb.AppendLine($"<Table ss:ExpandedColumnCount=\"{Math.Max(dt.Columns.Count, 1)}\" ss:ExpandedRowCount=\"{dt.Rows.Count + 1}\" x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultRowHeight=\"20\">");

            // Define column widths
            foreach (DataColumn col in dt.Columns)
            {
                int width = Math.Max(col.ColumnName.Length * 10 + 25, 110);
                sb.AppendLine($"<Column ss:AutoFitWidth=\"1\" ss:Width=\"{width}\"/>");
            }

            // Header Row
            sb.AppendLine("<Row ss:StyleID=\"HeaderStyle\" ss:Height=\"24\">");
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendLine($"<Cell><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(col.ColumnName)}</Data></Cell>");
            }
            sb.AppendLine("</Row>");

            // Data Rows
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                var styleId = rowIndex % 2 == 0 ? "RowOdd" : "RowEven";
                sb.AppendLine($"<Row ss:StyleID=\"{styleId}\">");
                foreach (DataColumn col in dt.Columns)
                {
                    var val = row[col];
                    if (val == DBNull.Value || val == null)
                    {
                        sb.AppendLine("<Cell><Data ss:Type=\"String\"></Data></Cell>");
                    }
                    else if (decimal.TryParse(val.ToString(), out var numVal) && !col.ColumnName.Contains("Code") && !col.ColumnName.Contains("Number") && !col.ColumnName.Contains("Phone") && !col.ColumnName.Contains("Roll") && !col.ColumnName.Contains("Pincode"))
                    {
                        sb.AppendLine($"<Cell><Data ss:Type=\"Number\">{numVal}</Data></Cell>");
                    }
                    else
                    {
                        sb.AppendLine($"<Cell><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(val.ToString())}</Data></Cell>");
                    }
                }
                sb.AppendLine("</Row>");
                rowIndex++;
            }

            sb.AppendLine("</Table>");
            sb.AppendLine("</Worksheet>");
            sb.AppendLine("</Workbook>");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
