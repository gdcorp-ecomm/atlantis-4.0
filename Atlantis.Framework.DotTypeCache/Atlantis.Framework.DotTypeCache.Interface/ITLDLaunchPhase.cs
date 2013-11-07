
namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDLaunchPhase
  {
    string Code { get; }

    string Type { get; }

    string Value { get; }

    ITLDLaunchPhasePeriod PreRegistrationPeriod { get; }

    ITLDLaunchPhasePeriod LivePeriod { get; }

    bool NeedsClaimCheck { get; }

    bool AppFeeRefundsEnabled { get; }
  }
}
