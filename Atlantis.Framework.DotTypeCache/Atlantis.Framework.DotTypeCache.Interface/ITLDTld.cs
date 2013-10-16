namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDTld
  {
    int RenewProhibitedPeriodForExpiration { get; }

    string RenewProhibitedPeriodForExpirationUnit { get; }

    bool IsGtld { get; }
  }
}
