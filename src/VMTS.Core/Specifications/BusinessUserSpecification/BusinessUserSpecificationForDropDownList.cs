using System.Linq.Expressions;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;

namespace VMTS.Core.Specifications;

public class BusinessUserSpecificationForDropDownList : BaseSpecification<BusinessUser>
{
    public BusinessUserSpecificationForDropDownList(string id)
        : base(u => u.Id == id)
    {
        Includes.Add(bs => bs.DriverTripRequest);
    }

    public BusinessUserSpecificationForDropDownList(BusinessUserSpecParams specParams)
        : base(BuildCriteria(specParams))
    {
        ApplySorting(specParams);
        ApplyPagination(specParams);
        // ApplyIncludes(specParams);
    }

    private static Expression<Func<BusinessUser, bool>> BuildCriteria(
        BusinessUserSpecParams specParams
    )
    {
        return u =>
            (string.IsNullOrEmpty(specParams.Role) || u.Role == specParams.Role)
            && (string.IsNullOrEmpty(specParams.Email) || u.Email.Contains(specParams.Email))
            && (
                string.IsNullOrEmpty(specParams.PhoneNumber)
                || u.PhoneNumber.Contains(specParams.PhoneNumber)
            )
            && (
                string.IsNullOrEmpty(specParams.DisplayName)
                || u.DisplayName.Contains(specParams.DisplayName)
            )
            // 👇 Driver Filter
            && (
                !string.IsNullOrEmpty(specParams.Filter) && specParams.Filter != "FreeDrivers"
                || !u.DriverTripRequest.Any(tr =>
                    (
                        !specParams.TripDate.HasValue
                        || tr.Date.Date == specParams.TripDate.Value.Date
                    ) && (tr.Status == TripStatus.Approved || tr.Status == TripStatus.Pending)
                )
            )
            // 👇 Mechanic Filter
            && (
                (!string.IsNullOrEmpty(specParams.Filter) && specParams.Filter != "FreeMechanics")
                || u.MechanicMaintenaceRequests.Count(mr =>
                    mr.Status != MaintenanceStatus.Completed
                ) <= 5
            );
    }

    private void ApplyIncludes(BusinessUserSpecParams specParams)
    {
        if (!string.IsNullOrEmpty(specParams.Role))
        {
            switch (specParams.Role.ToLower())
            {
                case "driver":
                    Includes.Add(bs => bs.DriverTripRequest);
                    break;

                // case "mechanic":
                //     Includes.Add(bs =>
                //         bs.MechanicMaintenaceRequests
                //     // .Where(mr => mr.Status != Status.Completed)
                //     );
                //     break;

                case "manager":
                    Includes.Add(bs => bs.ManagerTripRequest);
                    Includes.Add(bs => bs.ManagerMaintenaceRequests);
                    break;
            }
        }
    }

    private void ApplySorting(BusinessUserSpecParams specParams)
    {
        if (string.IsNullOrEmpty(specParams.Sort))
        {
            AddOrderBy(u => u.DisplayName);
            return;
        }

        switch (specParams.Sort?.ToLower())
        {
            case "driverloadasc":
                AddOrderBy(u =>
                    u.DriverTripRequest.Count(tr =>
                        tr.Status == TripStatus.Pending || tr.Status == TripStatus.Approved
                    )
                );
                break;

            case "driverloaddsc":
                AddOrderByDesc(u =>
                    u.DriverTripRequest.Count(tr =>
                        tr.Status == TripStatus.Pending || tr.Status == TripStatus.Approved
                    )
                );
                break;

            case "mechanicloadasc":
                AddOrderBy(u =>
                    u.MechanicMaintenaceRequests.Count(mr =>
                        mr.Status != MaintenanceStatus.Completed
                    )
                );
                break;

            case "mechanicloaddsc":
                AddOrderByDesc(u =>
                    u.MechanicMaintenaceRequests.Count(mr =>
                        mr.Status != MaintenanceStatus.Completed
                    )
                );
                break;

            case "displaynameasc":
                AddOrderBy(u => u.DisplayName);
                break;

            case "displaynamedsc":
                AddOrderByDesc(u => u.DisplayName);
                break;

            default:
                AddOrderBy(u => u.DisplayName);
                break;
        }
    }

    private void ApplyPagination(BusinessUserSpecParams specParams)
    {
        AddPaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }
}
