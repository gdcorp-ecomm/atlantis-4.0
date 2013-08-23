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
          _punnyCodeSld = sld;
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
          _punnyCodeSld = IdnConvertor.GetAscii(sld);
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
          _punnyCodeTld = tld;
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
          _punnyCodeTld = IdnConvertor.GetAscii(tld); 
        }
        catch (Exception)
        {

        }
      }
    }

    public Domain(string sld, string tld, string punnyCodeSld, string punnyCodeTld)
    {
      _sld = sld ?? string.Empty;
      _tld = tld ?? string.Empty;
      _punnyCodeSld = punnyCodeSld ?? string.Empty;
      _punnyCodeTld = punnyCodeTld ?? string.Empty;
    }

    public string DomainName
    {
      get
      {
        if (string.IsNullOrEmpty(_sld) && string.IsNullOrEmpty(_tld))
        {
          return string.Empty;
        }

        return string.Concat(_sld, ".", _tld);
      }
    }

    public string PunnyCodeDomainName
    {
      get
      {
        if (string.IsNullOrEmpty(_punnyCodeSld) && string.IsNullOrEmpty(_punnyCodeTld))
        {
          return string.Empty;
        }

        return string.Concat(_punnyCodeSld, ".", _punnyCodeTld);
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

    private readonly string _punnyCodeSld = string.Empty;
    public string PunnyCodeSld
    {
      get { return _punnyCodeSld; }
    }

    private readonly string _punnyCodeTld = string.Empty;
    public string PunnyCodeTld
    {
      get { return _punnyCodeTld; }
    }

    private IdnMapping _idnmapping;
    private IdnMapping IdnConvertor
    {
      get { return _idnmapping ?? (_idnmapping = new IdnMapping()); }
    }
  }
}