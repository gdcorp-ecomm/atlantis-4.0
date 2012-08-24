using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Atlantis.Framework.EasyDBGetUsage.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EasyDBGetUsage.Impl
{
  public class EasyDBGetUsageRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EasyDBGetUsageResponseData responseData = null;

      try
      {
        EasyDBGetUsageRequestData request = (EasyDBGetUsageRequestData)requestData;

        Uri address = new Uri(((WsConfigElement)config).WSURL);
        var webRequest = WebRequest.Create(address) as HttpWebRequest;
        webRequest.Method = "POST";
        webRequest.ContentType = "application/json";

        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        using (TextWriter writer = new StreamWriter(webRequest.GetRequestStream()))
        {
          EasyDBGetUsageJsonRequestData jsonRequest = new EasyDBGetUsageJsonRequestData();
          jsonRequest.accountUid = request.AccountUid;

          string jdata = jsSerializer.Serialize(jsonRequest);

          writer.Write(jdata);
        }

        using (var webResponse = webRequest.GetResponse() as HttpWebResponse)
        {
          if (webResponse != null)
          {
            using (TextReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
              EasyDBGetUsageJsonResponseData jsonResponse = null;
              string jdata = reader.ReadToEnd();

              try
              {
                jsonResponse = jsSerializer.Deserialize<EasyDBGetUsageJsonResponseData>(jdata);
              }
              catch { }

              if (jsonResponse != null && jsonResponse.result == 1)
              {
                responseData = new EasyDBGetUsageResponseData(jsonResponse.disk_used, jsonResponse.disk_available, jsonResponse.bandwidth_used, jsonResponse.bandwidth_available, jsonResponse.measurement_unit);
              }
              else
              {
                EasyDBGetUsageJsonResponseErrorData jsonErrorResponse = null;

                try
                {
                  jsonErrorResponse = jsSerializer.Deserialize<EasyDBGetUsageJsonResponseErrorData>(jdata);
                }
                catch { }

                string errorMessage = "Error deserializing JSON from Easy Database Quotas web service";
                if (jsonErrorResponse != null)
                {
                  errorMessage = string.Format("Error {0}: {1}", jsonErrorResponse.error.code, jsonErrorResponse.error.message);
                }

                responseData = new EasyDBGetUsageResponseData(requestData, new Exception(errorMessage));
              }

              reader.Close();
            }
          }
          else
          {
            responseData = new EasyDBGetUsageResponseData(requestData, new Exception("Error invoking Easy Database Quotas web service"));
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new EasyDBGetUsageResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new EasyDBGetUsageResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Serializable Objects

    [Serializable]
    private class EasyDBGetUsageJsonRequestData
    {
      public string accountUid { get; set; }
    }

    [Serializable]
    private class EasyDBGetUsageJsonResponseData
    {
      public int result { get; set; }
      public double disk_used { get; set; }
      public double bandwidth_used { get; set; }
      public double disk_available { get; set; }
      public double bandwidth_available { get; set; }
      public string measurement_unit { get; set; }
    }

    [Serializable]
    private class EasyDBGetUsageJsonResponseErrorData
    {
      public int result { get; set; }
      public JsonError error { get; set; }

      public struct JsonError
      {
        public int code { get; set; }
        public string message { get; set; }
      }
    }

    #endregion
  }
}
