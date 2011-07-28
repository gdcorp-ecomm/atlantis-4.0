using System.IO;
using System.Xml;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Products
{
  public class ProductInfo : IProductInfo
  {
    private const string ProductInfoRequestXml = "<GetProductInfoByUnifiedPFID><param name=\"n_gdshop_product_unifiedProductID\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetProductInfoByUnifiedPFID>";
    private int _productId;
    private RecurringPaymentUnitType _recurringPayment;
    private int _numberOfPeriods;
    private int _gdshop_product_typeID;
    private string _name;

    public ProductInfo(int productId, int privateLabelId)
    {
      _productId = productId;
      _recurringPayment = RecurringPaymentUnitType.Unknown;
      _numberOfPeriods = 0;
      _gdshop_product_typeID = -1;

      string xml = DataCache.DataCache.GetCacheData(string.Format(ProductInfoRequestXml, productId, privateLabelId));

      if (string.IsNullOrEmpty(xml))
      {
        string message = "ProductInfo not found for product=" + _productId.ToString() + ", plid=" + privateLabelId.ToString() + ". This is a 120 minute cache.";
        string data = xml;
        AtlantisException ex = new AtlantisException("IProductInfo.ProductInfo", "86", message, data, null, null);
        Engine.Engine.LogAtlantisException(ex);
      }
      else
      {
        using (StringReader sr = new StringReader(xml))
        {
          using (XmlReader reader = XmlReader.Create(sr))
          {
            while (reader.Read())
            {
              if (reader.Name == "item")
              {
                while (reader.MoveToNextAttribute())
                {
                  switch (reader.Name)
                  {
                    case "numberOfPeriods":
                      int numPeriods;
                      if (int.TryParse(reader.Value, out numPeriods))
                      {
                        _numberOfPeriods = numPeriods;
                      }
                      break;
                    case "recurring_payment":
                      if (reader.Value == "monthly")
                        _recurringPayment = RecurringPaymentUnitType.Monthly;
                      else if (reader.Value == "annual")
                        _recurringPayment = RecurringPaymentUnitType.Annual;
                      else if (reader.Value == "semiannual")
                        _recurringPayment = RecurringPaymentUnitType.SemiAnnual;
                      else if (reader.Value == "quarterly")
                        _recurringPayment = RecurringPaymentUnitType.Quarterly;
                      else
                        _recurringPayment = RecurringPaymentUnitType.Unknown;
                      break;
                    case "gdshop_product_typeID":
                      int productTypeId;
                      if (int.TryParse(reader.Value, out productTypeId))
                      {
                        _gdshop_product_typeID = productTypeId;
                      }
                      break;
                    case "name":
                      _name = reader.Value;
                      break;

                  }
                }
              }
            }
          }
        }
      }

      if (string.IsNullOrEmpty(_name))
      {
        _name = _productId.ToString();
      }
    }

    public string Name
    {
      get { return _name; }
    }

    public int ProductTypeId
    {
      get
      {
        return _gdshop_product_typeID;
      }
    }

    public int NumberOfPeriods
    {
      get
      {
        return _numberOfPeriods;
      }
    }

    public RecurringPaymentUnitType RecurringPayment
    {
      get { return _recurringPayment; }
    }
  }
}
