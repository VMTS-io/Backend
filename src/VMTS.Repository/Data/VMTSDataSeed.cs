using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data;

public static class VMTSDataSeed
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedAsync(VTMSDbContext dbContext)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            try
            {
                await SeedVehicleCategoriesAsync(dbContext);
                await SeedBrandsAsync(dbContext);
                await SeedVehicleModelsAsync(dbContext);
                await SeedVehiclesAsync(dbContext);
                await SeedPartsAsync(dbContext);
                await SeedMaintenanceRequestsAsync(dbContext);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding failed: {ex.Message}");
                throw;
            }
        });
    }

    private static async Task SeedVehicleCategoriesAsync(VTMSDbContext dbContext)
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
                Console.WriteLine("Seeded VehicleCategories");
            }
        }
    }

    private static async Task SeedBrandsAsync(VTMSDbContext dbContext)
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
                Console.WriteLine("Seeded Brands");
            }
        }
    }

    private static async Task SeedVehicleModelsAsync(VTMSDbContext dbContext)
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
                Console.WriteLine("Seeded VehicleModels");
            }
        }
    }

    private static async Task SeedVehiclesAsync(VTMSDbContext dbContext)
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
                Console.WriteLine("Seeded Vehicles");
            }
        }
    }

    private static async Task SeedPartsAsync(VTMSDbContext dbContext)
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
                Console.WriteLine("Seeded Parts");
            }
        }
    }

    private static async Task SeedMaintenanceRequestsAsync(VTMSDbContext dbContext)
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
                        Console.WriteLine(
                            $"Invalid VehicleId {request.VehicleId} for maintenance request {request.Description}"
                        );
                        continue;
                    }
                    if (!await dbContext.BusinessUsers.AnyAsync(u => u.Id == request.ManagerId))
                    {
                        Console.WriteLine(
                            $"Invalid ManagerId {request.ManagerId} for maintenance request {request.Description}"
                        );
                        continue;
                    }
                    if (!await dbContext.BusinessUsers.AnyAsync(u => u.Id == request.MechanicId))
                    {
                        Console.WriteLine(
                            $"Invalid MechanicId {request.MechanicId} for maintenance request {request.Description}"
                        );
                        continue;
                    }
                    if (
                        !await dbContext.MaintenanceCategories.AnyAsync(c =>
                            c.Id == request.MaintenanceCategoryId
                        )
                    )
                    {
                        Console.WriteLine(
                            $"Invalid MaintenanceCategoryId {request.MaintenanceCategoryId} for maintenance request {request.Description}"
                        );
                        continue;
                    }
                    await dbContext.AddAsync(request);
                    Console.WriteLine($"Seeded MaintenanceRequest: {request.Description}");
                }
            }
        }
    }
}
