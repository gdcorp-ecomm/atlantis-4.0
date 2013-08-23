namespace Atlantis.Framework.Domains.Interface
{
  public interface IDomain
  {
    string DomainName { get; }
    string PunnyCodeDomainName { get; }
    string Sld { get; }
    string Tld { get; }
    bool HasSubDomains { get; }
    string HtmlFormat { get; }
  }
}