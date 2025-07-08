using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance;

public class MaintenanceRequestSpecification : BaseSpecification<MaintenaceRequest>
{
    public MaintenanceRequestSpecification()
    {
        Includes.Add(MR => MR.Vehicle);
        Includes.Add(MR => MR.Manager);
        Includes.Add(MR => MR.Mechanic);
        // Includes.Add(MR => MR.MaintenanceCategory);
    }

    public MaintenanceRequestSpecification(string id)
        : base(MR => MR.Id == id)
    {
        Includes.Add(MR => MR.Vehicle);
        Includes.Add(MR => MR.Manager);
        Includes.Add(MR => MR.Mechanic);
        Includes.Add(MR => MR.Parts);
        Includes.Add(MR => MR.Vehicle.VehicleModel.Category);
    }

    public MaintenanceRequestSpecification(MaintenanceRequestSpecParams specParams)
        : base(MR =>
            (string.IsNullOrEmpty(specParams.VehicleId) || MR.VehicleId == specParams.VehicleId)
            && (!specParams.Status.HasValue || MR.Status == specParams.Status)
            && (!specParams.Date.HasValue || MR.Date == specParams.Date)
            && (
                string.IsNullOrEmpty(specParams.MechanicId)
                || MR.MechanicId == specParams.MechanicId
            )
        )
    {
        Includes.Add(MR => MR.Manager);
        Includes.Add(MR => MR.Mechanic);
        Includes.Add(MR => MR.Parts);
        Includes.Add(MR => MR.InitialReport);
        Includes.Add(MR => MR.FinalReport);
        Includes.Add(MR => MR.Vehicle.VehicleModel.Category);
        if (!string.IsNullOrEmpty(specParams.OrderBy))
        {
            switch (specParams.OrderBy)
            {
                case "Date":
                    AddOrderBy(MR => MR.Date);
                    break;
                case "DateDesc":
                    AddOrderByDesc(O => O.Date);
                    break;
                case "Status":
                    AddOrderBy(MR => MR.Status);
                    break;
                case "StatusDesc":
                    AddOrderByDesc(MR => MR.Status);
                    break;
            }
        }
        else
            AddOrderByDesc(MR => MR.Date);
    }

    public MaintenanceRequestSpecification(MaintenanceRequestSpecParamsForMechanic specParams)
        : base(MR =>
            (string.IsNullOrEmpty(specParams.VehicleId) || MR.VehicleId == specParams.VehicleId)
            && (!specParams.Status.HasValue || MR.Status == specParams.Status)
            && (!specParams.Date.HasValue || MR.Date == specParams.Date)
        )
    {
        Includes.Add(MR => MR.Vehicle);
        Includes.Add(MR => MR.Manager);
        Includes.Add(MR => MR.Mechanic);
        Includes.Add(MR => MR.Parts);
        Includes.Add(MR => MR.InitialReport);
        Includes.Add(MR => MR.Vehicle.VehicleModel.Category);
        if (!string.IsNullOrEmpty(specParams.OrderBy))
        {
            switch (specParams.OrderBy)
            {
                case "Date":
                    AddOrderBy(MR => MR.Date);
                    break;
                case "DateDesc":
                    AddOrderByDesc(O => O.Date);
                    break;
                case "Status":
                    AddOrderBy(MR => MR.Status);
                    break;
                case "StatusDesc":
                    AddOrderByDesc(MR => MR.Status);
                    break;
            }
        }
        else
            AddOrderByDesc(MR => MR.Date);
    }
}
