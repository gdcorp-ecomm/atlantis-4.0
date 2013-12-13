namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeContact
  {
    public DomainsTrusteeContactTypes ContactType { get; set; }
    public string CountryCode { get; set; }

    public DomainsTrusteeContact(DomainsTrusteeContactTypes contactType, string countryCode)
    {
      ContactType = contactType;
      CountryCode = countryCode;
    }
  }
}
