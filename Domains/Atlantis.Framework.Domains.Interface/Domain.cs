using System;
using System.Globalization;

namespace Atlantis.Framework.Domains.Interface
{
  public class Domain : IDomain
  {
    public Domain(string sld, string tld)
    {
      if (string.IsNullOrEmpty(sld) || string.IsNullOrEmpty(tld)) return;

      if (sld.StartsWith("xn--", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          _sld = IdnConvertor.GetUnicode(sld);
          _punyCodeSld = sld;
        }
        catch (Exception)
        {

        }
      }
      else
      {
        try
        {
          _sld = sld;
          _punyCodeSld = IdnConvertor.GetAscii(sld);
        }
        catch (Exception)
        {

        }
      }

      if (tld.StartsWith("xn--", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          _tld = IdnConvertor.GetUnicode(tld);
          _punyCodeTld = tld;
        }
        catch (Exception)
        {

        }
      }
      else
      {
        try
        {
          _tld = tld;
          _punyCodeTld = IdnConvertor.GetAscii(tld); 
        }
        catch (Exception)
        {

        }
      }
    }

    public Domain(string sld, string tld, string punyCodeSld, string punyCodeTld)
    {
      _sld = sld ?? string.Empty;
      _tld = tld ?? string.Empty;
      _punyCodeSld = punyCodeSld ?? string.Empty;
      _punyCodeTld = punyCodeTld ?? string.Empty;
    }

    private string _domainName;
    public string DomainName
    {
      get
      {
        if (_domainName == null)
        {
          if (string.IsNullOrEmpty(_sld) || string.IsNullOrEmpty(_tld))
          {
            _domainName =  string.Empty;
          }
          else
          {
            _domainName = string.Concat(_sld, ".", _tld);
          }
        }

        return _domainName;
      }
    }

    private string _punyCodeDomainName;
    public string PunyCodeDomainName
    {
      get
      {
        if (_punyCodeDomainName == null)
        {
          if (string.IsNullOrEmpty(_punyCodeSld) || string.IsNullOrEmpty(_punyCodeTld))
          {
            _punyCodeDomainName = string.Empty;
          }
          else
          {
            _punyCodeDomainName = string.Concat(_punyCodeSld, ".", _punyCodeTld);
          }
        }

        return _punyCodeDomainName;
      }
    }

    private readonly string _sld = string.Empty;
    public string Sld
    {
      get { return _sld; }
    }

    private readonly string _tld = string.Empty;
    public string Tld
    {
      get { return _tld; }
    }

    private readonly string _punyCodeSld = string.Empty;
    public string PunyCodeSld
    {
      get { return _punyCodeSld; }
    }

    private readonly string _punyCodeTld = string.Empty;
    public string PunyCodeTld
    {
      get { return _punyCodeTld; }
    }

    private IdnMapping _idnmapping;
    private IdnMapping IdnConvertor
    {
      get { return _idnmapping ?? (_idnmapping = new IdnMapping()); }
    }
  }
}