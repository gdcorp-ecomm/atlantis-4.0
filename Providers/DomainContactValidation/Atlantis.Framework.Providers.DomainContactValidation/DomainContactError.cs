using System;
using System.Collections;
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
      _errorElement = CreateElement(ERROR_ELEMENT_NAME);
      AppendChild(_errorElement);
    }
    
    internal DomainContactError(string sAttribute, int iCode, int contactType, string sDescription = "", string displayString = "", string dotType = "")
      : this()
    {
      _errorElement.SetAttribute(DomainContactErrorAttributes.Attribute, sAttribute);
      _errorElement.SetAttribute(DomainContactErrorAttributes.Code, iCode.ToString(CultureInfo.InvariantCulture));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Description, sDescription);
      _errorElement.SetAttribute(DomainContactErrorAttributes.ContactType, contactType.ToString(CultureInfo.InvariantCulture));
      _errorElement.SetAttribute(DomainContactErrorAttributes.DisplayString, displayString);
      _errorElement.SetAttribute(DomainContactErrorAttributes.DotType, dotType);
    }

    internal DomainContactError(XmlElement errorXml)
      : this()
    {
      _errorElement.SetAttribute(DomainContactErrorAttributes.Attribute, errorXml.GetAttribute(DomainContactErrorAttributes.Attribute));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Code, errorXml.GetAttribute(DomainContactErrorAttributes.Code));
      _errorElement.SetAttribute(DomainContactErrorAttributes.Description, errorXml.GetAttribute(DomainContactErrorAttributes.Description));
      _errorElement.SetAttribute(DomainContactErrorAttributes.ContactType, errorXml.GetAttribute(DomainContactErrorAttributes.ContactType));
      _errorElement.SetAttribute(DomainContactErrorAttributes.DisplayString, errorXml.GetAttribute(DomainContactErrorAttributes.DisplayString));
      _errorElement.SetAttribute(DomainContactErrorAttributes.DotType, errorXml.GetAttribute(DomainContactErrorAttributes.DotType));
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
          from d in _errorElement.GetElementsByTagName(DomainContactErrorAttributes.Domains).OfType<XmlElement>()
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
      var result = new DomainContactError(Attribute, Code, ContactType, Description, DisplayString, DotType);
      return result;
    }

    #endregion
  }
}
