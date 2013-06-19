using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  public class DotTypeProduct
  {
    const string _DURATIONATTRIBUTE = "duration";
    const string _QUANTITYATTRIBUTE = "quantity";
    const string _TYPEATTRIBUTE = "type";
    const string _IDATTRIBUTE = "id";
    const string _REGISTRATIONAPIID = "registrationapiid";

    internal static DotTypeProduct FromXElement(XElement productElement)
    {
      return new DotTypeProduct(productElement);
    }

    public int Years { get; private set; }
    public int MinDomainCount { get; private set; }
    public int ProductId { get; private set; }
    public DotTypeProductTypes ProductType { get; private set; }
    public string RegistryId { get; private set; }

    private DotTypeProduct(XElement productElement)
    {
      Years = GetAttributeValueInt(productElement, _DURATIONATTRIBUTE, 0);
      MinDomainCount = GetAttributeValueInt(productElement, _QUANTITYATTRIBUTE, 0);
      ProductId = GetAttributeValueInt(productElement, _IDATTRIBUTE, 0);
      RegistryId = GetAttributeValueString(productElement, _REGISTRATIONAPIID, string.Empty);
      
      string type = GetAttributeValueString(productElement, _TYPEATTRIBUTE, string.Empty);

      ProductType = EnumHelper.GetValueFromDescription<DotTypeProductTypes>(type);
    }

    public bool IsValid
    {
      get
      {
        return (ProductType != DotTypeProductTypes.None) && (Years > 0) && (MinDomainCount > 0) && (ProductId > 0);
      }
    }

    private int GetAttributeValueInt(XElement element, string attributeName, int defaultValue)
    {
      int result = defaultValue;

      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        int temp;
        if (int.TryParse(attribute.Value, out temp))
        {
          result = temp;
        }
      }

      return result;
    }

    private string GetAttributeValueString(XElement element, string attributeName, string defaultValue)
    {
      string result = defaultValue;

      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }

      return result;
    }

  }
}
