using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Helpers;

public static class ExcelFile
{
    public static async Task<(
        List<MaintenanceTracking> ValidItems,
        List<string> Errors
    )> ParseExcelAsync(IFormFile file, IPartService partServices, string vehicleId)
    {
        var results = new List<MaintenanceTracking>();
        var errors = new List<string>();

        if (file == null || file.Length == 0)
        {
            errors.Add("No file uploaded.");
            return (results, errors);
        }

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            // Set EPPlus license context

            using var package = new ExcelPackage(stream);
            var wsData = package.Workbook.Worksheets["MaintenanceData"];
            var wsLookup = package.Workbook.Worksheets["Lookup"];

            if (wsData == null)
            {
                errors.Add("Worksheet 'MaintenanceData' not found in Excel file.");
                return (results, errors);
            }

            if (wsLookup == null)
            {
                errors.Add("Worksheet 'Lookup' not found in Excel file.");
                return (results, errors);
            }

            // Validate headers (row 2)
            var expectedHeaders = new[] { "Part", "Last Change Date", "Last Change KM" };
            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                var header = wsData.Cells[2, i + 1].Text?.Trim();
                if (!string.Equals(header, expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add(
                        $"Expected column '{expectedHeaders[i]}' at position {i + 1}, but got '{header ?? "empty"}'"
                    );
                    return (results, errors);
                }
            }

            // Parse Maintenance History (starting at row 3)
            for (int row = 3; row <= wsData.Dimension.End.Row; row++)
            {
                try
                {
                    var partName = wsData.Cells[row, 1].Text?.Trim();
                    var dateStr = wsData.Cells[row, 2].Text?.Trim();
                    var kmStr = wsData.Cells[row, 3].Text?.Trim();

                    // Skip rows where both date and KM are empty
                    if (string.IsNullOrWhiteSpace(dateStr) && string.IsNullOrWhiteSpace(kmStr))
                    {
                        continue;
                    }

                    // Map part name to PartId using Lookup sheet
                    string? partId = null;
                    if (!string.IsNullOrWhiteSpace(partName))
                    {
                        var partIdCell = wsLookup
                            .Cells[2, 1, wsLookup.Dimension.End.Row, 1]
                            .FirstOrDefault(c => c.Offset(0, 1).Text?.Trim() == partName);
                        if (partIdCell != null)
                        {
                            partId = partIdCell.Text?.Trim();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(partId))
                    {
                        errors.Add(
                            $"Row {row - 1}: Invalid or missing part name '{partName ?? "empty"}'."
                        );
                        continue;
                    }

                    // Verify part exists using IPartService
                    if (!await partServices.IsExist(partId))
                    {
                        errors.Add($"Row {row - 1}: Part with ID '{partId}' does not exist.");
                        continue;
                    }

                    // Parse date (required)
                    if (
                        string.IsNullOrWhiteSpace(dateStr)
                        || !DateTime.TryParse(dateStr, out var lastChangedDate)
                    )
                    {
                        errors.Add(
                            $"Row {row - 1}: Invalid or missing date in 'Last Change Date'."
                        );
                        continue;
                    }

                    // Parse KM (default to 0 if empty)
                    int kmAtLastChange = 0;
                    if (
                        !string.IsNullOrWhiteSpace(kmStr)
                        && !int.TryParse(kmStr, out kmAtLastChange)
                    )
                    {
                        errors.Add($"Row {row - 1}: Invalid number in 'Last Change KM'.");
                        continue;
                    }

                    results.Add(
                        new MaintenanceTracking
                        {
                            VehicleId = vehicleId,
                            PartId = partId,
                            LastChangedDate = lastChangedDate,
                            KMAtLastChange = kmAtLastChange,
                        }
                    );
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {row - 1}: Unexpected error - {ex.Message}");
                }
            }

            return (results, errors);
        }
        catch (Exception ex)
        {
            errors.Add($"Error processing Excel file: {ex.Message}");
            return (results, errors);
        }
    }

