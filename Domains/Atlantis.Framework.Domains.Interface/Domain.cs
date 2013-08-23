using System;

namespace Atlantis.Framework.Domains.Interface
{
  public class Domain : IDomain
  {
    public Domain(string sld, string tld, string name, string punnyCode)
    {
      _domainName = name ?? string.Empty;
      _punnyCodeDomainName = punnyCode ?? _domainName;
      _sld = sld ?? string.Empty;
      _tld = tld ?? string.Empty;
    }

    private readonly string _domainName;
    public string DomainName
    {
      get { return _domainName; }
    }

    private readonly string _punnyCodeDomainName;
    public string PunnyCodeDomainName
    {
      get { return _punnyCodeDomainName; }
    }

    private readonly string _sld;
    public string Sld
    {
      get { return _sld; }
    }

    private readonly string _tld;
    public string Tld
    {
      get { return _tld; }
    }

    public bool HasSubDomains
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public string HtmlFormat
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}