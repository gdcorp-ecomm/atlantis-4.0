namespace Atlantis.Framework.Providers.DomainLookup.Interface
{
    public interface IDomainLookupProvider
    {
      IDomainLookupData GetDomainInformation(string domainName);
    }
}
