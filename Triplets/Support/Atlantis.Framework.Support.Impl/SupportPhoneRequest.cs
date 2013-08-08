using System;
using System.Globalization;
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

        if (string.IsNullOrEmpty(supportPhoneRequest.CountryCode))
        {
          throw new Exception("Null or empty country code");
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

        result = SupportPhoneResponseData.FromResponseXml(responseXml, supportPhoneRequest.CountryCode);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "SupportPhoneRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = SupportPhoneResponseData.FromException(aex);
      }

      return result;
    }
  }
}
