using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TrafficSmsTracking.Impl.FbiSmsTrackingService;
using Atlantis.Framework.TrafficSmsTracking.Interface;

namespace Atlantis.Framework.TrafficSmsTracking.Impl
{
  public class TrafficSmsTrackingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IfbiSmsTrackingServiceClient smsTrackingServiceClient = null;
      IResponseData responseData;

      try
      {
        TrafficSmsTrackingRequestData trafficSmsTrackingRequestData = (TrafficSmsTrackingRequestData) requestData;
        WsConfigElement wsConfig = ((WsConfigElement) config);

        smsTrackingServiceClient = GetWebServiceInstance(wsConfig.WSURL, trafficSmsTrackingRequestData.RequestTimeout);

        smsTrackingServiceClient.SendData(trafficSmsTrackingRequestData.SmsId,
                                          trafficSmsTrackingRequestData.MessageId,
                                          trafficSmsTrackingRequestData.PhoneNumberHash,
                                          trafficSmsTrackingRequestData.MobileCarrier,
                                          trafficSmsTrackingRequestData.MessageReceivedDate,
                                          trafficSmsTrackingRequestData.MessageReceieved,
                                          trafficSmsTrackingRequestData.SearchedSld,
                                          trafficSmsTrackingRequestData.SearchedTld,
                                          trafficSmsTrackingRequestData.IsAvailable,
                                          trafficSmsTrackingRequestData.MessageSentDate,
                                          trafficSmsTrackingRequestData.MessageSent,
                                          trafficSmsTrackingRequestData.ActionPerformed,
                                          trafficSmsTrackingRequestData.ProposedSld,
                                          trafficSmsTrackingRequestData.ProposedTld,
                                          Environment.MachineName);

        responseData = new TrafficSmsTrackingResponseData(true);
      }
      catch (Exception ex)
      {
        responseData = new TrafficSmsTrackingResponseData(requestData, ex);
      }
      finally
      {
        if(smsTrackingServiceClient != null && smsTrackingServiceClient.State == CommunicationState.Opened)
        {
          smsTrackingServiceClient.Close();
        }
      }

      return responseData;
    }

    private IfbiSmsTrackingServiceClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
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

      return new IfbiSmsTrackingServiceClient(wsHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }
  }
}
