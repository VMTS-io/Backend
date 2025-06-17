using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.TripRequestSpecification;

public class TripRequestIncludesSpecification : BaseSpecification<TripRequest>
{
        public TripRequestIncludesSpecification(string driverId) 
            : base(t => 
                t.Driver != null && t.DriverId == driverId &&
                t.Status == TripStatus.Approved 
                )
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(t => t.Vehicle);
            Includes.Add(t => t.Driver);  
        }
    }

 