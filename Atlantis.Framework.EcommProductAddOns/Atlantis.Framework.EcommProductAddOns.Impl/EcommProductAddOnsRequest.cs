using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.EcommProductAddOns.Impl.BillingStateSvc;
using Atlantis.Framework.EcommProductAddOns.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommProductAddOns.Impl
{
  public class EcommProductAddOnsRequest : IRequest
  {
    private bool? HasRenewableAddon { get; set; }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommProductAddOnsResponseData responseData;
      
      try
      {
        var wsConfigElement = (WsConfigElement) config;

        using (var billingStateSvc = new wscgdBillingStateService())
        {
          var request = (EcommProductAddOnsRequestData)requestData;

          billingStateSvc.Url = wsConfigElement.WSURL;
          billingStateSvc.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          string responseXml;
          string errorXml;
          var statusCode = billingStateSvc.Query(request.OrionId, request.ResourceType, request.IdType, out responseXml, out errorXml);

          if (statusCode < 0 && !string.IsNullOrEmpty(errorXml))
          {
            var tuple = GetAtlantisExceptionData(errorXml);
            var aex = new AtlantisException(requestData, "EcommProductAddOnsRequest::RequestHandler", tuple.Item1, tuple.Item2);
            responseData = new EcommProductAddOnsResponseData(aex);
          }
          else if (statusCode < 0)
          {
            var aex = new AtlantisException(requestData, "EcommProductAddOnsRequest::RequestHandler", "Unknown Error", string.Empty);
            responseData = new EcommProductAddOnsResponseData(aex);            
          }
          else
          {
            var addOns = CreateAddOnProductList(responseXml, request.PrivateLabelId);
            responseData = new EcommProductAddOnsResponseData(addOns, HasRenewableAddon.Value);
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommProductAddOnsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommProductAddOnsResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Private Methods
    private List<AddOnProduct> CreateAddOnProductList(string responseXml, int privateLabelId)
    {
      var addOnProducts = new List<AddOnProduct>();
      
      var xDoc = XDocument.Parse(responseXml);
      var bsq = xDoc.Element("BillingStateQuery");
      if (bsq != null)
      {
        var account = bsq.Element("Account");
        if (account != null)
        {
          var addOns = account.Elements("AddOn");
          foreach (var addOn in addOns)
          {
            addOnProducts.Add(CreateAddOnProduct(addOn, privateLabelId));
          }          
        }
      }
      return addOnProducts;
    }

    private AddOnProduct CreateAddOnProduct(XElement addOn, int privateLabelId)
    {
      const string BILLING_SYNC_CALL = "<BillingSyncProductList><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privatelabelResellerTypeID\" value=\"{1}\"/></BillingSyncProductList>";
      if (!HasRenewableAddon.HasValue)
      {
        HasRenewableAddon = false;
      }

      var addOnProduct = new AddOnProduct
                           {
                             Quantity = addOn.Attribute("quantity") != null ? Convert.ToInt32(addOn.Attribute("quantity").Value) : 0,
                             UnifiedProductId = addOn.Attribute("UnifiedProductID") != null ? Convert.ToInt32(addOn.Attribute("UnifiedProductID").Value) : 0
                           };

      var productList = DataCache.DataCache.GetCacheData(string.Format(BILLING_SYNC_CALL, addOnProduct.UnifiedProductId, privateLabelId));

      var xDoc = XDocument.Parse(productList);

      var dataElement = xDoc.Element("data");
      if (dataElement != null)
      {
        var ownedProduct = dataElement.Elements("item").FirstOrDefault(item => item.Attribute("catalog_productUnifiedProductID").Value == addOnProduct.UnifiedProductId.ToString());
        if (ownedProduct != null)
        {
          addOnProduct.RenewalUnifiedProductId = 0;
          var renewalItem = dataElement.Elements("item").FirstOrDefault(item => item.Attribute("isRenewal").Value == "1" && item.Attribute("recurring_payment").Value == ownedProduct.Attribute("recurring_payment").Value && item.Attribute("numberOfPeriods").Value == ownedProduct.Attribute("numberOfPeriods").Value);
          if (renewalItem != null)
          {
            HasRenewableAddon = true;
            addOnProduct.RenewalUnifiedProductId = Convert.ToInt32(renewalItem.Attribute("catalog_productUnifiedProductID").Value);
          }
        }
      }
      return addOnProduct;
    }

    private static Tuple<string, string> GetAtlantisExceptionData(string errorXml)
    {
      var msg = string.Empty;
      var data = string.Empty;

      var xDoc = XDocument.Parse(errorXml);
      var error = xDoc.Element("Error");
      if (error != null)
      {
        var reason = error.Element("Reason");
        if (reason != null)
        {
          msg = reason.Value;            
        }
        var method = error.Element("Method");
        if (method != null)
        {
          data = method.Value;
        }
      }

      return new Tuple<string, string>(msg, data);
    }
    #endregion
  }
}
