using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Xml;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public sealed class DomainContactError : XmlDocument, IDomainContactError
  {
    public const string ERROR_ELEMENT_NAME = "error";
    private readonly XmlElement _errorElement;

    internal DomainContactError()
    {
      _errorElement = base.CreateElement(ERROR_ELEMENT_NAME);
      base.AppendChild(_errorElement);
    }
    
    internal DomainContactError(string sAttribute, int iCode, int minorCode, int contactType, string description = "", bool showError = false, bool removeDomain = false, string displayString = "", string dotType = "", string country = "", IEnumerable<string> domains = null )
      : this()
    {
      _errorElement.SetAttribute(DomainContactErrorAttributes.Attribute, sAttribute);
      _errorElement.SetAttribute(DomainContactErrorAttributes.Code, iCode.ToString(CultureInfo.InvariantCulture));
      _errorElement.SetAttribute(DomainContactErrorAttributes.MinorCode, minorCode.ToString(CultureInfo.InvariantCulture));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Description, description);
      _errorElement.SetAttribute(DomainContactErrorAttributes.ContactType, contactType.ToString(CultureInfo.InvariantCulture));
      _errorElement.SetAttribute(DomainContactErrorAttributes.ShowError, showError ? "1" : "0");
      _errorElement.SetAttribute(DomainContactErrorAttributes.RemoveDomain, removeDomain ? "1" : "0");
      _errorElement.SetAttribute(DomainContactErrorAttributes.DisplayString, displayString);
      _errorElement.SetAttribute(DomainContactErrorAttributes.DotType, dotType);
      _errorElement.SetAttribute(DomainContactErrorAttributes.Country, country);
      XmlElement domainsElement =
        (XmlElement)_errorElement.AppendChild(_errorElement.OwnerDocument.CreateElement(DomainContactErrorAttributes.Domains));
      if (domains != null)
      {
        foreach (var domain in domains)
        {
          ((XmlElement)domainsElement.AppendChild(_errorElement.OwnerDocument.CreateElement(DomainContactErrorAttributes.Domain))).SetAttribute(DomainContactErrorAttributes.DomainName, domain);
        } 
      }
    }

    internal DomainContactError(XmlElement errorXml)
      : this()
    {
      _errorElement.SetAttribute(DomainContactErrorAttributes.Attribute, errorXml.GetAttribute(DomainContactErrorAttributes.Attribute));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Code, errorXml.GetAttribute(DomainContactErrorAttributes.Code));
      _errorElement.SetAttribute(DomainContactErrorAttributes.MinorCode, errorXml.GetAttribute(DomainContactErrorAttributes.MinorCode));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Description, errorXml.GetAttribute(DomainContactErrorAttributes.Description));
      _errorElement.SetAttribute(DomainContactErrorAttributes.ContactType, errorXml.GetAttribute(DomainContactErrorAttributes.ContactType));
      _errorElement.SetAttribute(DomainContactErrorAttributes.ShowError, errorXml.GetAttribute(DomainContactErrorAttributes.ShowError));
      _errorElement.SetAttribute(DomainContactErrorAttributes.RemoveDomain, errorXml.GetAttribute(DomainContactErrorAttributes.RemoveDomain));
      _errorElement.SetAttribute(DomainContactErrorAttributes.DisplayString, errorXml.GetAttribute(DomainContactErrorAttributes.DisplayString));
      _errorElement.SetAttribute(DomainContactErrorAttributes.DotType, errorXml.GetAttribute(DomainContactErrorAttributes.DotType));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Country, errorXml.GetAttribute(DomainContactErrorAttributes.Country));
      XmlNodeList doamiansNodeList = errorXml.GetElementsByTagName(DomainContactErrorAttributes.Domains);
      if (doamiansNodeList != null && doamiansNodeList.Count > 0)
      {
        XmlNode node = _errorElement.OwnerDocument.ImportNode(doamiansNodeList[0], true);
        _errorElement.AppendChild(node);
	    }
      
    }

    #region Properties

    public string Attribute
    {
      get { return _errorElement.GetAttribute(DomainContactErrorAttributes.Attribute); }
    }

    public int Code
    {
      get
      {
        int result;
        Int32.TryParse(_errorElement.GetAttribute(DomainContactErrorAttributes.Code), out result);
        return result;
      }
    }

    public int MinorCode
    {
      get
      {
        int result;
        Int32.TryParse(_errorElement.GetAttribute(DomainContactErrorAttributes.MinorCode), out result);
        return result;
      }
    }

    public IEnumerable<string> Domains
    {
      get
      {
        return
          from ds in _errorElement.GetElementsByTagName(DomainContactErrorAttributes.Domains).OfType<XmlElement>()
          from d in ds.GetElementsByTagName(DomainContactErrorAttributes.Domain).OfType<XmlElement>()
          let n = d.Attributes[DomainContactErrorAttributes.DomainName]
          where n != null
          select n.Value;
      }
    }

    public string Description
    {
      get { return _errorElement.GetAttribute(DomainContactErrorAttributes.Description); }
    }

    public int ContactType
    {
      get 
      {
        int result;
        Int32.TryParse(_errorElement.GetAttribute(DomainContactErrorAttributes.ContactType), out result);
        return result;
      }
    }

    public string DisplayString
    {
      get { return _errorElement.GetAttribute(DomainContactErrorAttributes.DisplayString); }
    }

    public bool ShowError
    {
      get
      {
        return _errorElement.GetAttribute(DomainContactErrorAttributes.ShowError) == "1";
      }
    }

    public bool RemoveDomain
    {
      get
      {
        return _errorElement.GetAttribute(DomainContactErrorAttributes.RemoveDomain) == "1";
      }
    }

    
    public string DotType
    {
      get { return _errorElement.GetAttribute(DomainContactErrorAttributes.DotType); }
    }

    public string Country
    {
      get
      {
        return _errorElement.GetAttribute(DomainContactErrorAttributes.Country);
      }
    }

    #endregion

    #region ICloneable Members

    public override XmlNode Clone()
    {
      return new DomainContactError(Attribute, Code, MinorCode, ContactType, Description, ShowError, RemoveDomain, DisplayString, DotType, Country, Domains);
    }

    #endregion
  }
}
