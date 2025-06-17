using VMTS.Core.Entities.User_Business;

namespace VMTS.Core.Specifications;

public class BusinessUserSpecification : BaseSpecification<BusinessUser>
{
    public BusinessUserSpecification(string userId): 
        base(bs => bs.Id == userId)
    {
        Includes.Add(bs => bs.DriverTripRequest);
    }
}