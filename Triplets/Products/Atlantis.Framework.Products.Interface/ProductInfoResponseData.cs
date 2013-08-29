using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductInfoResponseData : IResponseData, IProductInfo
  {
    public static ProductInfoResponseData None { get; private set; }

    static ProductInfoResponseData()
    {
      None = new ProductInfoResponseData(string.Empty, string.Empty, 1, 0, RecurringPaymentUnitType.Unknown);
    }

    // "<item pf_id=\"101\" description2=\".COM is the bomb!\" name=\".COM Domain Name Registration - 1 Year\" 
    //     gdshop_product_typeID=\"2\" numberOfPeriods=\"1\" recurring_payment=\"annual\"/>";
    public static ProductInfoResponseData FromCacheData(string cacheDataXml)
    {
      ProductInfoResponseData result = None;

      try
      {
        XElement itemElement = XElement.Parse(cacheDataXml);
        if (itemElement.Name.LocalName != "item")
        {
          itemElement = itemElement.Descendants("item").FirstOrDefault();
        }

        string pfid = itemElement.GetAttributeValueString("pf_id", "unknown");
        string name = itemElement.GetAttributeValueString("name", pfid);
        string friendlyName = itemElement.GetAttributeValueString("description2", name);
        int productTypeId = itemElement.GetAttributeValueInt("gdshop_product_typeID", -1);
        int numberOfPeriods = itemElement.GetAttributeValueInt("numberOfPeriods", 0);

        string recurringPaymentText = itemElement.GetAttributeValueString("recurring_payment", null);
        RecurringPaymentUnitType recurringPayment = ParseRecurringPayment(recurringPaymentText);

        result = new ProductInfoResponseData(name, friendlyName, numberOfPeriods, productTypeId, recurringPayment);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("ProductInfoResponseData.FromeCacheData", 0, ex.Message + ex.StackTrace, cacheDataXml);
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }

    private ProductInfoResponseData(string name, string friendlyDescription, int numberOfPeriods, int productTypeId, RecurringPaymentUnitType recurringPayment)
    {
      Name = name;
      FriendlyDescription = friendlyDescription;
      NumberOfPeriods = numberOfPeriods;
      ProductTypeId = productTypeId;
      RecurringPayment = recurringPayment;
    }

    public string ToXML()
    {
      var element = new XElement("ProductInfoResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public string FriendlyDescription { get; private set; }
    public string Name { get; private set; }
    public int NumberOfPeriods { get; private set; }
    public int ProductTypeId { get; private set; }
    public RecurringPaymentUnitType RecurringPayment { get; private set; }

    private static RecurringPaymentUnitType ParseRecurringPayment(string recurringPaymentText)
    {
      var result = RecurringPaymentUnitType.Unknown;

      if (!string.IsNullOrEmpty(recurringPaymentText))
      {
        switch (recurringPaymentText.ToLowerInvariant())
        {
          case "monthly":
            result = RecurringPaymentUnitType.Monthly;
            break;
          case "annual":
            result = RecurringPaymentUnitType.Annual;
            break;
          case "semiannual":
            result = RecurringPaymentUnitType.SemiAnnual;
            break;
          case "quarterly":
            result = RecurringPaymentUnitType.Quarterly;
            break;
        }
      }

      return result;
    }
  }
}
