using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Security;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS;
using Atlantis.Framework.FileHandlerServiceSaveFile.Interface;

namespace Atlantis.Framework.FileHandlerServiceSaveFile.Impl
{
  public class FileHandlerServiceSaveFileRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IntakeServiceClient storageClient = null;
      IResponseData responseData;

      try
      {
        FileHandlerServiceSaveFileRequestData request = (FileHandlerServiceSaveFileRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);
        storageClient = GetWebServiceInstance(wsConfig.WSURL, request.RequestTimeout, request.Environment);

        string message = string.Empty;
        bool success = false;
        success = storageClient.SaveFile(request.ApplicationData, request.ApplicationKey, request.FileNameOnly,
                      request.SettingId, request.SubscriberId, request.Stream, out message);
      

        responseData = new FileHandlerServiceSaveFileResponseData(message, success);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new FileHandlerServiceSaveFileResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new FileHandlerServiceSaveFileResponseData(oRequestData, ex);
      }

      finally
      {
        if (storageClient != null && storageClient.State == CommunicationState.Opened)
        {
          storageClient.Close();
        }
      }

      return responseData;
    }

    private IntakeServiceClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout, string environment)
    {
      BasicHttpBinding basicHttpBinding;
      if (environment == "PROD")
      {
        basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
      }
      else
      {
        basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
      }

      basicHttpBinding.SendTimeout = requestTimeout;
      basicHttpBinding.OpenTimeout = requestTimeout;
      basicHttpBinding.CloseTimeout = requestTimeout;

      basicHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10); // default
      basicHttpBinding.AllowCookies = false;
      basicHttpBinding.BypassProxyOnLocal = false;
      basicHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
      basicHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;
      basicHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
      basicHttpBinding.UseDefaultWebProxy = true;
      basicHttpBinding.MaxBufferSize = 65536;
      basicHttpBinding.MaxBufferPoolSize = 524288;
      basicHttpBinding.MaxReceivedMessageSize = 65536;
      basicHttpBinding.TransferMode = TransferMode.Buffered;

      basicHttpBinding.ReaderQuotas.MaxDepth = 32;
      basicHttpBinding.ReaderQuotas.MaxStringContentLength = 8192;
      basicHttpBinding.ReaderQuotas.MaxArrayLength = 16384;
      basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 4096;
      basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = 16384;

      if (environment == "PROD")
      {
        basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
      }
      else
      {
        basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
      }

      basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
      basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
      basicHttpBinding.Security.Transport.Realm = string.Empty;
      basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
      basicHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;

      EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder();
      endpointAddressBuilder.Identity = EndpointIdentity.CreateDnsIdentity("localhost");
      endpointAddressBuilder.Uri = new Uri(webServiceUrl);



      return new IntakeServiceClient(basicHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }

  }
}
