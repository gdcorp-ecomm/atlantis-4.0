using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TrafficMobileTracking.Impl.MobileTrackingService;
using Atlantis.Framework.TrafficMobileTracking.Interface;

namespace Atlantis.Framework.TrafficMobileTracking.Impl
{
  public class TrafficMobileTrackingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IfbiMobileTrackingServiceClient mobileTrackingServiceClient = null;
      IResponseData responseData;

      try
      {
        TrafficMobileTrackingRequestData trafficMobileTrackingRequestData = (TrafficMobileTrackingRequestData)oRequestData;
        WsConfigElement wsConfig = (WsConfigElement) oConfig;

        mobileTrackingServiceClient = GetWebServiceInstance(wsConfig.WSURL, trafficMobileTrackingRequestData.RequestTimeout);
        if(trafficMobileTrackingRequestData.CiCode != null)
        {
          mobileTrackingServiceClient.MobileClientDataWithCICode(trafficMobileTrackingRequestData.MobileDeviceUid,
                                                                 trafficMobileTrackingRequestData.MobileApplicationId,
                                                                 trafficMobileTrackingRequestData.ApplicationVersion,
                                                                 trafficMobileTrackingRequestData.OperatingSystem,
                                                                 trafficMobileTrackingRequestData.OperatingSystemVersion,
                                                                 trafficMobileTrackingRequestData.DeviceModel,
                                                                 trafficMobileTrackingRequestData.DeviceType,
                                                                 trafficMobileTrackingRequestData.ShopperID,
                                                                 trafficMobileTrackingRequestData.PrivateLabelId,
                                                                 trafficMobileTrackingRequestData.MethodName,
                                                                 trafficMobileTrackingRequestData.ClientIp,
                                                                 trafficMobileTrackingRequestData.ClientCarrier,
                                                                 trafficMobileTrackingRequestData.LogData,
                                                                 trafficMobileTrackingRequestData.CiCode.Value);
        }
        else
        {
          mobileTrackingServiceClient.MobileClientData(trafficMobileTrackingRequestData.MobileDeviceUid,
                                                       trafficMobileTrackingRequestData.MobileApplicationId,
                                                       trafficMobileTrackingRequestData.ApplicationVersion,
                                                       trafficMobileTrackingRequestData.OperatingSystem,
                                                       trafficMobileTrackingRequestData.OperatingSystemVersion,
                                                       trafficMobileTrackingRequestData.DeviceModel,
                                                       trafficMobileTrackingRequestData.DeviceType,
                                                       trafficMobileTrackingRequestData.ShopperID,
                                                       trafficMobileTrackingRequestData.PrivateLabelId,
                                                       trafficMobileTrackingRequestData.MethodName,
                                                       trafficMobileTrackingRequestData.ClientIp,
                                                       trafficMobileTrackingRequestData.ClientCarrier,
                                                       trafficMobileTrackingRequestData.LogData);
        }

        responseData = new TrafficMobileTrackingResponseData();
      }
      catch (Exception ex)
      {
        responseData = new TrafficMobileTrackingResponseData(oRequestData, ex);
      }
      finally
      {
        if (mobileTrackingServiceClient != null && mobileTrackingServiceClient.State == CommunicationState.Opened)
        {
          mobileTrackingServiceClient.Close();
        }
      }

      return responseData;
    }

    private static IfbiMobileTrackingServiceClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
    {
      WSHttpBinding wsHttpBinding = new WSHttpBinding(SecurityMode.None);
      wsHttpBinding.SendTimeout = requestTimeout;
      wsHttpBinding.OpenTimeout = requestTimeout;
      wsHttpBinding.CloseTimeout = requestTimeout;
      wsHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10); // default
      wsHttpBinding.AllowCookies = false;
      wsHttpBinding.BypassProxyOnLocal = false;
      wsHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
      wsHttpBinding.MessageEncoding = WSMessageEncoding.Text;
      wsHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
      wsHttpBinding.UseDefaultWebProxy = true;

      wsHttpBinding.Security.Mode = SecurityMode.None;
      wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
      wsHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
      wsHttpBinding.Security.Transport.Realm = string.Empty;
      wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
      wsHttpBinding.Security.Message.NegotiateServiceCredential = true;
      wsHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
      wsHttpBinding.Security.Message.EstablishSecurityContext = true;

      EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder();
      endpointAddressBuilder.Identity = EndpointIdentity.CreateDnsIdentity("localhost");
      endpointAddressBuilder.Uri = new Uri(webServiceUrl);

      return new IfbiMobileTrackingServiceClient(wsHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }
  }
}
