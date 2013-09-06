using System;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Support.Interface;

namespace Atlantis.Framework.Support.Impl
{
  public class SupportPhoneRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;

      try
      {
        var supportPhoneRequest = (SupportPhoneRequestData)requestData;

        if (supportPhoneRequest.ResellerTypeId <= 0)
        {
          throw new Exception("ResellerTypeId should be greater than 0");
        }

        string responseXml;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData("<GetFlagSiteSettingsByResellerType><param name='n_privateLabelResellerType' value='" + supportPhoneRequest.ResellerTypeId.ToString(CultureInfo.InvariantCulture) + "'/></GetFlagSiteSettingsByResellerType>");
        }

        if (string.IsNullOrEmpty(responseXml))
        {
          throw new Exception("Null or empty response xml");
        }

        var responseElement = XElement.Parse(responseXml);
        result = SupportPhoneResponseData.FromResponseXml(responseElement);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(requestData, "SupportPhoneRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = SupportPhoneResponseData.FromException(aex);
      }

      return result;
    }
  }
}
