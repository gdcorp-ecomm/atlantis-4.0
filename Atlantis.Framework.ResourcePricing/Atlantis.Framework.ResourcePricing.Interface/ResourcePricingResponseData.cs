using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.ResourcePricing.Interface
{
  public class ResourcePricingResponseData:IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private readonly string _responseXml;
    private readonly RequestData _request;

    public bool IsSuccess { get; private set; }
    public Dictionary<int, LockedPrice> LockedPrices { get; private set; }

    public ResourcePricingResponseData(RequestData request, string responseXml) 
    {
      IsSuccess = true;

      _request = request;
      _responseXml = responseXml;

      LockedPrices = new Dictionary<int, LockedPrice>();

      var xml = XDocument.Parse(responseXml);
      if (xml.Root != null && xml.Root.HasElements)
      {
        foreach (XElement element in xml.Descendants("Item"))
        {
          int upid = GetIntFromElement(element, "unifiedProductID");
          int price = GetIntFromElement(element, "price");

          LockedPrices[upid] = new LockedPrice(price, ((ResourcePricingRequestData)request).TransactionalCurrencyType, CurrencyPriceType.Transactional);
        }
      }
    }

    public ResourcePricingResponseData(string responseXml, AtlantisException atlantisException)
    {
      IsSuccess = false;
      _responseXml = responseXml;
      _atlantisException = atlantisException;
    }

    public ResourcePricingResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public ResourcePricingResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , requestData.GetType().ToString()
        , string.Format("ResourcePricingResponseData Error: {0}", ex.Message)
        , ex.StackTrace
        , ex);                                   
    }

    private static int GetIntFromElement(XElement element, string xname)
    {
      int retVal = 0;

      var xAttribute = element.Attribute(XName.Get(xname));
      if (xAttribute != null)
        int.TryParse(xAttribute.Value, out retVal);

      return retVal;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members
    
  }
}
