namespace Atlantis.Framework.Providers.DomainLookup.Interface
{
    public interface IDomainLookupProvider
    {
      string DomainName { get; }
      IDomainLookupData ParkedDomainInfo { get; }
      IDomainLookupData GetDomainInformation(string domainName);
      bool IsDomainExpired();
      bool IsDomainAdult();
      bool IsDomainWithin90DaysOfExpiration();
    }
}
