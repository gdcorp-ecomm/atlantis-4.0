using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class UnifiedProductIdResponseData : IResponseData
  {
    public static UnifiedProductIdResponseData NotFound { get; private set; }

    static UnifiedProductIdResponseData()
    {
      NotFound = new UnifiedProductIdResponseData(int.MinValue);
    }

    public int UnifiedProductId { get; set; }

    public static UnifiedProductIdResponseData FromCacheData(string cacheDataXml)
    {
      UnifiedProductIdResponseData result = NotFound;

      try
      {
        XElement dataElement = XElement.Parse(cacheDataXml);
        string productIdString = (from item in dataElement.Descendants("item")
                                  select item.Attribute("gdshop_product_unifiedProductID").Value).FirstOrDefault();

        int unifiedProductId;
        if (int.TryParse(productIdString, out unifiedProductId))
        {
          result = new UnifiedProductIdResponseData(unifiedProductId);
        }
      }
      catch(Exception ex)
      {
        AtlantisException aex = new AtlantisException("UnifiedProductIdResponseData.FromeCacheData", 0, ex.Message + ex.StackTrace, cacheDataXml);
      }

      return result;
    }

    private UnifiedProductIdResponseData(int unifiedProductId)
    {
      UnifiedProductId = unifiedProductId;
    }

    public string ToXML()
    {
      XElement element = new XElement("UnifiedProductIdResponseData");
      element.Add(new XAttribute("unifiedproductid", UnifiedProductId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
