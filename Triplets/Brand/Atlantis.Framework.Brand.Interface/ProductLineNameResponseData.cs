using System.Collections.Generic;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineNameResponseData : IResponseData
  {
    private AtlantisException _exception;
    private List<ProductLineName> _productLineList;

    public static ProductLineNameResponseData Empty { get; private set; }

    public static ProductLineNameResponseData FromProductLineNameXml(string productLineXml)
    {
      XDocument productLineNamesDoc = XDocument.Parse(productLineXml);
      List<ProductLineName> productLineList = new List<ProductLineName>();

      foreach (XElement productLineNamesElement in productLineNamesDoc.Descendants("ProductLineName"))
      {
        ProductLineName productLine = ProductLineName.FromCacheXml(productLineNamesElement);

        productLineList.Add(productLine);
      }

      return new ProductLineNameResponseData(productLineList);
    }

    static ProductLineNameResponseData()
    {
      Empty = new ProductLineNameResponseData(new List<ProductLineName>());
    }

    public IEnumerable<ProductLineName> ProductLineNames
    {
      get { return _productLineList; }
    }

    private ProductLineNameResponseData(List<ProductLineName> productLineList)
    {
      _productLineList = productLineList;
    }

    public static ProductLineNameResponseData FromException(AtlantisException exception)
    {
      return new ProductLineNameResponseData(exception);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    private ProductLineNameResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public string ToXML()
    {
      XElement element = new XElement("ProductLineNameResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
