namespace Atlantis.Framework.Domains.Interface
{
  public interface IDomain
  {
    string DomainName { get; }
    string PunnyCodeDomainName { get; }
    string Sld { get; }
    string PunnyCodeSld { get; }
    string Tld { get; }
    string PunnyCodeTld { get; }
  }
}