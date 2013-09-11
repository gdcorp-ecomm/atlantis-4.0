namespace Atlantis.Framework.Domains.Interface
{
  public interface IDomain
  {
    string DomainName { get; }
    string PunyCodeDomainName { get; }
    string Sld { get; }
    string PunyCodeSld { get; }
    string Tld { get; }
    string PunyCodeTld { get; }
  }
}