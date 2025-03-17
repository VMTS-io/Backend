using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data;

public class VTMSDbContext : DbContext
{
    #region Vehicle
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleModel> Models { get; set; }
    public DbSet<VehicleCategory> Categories { get; set; }
    #endregion

    #region parts

    public DbSet<Part> Parts { get; set; }
    public DbSet<PartCategory> PartCategories { get; set; }

    #endregion

    #region Maintenance

    public DbSet<MaintenaceCategory> MaintenanceCategories { get; set; }
    public DbSet<MaintenaceReport> MaintenanceReports { get; set; }
    public DbSet<MaintenaceTracking> MaintenanceTrackings { get; set; }
    public DbSet<MaintenaceRequest> MaintenanceRequests { get; set; }

    #endregion

    #region Trips

    public DbSet<TripReport> TripsReports { get; set; }
    public DbSet<TripRequest> TripsRequests { get; set; }

    #endregion

    #region Fault Reports

    public DbSet<FaultReport> FaultReports { get; set; }

    #endregion

    #region Business User

    public DbSet<BusinessUser> BusinessUsers { get; set; }

    #endregion

    public VTMSDbContext(DbContextOptions<VTMSDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Ignore<CustomAttributeData>(); // Ignore this type
        modelBuilder.Ignore<Type>(); // Exclude System.Type from EF Core
    }
}
