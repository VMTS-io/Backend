using OfficeOpenXml;
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

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet == null)
        {
            errors.Add("No worksheet found in Excel file.");
            return (results, errors);
        }

        var expectedHeaders = new[] { "PartId", "LastChangedDate", "KMAtLastChange" };
        for (int i = 0; i < expectedHeaders.Length; i++)
        {
            var header = worksheet.Cells[1, i + 1].Text?.Trim();
            if (!string.Equals(header, expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(
                    $"Expected column '{expectedHeaders[i]}' at position {i + 1}, but got '{header}'"
                );
                return (results, errors);
            }
        }

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            try
            {
                var partId = worksheet.Cells[row, 1].Text?.Trim();
                var dateStr = worksheet.Cells[row, 2].Text?.Trim();
                var kmStr = worksheet.Cells[row, 3].Text?.Trim();

                if (
                    string.IsNullOrWhiteSpace(partId)
                    || string.IsNullOrWhiteSpace(dateStr)
                    || string.IsNullOrWhiteSpace(kmStr)
                )
                {
                    errors.Add($"Row {row - 1}: One or more fields are empty.");
                    continue;
                }

                if (!await partServices.IsExist(partId))
                {
                    errors.Add($"not part with id {partId}");
                    continue;
                }

                if (!DateTime.TryParse(dateStr, out var lastChangedDate))
                {
                    errors.Add($"Row {row - 1}: Invalid date format in 'LastChangedDate'.");
                    continue;
                }

                if (!int.TryParse(kmStr, out var kmAtLastChange))
                {
                    errors.Add($"Row {row - 1}: Invalid number in 'KMAtLastChange'.");
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
}
