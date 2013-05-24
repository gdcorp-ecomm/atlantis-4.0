using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupsOfferedResponseData : IResponseData
  {
    private HashSet<int> _productGroupsOffered;

    public string ToXML()
    {
      XElement result = new XElement("ProductGroupsOffered");

      foreach (int groupId in _productGroupsOffered)
      {
        result.Add(new XElement("id", groupId.ToString()));
      }

      return result.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    private ProductGroupsOfferedResponseData(string cacheDataXml)
    {
      _productGroupsOffered = new HashSet<int>();

      // "<data><item pl_productGroupID=\"1\" description=\"Web Hosting\"/></data>";
      XElement productGroupData = XElement.Parse(cacheDataXml);
      var items = productGroupData.Descendants("item");

      foreach(XElement itemElement in items)
      {
        var groupIdAttribute = itemElement.Attribute("pl_productGroupID");
        int productGroupId;
        if ((groupIdAttribute != null) && (int.TryParse(groupIdAttribute.Value, out productGroupId)))
        {
          _productGroupsOffered.Add(productGroupId);
        }
      }
    }

    public int Count
    {
      get { return _productGroupsOffered.Count; }
    }

    public bool IsOffered(int productGroupId)
    {
      return _productGroupsOffered.Contains(productGroupId);
    }

    public static ProductGroupsOfferedResponseData FromCacheXml(string cacheDataXml)
    {
      return new ProductGroupsOfferedResponseData(cacheDataXml);
    }
  }
}
