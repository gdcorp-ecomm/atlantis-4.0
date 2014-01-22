using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoData.Impl.FastballWS;
using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  public class PromoDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData,
       ConfigElement configElement)
    {
      IResponseData responseData = null;
      string responseXML = String.Empty;
      
      FastballWS.Service ws = new FastballWS.Service();
      
      try
      {
        PromoDataRequestData request = (PromoDataRequestData)requestData;
        
        string requestXml = request.ToXML();
        ws.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        WsConfigElement ce = (WsConfigElement)configElement;
        ws.Url = ((WsConfigElement)ce).WSURL;
        PromoRequest pr = new PromoRequest();
        pr.ShowInactivePromos = true;
        pr.PromoCodes = new PromoRequestItem[1];
        pr.PromoCodes[0] = new PromoRequestItem() { PromoID = request.PromoCode };
        IPromoProduct invalidProduct = new PromoProduct(string.Empty, DateTime.MinValue, DateTime.MinValue, false);
        Promo[] promoList;
        bool isValid = (ws.GetPromoData(pr, out promoList) == 0);

        if (isValid)
        {
          Promo promo = promoList[0];

          if (promo.isValid)
          {
            string promoCode = promo.promoCode;
            DateTime startDate;
            DateTime.TryParse(promo.Product.startDate, out startDate);
            DateTime endDate;
            DateTime.TryParse(promo.Product.endDate, out endDate);

            if (promo.Product.ProductAwards.Length > 0)
            {
              PromoProduct product = new PromoProduct(promo.promoCode, startDate, endDate, promo.Product.active);
              Dictionary<int, IPromoCondition> conditions = LoadPromoConditions(promo.Product.ProductConditions);

              foreach (PromoProductAward productAward in promo.Product.ProductAwards)
              {
                int productId;
                string[] arr = productAward.unifiedProductID.Split('|');

                if ((arr.Length > 0) && int.TryParse(arr[0], out productId)
                  && (productAward.AwardCurrencies.Length > 0))
                {
                  IPromoCondition cnd;
                  if (!conditions.TryGetValue(productId, out cnd))
                  {
                    cnd = new PromoCondition();
                  }

                  ProductAward pa = new ProductAward(productAward.type, productId,
                    productAward.productQtyLimit, cnd);

                  foreach (AwardCurrency award in productAward.AwardCurrencies)
                  {
                    ProductAwardCurrency pac = new ProductAwardCurrency(award.transactionalCurrency, award.awardAmount);
                    pa.AddProductAwardCurrency(pac);
                  }

                  foreach (PrivateLabelType plType in promo.Product.PrivateLabelTypes)
                  {
                    product.AddPrivateLabelType(plType.privateLabelTypeID);
                  }

                  product.Disclaimer = promo.Product.Disclaimer;
                  product.AddProductAward(pa);
                }
                else
                {
                  LogException("PromoDataRequest.RequestHandler", "91",
                    "Invalid unifiedProductID type. PromoCode: " + promoCode, string.Empty);
                }
              }

              responseData = new PromoDataResponseData(product);
            }
            else
            {
              responseData = new PromoDataResponseData(invalidProduct);
              LogException("PromoDataRequest.RequestHandler", "90",
                "No ProductAwards received for PromoCode: " + promoCode, string.Empty);
            }
          }
          else
          {
            responseData = new PromoDataResponseData(invalidProduct);
            LogException("PromoDataRequest.RequestHandler", "89",
              "Promo is Invalid PromoCode: " + promo.promoCode, string.Empty);
          }
        }
        else
        {
          responseData = new PromoDataResponseData(invalidProduct);
          LogException("PromoDataRequest.RequestHandler", "88",
            "No valid PromoProduct received for PromoCode: " + request.PromoCode, string.Empty);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new PromoDataResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new PromoDataResponseData(requestData, ex);
      }
      finally
        {
          ws.Dispose();
        }

      return responseData;
    }

    private Dictionary<int, IPromoCondition> LoadPromoConditions(PromoProductCondition[] promoConditions)
    {
      Dictionary<int, IPromoCondition> result = new Dictionary<int, IPromoCondition>();

      foreach (PromoProductCondition cnd in promoConditions)
      {
        int productId;
        if(int.TryParse(cnd.unifiedProductID, out productId))
        {
          result[productId] = new PromoCondition(cnd.productMinQty, cnd.productExactQty, 
            cnd.conditionField, cnd.conditionValue);
        }
      }

      return result;
    }

    private void LogException(string sourceFunction, 
      string errorNumber, string description, string data)
    {
      int errorNum = 0;
      Int32.TryParse(errorNumber, out errorNum);

      AtlantisException aex = new AtlantisException(sourceFunction, errorNum, description, data);
      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
