using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.FTE.Interface
{
  public class FteWebRequest
  {
    public void GetFTEToken(string urlRequest, string admin, string password, Properties getApiProps, out WebResponse webResponse, out HttpWebRequest httpWebRequest)
    {
      httpWebRequest = (HttpWebRequest)WebRequest.Create(urlRequest);

      httpWebRequest.ContentType = "text/json";
      httpWebRequest.Method = "POST";
      httpWebRequest.Credentials = new NetworkCredential(admin, password);

      SerializeJsonCall(getApiProps.RequestToken, httpWebRequest);

      webResponse = httpWebRequest.GetResponse();

      DeserializeJsonCall(getApiProps, webResponse);

      getApiProps.GetTokenProps();
    }

    public void StatesAvailable(Properties getAPIProperties, string urlRequest, HttpWebRequest httpWebRequest, WebResponse webResponse, string ccCode)
    {
      getAPIProperties.GetStateProps(ccCode);

      httpWebRequest = (HttpWebRequest)WebRequest.Create(urlRequest);
      httpWebRequest.ContentType = "text/json";
      httpWebRequest.Method = "POST";

      SerializeJsonCall(getAPIProperties.RequestStates, httpWebRequest);

      webResponse = httpWebRequest.GetResponse();

      DeserializeJsonCall(getAPIProperties, webResponse);

      getAPIProperties.GetStateProps();
    }

    public void StateAreaCodes(Properties getAPIProperties, string urlRequest, string geoCode, HttpWebRequest httpWebRequest, WebResponse webResponse)
    {
      getAPIProperties.GetAreaCodeProps(geoCode);

      httpWebRequest = (HttpWebRequest)WebRequest.Create(urlRequest);
      httpWebRequest.ContentType = "text/json";
      httpWebRequest.Method = "POST";

      SerializeJsonCall(getAPIProperties.RequestAreaCodes, httpWebRequest);

      webResponse = httpWebRequest.GetResponse();

      DeserializeJsonCall(getAPIProperties, webResponse);

      getAPIProperties.SetAvailableAreaCodes();
    }

    private void SerializeJsonCall(Dictionary<string, object> dictionary, HttpWebRequest httpWebRequest)
    {
      var jsSerializer = new JavaScriptSerializer();

      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        var json = jsSerializer.Serialize(dictionary);

        streamWriter.Write(json);
      }
    }

    private void DeserializeJsonCall(Properties getProperties, WebResponse webResponse)
    {
      var jsSerializer = new JavaScriptSerializer();

      using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
      {
        var responseText = streamReader.ReadToEnd();

        getProperties.FteProperties = jsSerializer.Deserialize<Dictionary<string, object>>(responseText);
      }
    }
  }
}