    public static async Task<byte[]> GenerateVehicleTemplate(IPartService partServices)
    {
        var parts = await partServices.GetAllAsync();
        try
        {
            if (!parts.Any())
            {
                throw new InvalidOperationException("No parts provided.");
            }

            // Create Excel package
            using var package = new ExcelPackage();
            var wsData = package.Workbook.Worksheets.Add("MaintenanceData");
            var wsLookup = package.Workbook.Worksheets.Add("Lookup");
            wsLookup.Hidden = eWorkSheetHidden.Hidden;

            // Populate Lookup sheet for part ID mapping
            wsLookup.Cells[1, 1].Value = "Part ID";
            wsLookup.Cells[1, 2].Value = "Part Name";
            for (int i = 0; i < parts.Count; i++)
            {
                wsLookup.Cells[i + 2, 1].Value = parts[i].Id;
                wsLookup.Cells[i + 2, 2].Value = parts[i].Name;
            }

            // --- Maintenance History Section ---
            // Title
            wsData.Cells[1, 1].Value = "Maintenance History";
            wsData.Cells[1, 1, 1, 3].Merge = true;
            wsData.Cells[1, 1].Style.Font.Name = "Calibri";
            wsData.Cells[1, 1].Style.Font.Size = 14;
            wsData.Cells[1, 1].Style.Font.Bold = true;
            wsData.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsData.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 84, 150)); // Dark blue
            wsData.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
            wsData.Cells[1, 1].Style.Locked = true;
            wsData.Cells[1, 1, 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            // Headers
            wsData.Cells[2, 1].Value = "Part";
            wsData.Cells[2, 2].Value = "Last Change Date";
            wsData.Cells[2, 3].Value = "Last Change KM";
            wsData.Cells[2, 1, 2, 3].Style.Font.Name = "Calibri";
            wsData.Cells[2, 1, 2, 3].Style.Font.Size = 12;
            wsData.Cells[2, 1, 2, 3].Style.Font.Bold = true;
            wsData.Cells[2, 1, 2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsData
                .Cells[2, 1, 2, 3]
                .Style.Fill.BackgroundColor.SetColor(Color.FromArgb(75, 119, 190)); // Light blue
            wsData.Cells[2, 1, 2, 3].Style.Font.Color.SetColor(Color.White);
            wsData.Cells[2, 1, 2, 3].Style.Locked = true;
            wsData.Cells[2, 1, 2, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            // Pre-fill part names (read-only) and add validations for date and KM
            for (int i = 0; i < parts.Count; i++)
            {
                int row = 3 + i;
                // Apply alternating row colors
                var rowRange = wsData.Cells[row, 1, row, 3];
                rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rowRange.Style.Fill.BackgroundColor.SetColor(
                    i % 2 == 0 ? Color.FromArgb(232, 240, 232) : Color.White
                ); // Light green or white
                rowRange.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Part name (read-only)
                var partCell = wsData.Cells[row, 1];
                partCell.Value = parts[i].Name;
                partCell.Style.Font.Name = "Calibri";
                partCell.Style.Font.Size = 11;
                partCell.Style.Font.Bold = true;
                partCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                partCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230)); // Light gray
                partCell.Style.Locked = true;
                partCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                Console.WriteLine($"Set part name in {partCell.Address}: {parts[i].Name}");

                // Last Change Date (date format with validation)
                var dateCell = wsData.Cells[row, 2];
                dateCell.Style.Font.Name = "Calibri";
                dateCell.Style.Font.Size = 11;
                dateCell.Style.Numberformat.Format = "yyyy-mm-dd";
                dateCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                var dateValidation = dateCell.DataValidation.AddDateTimeDataValidation();
                dateValidation.Formula.Value = new DateTime(1900, 1, 1);
                dateValidation.Formula2.Value = new DateTime(9999, 12, 31);
                dateValidation.ShowErrorMessage = true;
                dateValidation.ErrorTitle = "Invalid Date";
                dateValidation.Error = "Please enter a valid date (YYYY-MM-DD).";

                // Last Change KM (integer, no scientific notation)
                var kmCell = wsData.Cells[row, 3];
                kmCell.Style.Font.Name = "Calibri";
                kmCell.Style.Font.Size = 11;
                kmCell.Style.Numberformat.Format = "0"; // Prevent scientific notation
                kmCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                var kmValidation = kmCell.DataValidation.AddIntegerDataValidation();
                kmValidation.Formula.ExcelFormula = "0";
                kmValidation.Formula2.ExcelFormula = "1000000";
                kmValidation.ShowErrorMessage = true;
                kmValidation.ErrorTitle = "Invalid KM";
                kmValidation.Error = "Please enter a valid kilometer value (0-1,000,000).";
                Console.WriteLine($"Applied number format to {kmCell.Address}: 0");
            }

            // Format worksheet (set column widths and row heights)
            wsData.Column(1).Width = 20; // Part
            wsData.Column(2).Width = 20; // Date
            wsData.Column(3).Width = 20; // KM
            wsData.Cells[1, 1, 2 + parts.Count, 3].Style.VerticalAlignment =
                ExcelVerticalAlignment.Center;
            wsData.Row(1).Height = 30; // Title
            wsData.Row(2).Height = 25; // Headers
            for (int i = 3; i <= 2 + parts.Count; i++)
            {
                wsData.Row(i).Height = 20; // Data rows
            }

            // Protect the worksheet
            wsData.Protection.IsProtected = true;
            wsData.Protection.SetPassword("fleet123");
            wsData.Protection.AllowSelectLockedCells = true;
            wsData.Protection.AllowSelectUnlockedCells = true;
            wsData.Cells[3, 2, 2 + parts.Count, 3].Style.Locked = false; // Unlock date and KM cells

            // Return as byte array
            return package.GetAsByteArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating template: {ex.Message}");
            throw;
        }
    }
}
