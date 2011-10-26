using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.StratosphereGetMap.Interface;

namespace Atlantis.Framework.StratosphereGetMap.Impl
{
  public class StratosphereGetMapAsyncRequest : IAsyncRequest
  {
    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      X509Certificate2 cert = new X509Certificate2();
      string aggregateUri = string.Empty;

      StratosphereGetMapRequestData request = (StratosphereGetMapRequestData)requestData;

      try
      {
        cert = MapHelper.FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();
        aggregateUri = MapHelper.GetStratosphereRequestUrl(request, config, cert);
      }
      catch (Exception ex)
      {
        string certName = cert.FriendlyName == null ? "Certificate could not be found" : cert.FriendlyName;
        string data = string.Format("Error getting certificate or Url.  Certificate Name: {0} | Url: {1}", certName, aggregateUri);
        throw new AtlantisException(requestData
          , "StratosphereGetMapAsyncRequest::BeginHandleRequest"
          , ex.Message
          , data
          , ex);
      }

      HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(aggregateUri);
      webRequest.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
      webRequest.ClientCertificates.Add(cert);
        
      AsyncState asyncState = new AsyncState(requestData, config, webRequest, state);
      IAsyncResult asyncResult = webRequest.BeginGetResponse(callback, asyncState);

      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      IResponseData responseData = null;
      string responseXml = string.Empty;
      AsyncState asyncState = (AsyncState)asyncResult.AsyncState;

      try
      {
        HttpWebRequest webRequest = (HttpWebRequest)asyncState.Request;
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult);

        if (webResponse.StatusCode == HttpStatusCode.OK)
        {
          string xml = string.Empty;
          using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
          {
            xml = reader.ReadToEnd();
          }
          responseData = new StratosphereGetMapResponseData(xml);
        }
        else
        {
          string data = string.Format("Error invoking map aggregates.  Status Code: {0}", webResponse.StatusCode);
          AtlantisException aex = new AtlantisException(asyncState.RequestData, "StratosphereGetMapAsyncRequest::EndHandleRequest", webResponse.StatusDescription, data);
          responseData = new StratosphereGetMapResponseData(aex);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new StratosphereGetMapResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new StratosphereGetMapResponseData(asyncState.RequestData, ex);
      }

      return responseData;
    }
  }
}
