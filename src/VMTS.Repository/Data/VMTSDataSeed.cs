using System.Text.Json;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data;

public static class VMTSDataSeed
{
    public static async Task SeedAsync(VTMSDbContext dbContext)
    {
        if (!dbContext.Set<Vehicle>().Any())
        {
            var vehicleText = File.ReadAllText("../VMTS.Repository/Data/DataSeed/Vehicle.json");
            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(vehicleText);
            if (vehicles!.Count > 0)
                await dbContext.AddRangeAsync(vehicles);
            dbContext.SaveChanges();
        }
    }
}
