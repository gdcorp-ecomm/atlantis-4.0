using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.StratosphereGetMap.Interface;

namespace Atlantis.Framework.StratosphereGetMap.Impl
{
  public class StratosphereGetMapRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      StratosphereGetMapResponseData responseData = null;
      HttpWebRequest webRequest = null;
      HttpWebResponse webResponse = null;

      try
      {
        X509Certificate2 cert = MapHelper.FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();

        StratosphereGetMapRequestData request = (StratosphereGetMapRequestData)requestData;
        string aggregateUri = MapHelper.GetStratosphereRequestUrl(request, config, cert);

        webRequest = (HttpWebRequest)System.Net.WebRequest.Create(aggregateUri);
        webRequest.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        webRequest.ClientCertificates.Add(cert);
        
        webResponse = (HttpWebResponse)webRequest.GetResponse();

        if (webResponse.StatusCode == HttpStatusCode.OK)
        {
          string xml = string.Empty;
          try
          {
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
              xml = reader.ReadToEnd();
            }
          }
          catch
          { } // swallow 
          finally
          {
            responseData = new StratosphereGetMapResponseData(xml);
          }
        }
        else
        {
          string data = string.Format("Error invoking map aggregates.  Status Code: {0}", webResponse.StatusCode);
          AtlantisException aex = new AtlantisException(requestData, "StratosphereGetMapRequest::RequestHandler", webResponse.StatusDescription, data);
          responseData = new StratosphereGetMapResponseData(aex);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new StratosphereGetMapResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new StratosphereGetMapResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
