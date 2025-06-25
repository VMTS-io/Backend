using VMTS.Core.Entities.User_Business;

namespace VMTS.Core.Specifications;

public class BusinessUserSpecification : BaseSpecification<BusinessUser>
{
    public BusinessUserSpecification(string id)
        : base(u => u.Id == id)
    {
        Includes.Add(bs => bs.DriverTripRequest);
    }

    public BusinessUserSpecification(BusinessUserSpecParams specParams)
        : base(u =>
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
        )
    {
        ApplySorting(specParams);
        ApplyPagination(specParams);

        if (!string.IsNullOrEmpty(specParams.Role))
        {
            switch (specParams.Role.ToLower())
            {
                case "driver":
                    ApplyDriverIncludes();
                    break;
                case "mechanic":
                    ApplyMechanicIncludes();
                    break;
            }
        }
    }

    private void ApplyMechanicIncludes()
    {
        Includes.Add(bs => bs.MechanicMaintenaceReports);
        Includes.Add(bs => bs.MechanicMaintenaceRequests);
    }

    private void ApplyDriverIncludes()
    {
        Includes.Add(bs => bs.DriverTripRequest);
        Includes.Add(bs => bs.DriverFaultReport);
        Includes.Add(bs => bs.DriverTripReport);
    }

    private void ApplyManagerIncludes()
    {
        Includes.Add(bs => bs.ManagerMaintenaceRequests);
        Includes.Add(bs => bs.ManagerTripRequest);
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
            case "DisplayNameAsc":
                AddOrderBy(u => u.DisplayName);
                break;
            case "DisplayNameDsc":
                AddOrderByDesc(u => u.DisplayName);
                break;
        }
    }

    private void ApplyPagination(BusinessUserSpecParams spec)
    {
        AddPaginaiton(Math.Max(0, spec.PageIndex - 1), spec.PageSize);
    }
}
