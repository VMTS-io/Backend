using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data;

public static class VMTSDataSeed
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true, // Optional: allows case-insensitive JSON property matching
    };

    // public static async Task SeedAsync(VTMSDbContext dbContext)
    // {
    //     if (!dbContext.Set<Vehicle>().Any())
    //     {
    //         var vehicleText = File.ReadAllText("../VMTS.Repository/Data/DataSeed/vehicles.json");
    //         var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(vehicleText);
    //         if (vehicles!.Count > 0)
    //             await dbContext.AddRangeAsync(vehicles);
    //         dbContext.SaveChanges();
    //     }
    // }
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
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error (ILogger can be injected if needed)
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
}
