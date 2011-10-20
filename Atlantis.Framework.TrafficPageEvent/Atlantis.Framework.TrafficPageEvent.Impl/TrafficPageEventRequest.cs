using System;
using System.ServiceModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TrafficPageEvent.Impl.Traffic;
using Atlantis.Framework.TrafficPageEvent.Interface;

namespace Atlantis.Framework.TrafficPageEvent.Impl
{
  public class TrafficPageEventRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PageEventServiceClient pageEventServiceClient = null;
      IResponseData responseData;

      try
      {
        TrafficPageEventRequestData trafficPageEventRequestData = (TrafficPageEventRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement) oConfig);

        pageEventServiceClient = GetWebServiceInstance(wsConfig.WSURL, trafficPageEventRequestData.RequestTimeout);
        pageEventServiceClient.LogPageEvents(trafficPageEventRequestData.Pathway,
                                             trafficPageEventRequestData.PageName,
                                             trafficPageEventRequestData.CiCode,
                                             trafficPageEventRequestData.EventType,
                                             trafficPageEventRequestData.CiImpressions,
                                             trafficPageEventRequestData.UserKeyValuePairsString);

        responseData = new TrafficPageEventResponseData(true);
      }
      catch(Exception ex)
      {
        responseData = new TrafficPageEventResponseData(oRequestData, ex);
      }
      finally
      {
        if(pageEventServiceClient != null && pageEventServiceClient.State == CommunicationState.Opened)
        {
          pageEventServiceClient.Close();
        }
      }

      return responseData;
    }

    private PageEventServiceClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
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

      wsHttpBinding.ReaderQuotas.MaxDepth = 32;
      wsHttpBinding.ReaderQuotas.MaxStringContentLength = 8192;
      wsHttpBinding.ReaderQuotas.MaxArrayLength = 16384;
      wsHttpBinding.ReaderQuotas.MaxBytesPerRead = 4096;
      wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = 16384;

      wsHttpBinding.ReliableSession.Ordered = true;
      wsHttpBinding.ReliableSession.InactivityTimeout = TimeSpan.FromMinutes(10);
      wsHttpBinding.ReliableSession.Enabled = false;

      wsHttpBinding.Security.Mode = SecurityMode.Message;

      wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
      wsHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
      wsHttpBinding.Security.Transport.Realm = string.Empty;

      wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
      wsHttpBinding.Security.Message.NegotiateServiceCredential = true;

      EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder();
      endpointAddressBuilder.Identity = EndpointIdentity.CreateDnsIdentity("localhost");
      endpointAddressBuilder.Uri = new Uri(webServiceUrl);

      return new PageEventServiceClient(wsHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }
  }
}