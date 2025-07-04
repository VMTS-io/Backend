using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data;

public class VMTSDataSeed
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedAsync(VTMSDbContext dbContext, ILogger<VMTSDataSeed> logger)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            try
            {
                await SeedVehicleCategoriesAsync(dbContext, logger);
                await SeedBrandsAsync(dbContext, logger);
                await SeedVehicleModelsAsync(dbContext, logger);
                await SeedVehiclesAsync(dbContext, logger);
                await SeedPartsAsync(dbContext, logger);
                await SeedMaintenanceCategoriesAsync(dbContext, logger);
                await dbContext.SaveChangesAsync();
                // await SeedMaintenanceRequestsAsync(dbContext, logger);
                // await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Seeding failed {message}", ex.Message);
                throw;
            }
        });
    }

    private static async Task SeedVehicleCategoriesAsync(
        VTMSDbContext dbContext,
        ILogger<VMTSDataSeed> logger
    )
    {
        if (!await dbContext.Set<VehicleCategory>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/vehicle_categories.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var categories = JsonSerializer.Deserialize<List<VehicleCategory>>(
                jsonText,
                _jsonOptions
            );
            if (categories != null && categories.Count > 0)
            {
                await dbContext.AddRangeAsync(categories);
                logger.LogInformation("Seeded {Count} vehicle categories", categories.Count);
            }
        }
    }

    private static async Task SeedBrandsAsync(VTMSDbContext dbContext, ILogger<VMTSDataSeed> logger)
    {
        if (!await dbContext.Set<Brand>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/brands.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var brands = JsonSerializer.Deserialize<List<Brand>>(jsonText, _jsonOptions);
            if (brands != null && brands.Count > 0)
            {
                await dbContext.AddRangeAsync(brands);
                logger.LogInformation("Seeded {Count} brands", brands.Count);
            }
        }
    }

    private static async Task SeedVehicleModelsAsync(
        VTMSDbContext dbContext,
        ILogger<VMTSDataSeed> logger
    )
    {
        if (!await dbContext.Set<VehicleModel>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/vehicle_models.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var models = JsonSerializer.Deserialize<List<VehicleModel>>(jsonText, _jsonOptions);
            if (models != null && models.Count > 0)
            {
                await dbContext.AddRangeAsync(models);
                logger.LogInformation("Seeded {Count} vehicle models", models.Count);
            }
        }
    }

    private static async Task SeedVehiclesAsync(
        VTMSDbContext dbContext,
        ILogger<VMTSDataSeed> logger
    )
    {
        if (!await dbContext.Set<Vehicle>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/vehicles.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);

            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(jsonText, _jsonOptions);
            if (vehicles != null && vehicles.Count > 0)
            {
                await dbContext.AddRangeAsync(vehicles);
                logger.LogInformation("Seeded {Count} vehicles", vehicles.Count);
            }
        }
    }

    private static async Task SeedPartsAsync(VTMSDbContext dbContext, ILogger<VMTSDataSeed> logger)
    {
        if (!await dbContext.Set<Part>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/parts.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var parts = JsonSerializer.Deserialize<List<Part>>(jsonText, _jsonOptions);
            if (parts != null && parts.Count > 0)
            {
                await dbContext.AddRangeAsync(parts);
                logger.LogInformation("Seeded {Count} parts", parts.Count);
            }
        }
    }

    private static async Task SeedMaintenanceCategoriesAsync(
        VTMSDbContext dbContext,
        ILogger<VMTSDataSeed> logger
    )
    {
        if (!await dbContext.Set<MaintenaceCategories>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/maintenance-categories.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var maintenanceCategories = JsonSerializer.Deserialize<List<MaintenaceCategories>>(
                jsonText,
                _jsonOptions
            );
            if (maintenanceCategories != null && maintenanceCategories.Count > 0)
            {
                await dbContext.AddRangeAsync(maintenanceCategories);
                logger.LogInformation(
                    "Seeded {Count} maintenance categories",
                    maintenanceCategories.Count
                );
            }
        }
    }

    private static async Task SeedMaintenanceRequestsAsync(
        VTMSDbContext dbContext,
        ILogger<VMTSDataSeed> logger
    )
    {
        if (!await dbContext.Set<MaintenaceRequest>().AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                "Data/DataSeed/maintenance_requests.json"
            );
            var jsonText = await File.ReadAllTextAsync(filePath);
            var maintenanceRequests = JsonSerializer.Deserialize<List<MaintenaceRequest>>(
                jsonText,
                _jsonOptions
            );
            if (maintenanceRequests != null && maintenanceRequests.Count > 0)
            {
                foreach (var request in maintenanceRequests)
                {
                    // Validate foreign keys
                    if (!await dbContext.Vehicles.AnyAsync(v => v.Id == request.VehicleId))
                    {
                        logger.LogWarning(
                            "Invalid VehicleId {VehicleId} for maintenance request {Description}",
                            request.VehicleId,
                            request.Description
                        );
                        continue;
                    }
                    if (!await dbContext.BusinessUsers.AnyAsync(u => u.Id == request.ManagerId))
                    {
                        logger.LogWarning(
                            "Invalid ManagerId {ManagerId} for maintenance request {Description}",
                            request.ManagerId,
                            request.Description
                        );
                        continue;
                    }
                    if (!await dbContext.BusinessUsers.AnyAsync(u => u.Id == request.MechanicId))
                    {
                        logger.LogWarning(
                            "Invalid MechanicId {MechanicId} for maintenance request {Description}",
                            request.MechanicId,
                            request.Description
                        );
                        continue;
                    }
                    // if (
                    //     !await dbContext.MaintenanceCategories.AnyAsync(c =>
                    //         c.Id == request.MaintenanceCategoryId
                    //
                    //     )
                    // )
                    // {
                    //     logger.LogWarning(
                    //         "Invalid MaintenanceCategoryId {MaintenanceCategoryId} for maintenance request {Description}",
                    //         request.MaintenanceCategoryId,
                    //         request.Description
                    //     );
                    //     continue;
                    // }
                    await dbContext.AddAsync(request);
                    logger.LogInformation(
                        "Seeded MaintenanceRequest: {Description}",
                        request.Description
                    );
                }
            }
        }
    }
}
