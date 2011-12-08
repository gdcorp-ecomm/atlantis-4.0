using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FastballProduct.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballProduct.Impl
{
  public class FastballProductRequest : IRequest
  {
    private static DateTime _donotCallUntil;
    private static TimeSpan _tenMinutes;
    private static Dictionary<string, string> _emptyResult;

    static FastballProductRequest()
    {
      _donotCallUntil = DateTime.MinValue;
      _tenMinutes = TimeSpan.FromMinutes(10.0);
      _emptyResult = new Dictionary<string, string>();
    }

    /// <summary>
    /// This request data is designed to not fail, and always return something that can be cached into the users session
    /// 0. If appsetting is not 'true' we don't call at all.
    /// 1. If the webservice fails, then we return empty values so the consuming application will just use defaults
    /// 2. If the webservice returns a "no testing active" then we set a static DateTime and do not make the call.
    ///    We do this without locking because a few extra calls will not hurt us.
    /// </summary>
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FastballProductResponseData result = new FastballProductResponseData(_emptyResult);
      string placement = string.Empty;

      try
      {
        if (DateTime.Now > _donotCallUntil)
        {
          // Check for service switch override
          string fastballProductOn = DataCache.DataCache.GetAppSetting("ATLANTIS_FRAMEWORK_FASTBALLPRODUCT_ON");
          if ("true".Equals(fastballProductOn, StringComparison.InvariantCultureIgnoreCase))
          {
            FastballProductRequestData request = (FastballProductRequestData)requestData;

            // Validate request
            placement = request.Placement;

            if (string.IsNullOrEmpty(placement))
            {
              throw new ArgumentException("Placement is empty or null.");
            }

            if (string.IsNullOrEmpty(requestData.Pathway))
            {
              throw new ArgumentException("Pathway is empty or null.");
            }

            using (offersApiWS.Service service = new offersApiWS.Service())
            {
              service.Url = ((WsConfigElement)config).WSURL;
              service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
              // make the call

              // if Response says no test
              {
                _donotCallUntil = DateTime.Now.Add(_tenMinutes);
              }
              // else
              {
                // parse the data to a dictionary and set the result;
              }

            }

          }
        }

      }
      catch (Exception ex)
      {
        result = new FastballProductResponseData(_emptyResult);
        try
        {
          string data = "Placement" + (placement ?? "null") + ":Pathway" + (requestData.Pathway ?? "null");
          AtlantisException aex = new AtlantisException(requestData, "FastballProduct.RequestHandler", ex.Message + ex.StackTrace, data);
          Engine.Engine.LogAtlantisException(aex);
        }
        catch { }
      }

      return result;
    }
  }
}
