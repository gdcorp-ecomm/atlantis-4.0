using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes
{
  public class TLDProduct
  {
    const string PFIDATTRIBUTE = "pf_id";
    const string UNIFIEDPRODUCTIDATTRIBUTE = "catalog_productUnifiedProductID";
    const string MINQUANTITYATTRIBUTE = "minimum_quantity";
    const string DURATIONATTRIBUTE = "registrationPeriod";
    const string PRICETIERIDATTRIBUTE = "gdshop_registryPartnerPriceTierID";
    const string REGISTRYIDATTRIBUTE = "gdshop_domainRegistryID";
    
    internal static TLDProduct FromXElement(XElement productElement)
    {
      return new TLDProduct(productElement);
    }

    private int PfId { get; set; }
    public int UnifiedProductId { get; private set; }
    public int MinDomainCount { get; private set; }
    public int Years { get; private set; }
    public int? PriceTierId { get; private set; }
    public int? RegistryId { get; private set; }

    private TLDProduct(XElement productElement)
    {
      var durationAttributeValueInt = GetAttributeValueInt(productElement, DURATIONATTRIBUTE, 0);
      if (durationAttributeValueInt != null)
        Years = (int)durationAttributeValueInt;

      var minDomainAttributeValueInt = GetAttributeValueInt(productElement, MINQUANTITYATTRIBUTE, 0);
      if (minDomainAttributeValueInt != null)
        MinDomainCount = (int) minDomainAttributeValueInt;

      var pfIdAttributeValueInt = GetAttributeValueInt(productElement, PFIDATTRIBUTE, 0);
      if (pfIdAttributeValueInt != null)
        PfId = (int) pfIdAttributeValueInt;

      var unifiedProductIdAttributeValueInt = GetAttributeValueInt(productElement, UNIFIEDPRODUCTIDATTRIBUTE, 0);
      if (unifiedProductIdAttributeValueInt != null)
        UnifiedProductId = (int) unifiedProductIdAttributeValueInt;

      PriceTierId = GetAttributeValueInt(productElement, PRICETIERIDATTRIBUTE, null);
      RegistryId = GetAttributeValueInt(productElement, REGISTRYIDATTRIBUTE, null);
    }

    public bool IsValid
    {
      get
      {
        return ((Years > 0) && (MinDomainCount > 0) && (PfId > 0) && (UnifiedProductId > 0));
      }
    }

    private int? GetAttributeValueInt(XElement element, string attributeName, int? defaultValue)
    {
      int? result = defaultValue;

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
  }
}
