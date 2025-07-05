using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.RecurringTripTemplateIncludesSpecification;

public class RecurringTripTemplateIncludesSpecification : BaseSpecification<RecurringTripTemplate>
{
    public RecurringTripTemplateIncludesSpecification()
        : base(x => x.IsActive)
    {
        Includes.Add(rt => rt.Driver);
        Includes.Add(rt => rt.Vehicle);
    }
}
