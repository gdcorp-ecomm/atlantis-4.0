using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FastballLogImpressions.Interface;

namespace Atlantis.Framework.FastballLogImpressions.Impl
{
  public class FastballLogImpressionsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = new FastballLogImpressionsResponseData();

      DynamicTrafficClient dynamicTrafficClient = null;
      try
      {
        FastballLogImpressionsRequestData requestData = (FastballLogImpressionsRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement) oConfig);

        XmlDocument requestDoc = new XmlDocument();
        requestDoc.LoadXml("<mobileClickImpressionData/>");

        XmlNode vInfoNode = AddNode(requestDoc.FirstChild, "visitInfo");

        XmlNode mUidNode = AddNode(vInfoNode, "MobileSessionUID");
        mUidNode.InnerText = requestData.MobileSessionGuid;

        XmlNode gUidNode = AddNode(vInfoNode, "deviceUniqueID");
        gUidNode.InnerText = requestData.DeviceGuid;

        XmlNode shopperNode = AddNode(vInfoNode, "shopperID");
        shopperNode.InnerText = requestData.ShopperID;


        XmlNode eventsNode = AddNode(requestDoc.FirstChild, "events");
        foreach (FastballTrafficEvent fbEvent in requestData.FastballTrafficEvents)
        {
          XmlNode eventNode = AddNode(eventsNode, "eventInfo");
          AddAttribute(eventNode, "eventOfferID", fbEvent.EventOfferId);
          AddAttribute(eventNode, "eventDate", fbEvent.EventDate.ToString("MM/dd/yyyy HH:mm:ss"));
          AddAttribute(eventNode, "pageSequence", fbEvent.PageSequence);
          AddAttribute(eventNode, "eventType", fbEvent.EventType.ToString());
        }

        dynamicTrafficClient = GetWebServiceInstance(wsConfig.WSURL, requestData.RequestTimeout);
        dynamicTrafficClient.Process(requestDoc.InnerXml, 4);
        
        result = new FastballLogImpressionsResponseData
        {
          IsSuccess = true
        };

      }
      catch (Exception ex)
      {
        result = new FastballLogImpressionsResponseData(oRequestData, ex);        
      }
      finally
      {
        if (dynamicTrafficClient != null && dynamicTrafficClient.State == CommunicationState.Opened)
        {
          dynamicTrafficClient.Close();
        }
      }
      return result;
    }

    #endregion

    #region xml utils
    public static XmlNode AddNode(XmlNode parentNode, string sChildNodeName)
    {
      XmlNode childNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
      parentNode.AppendChild(childNode);
      return childNode;
    }

    // ************************************************************************************** //

    public static void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
      node.Attributes.Append(attribute);
      attribute.Value = sAttributeValue;
    }
    #endregion

    private DynamicTrafficClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
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

      wsHttpBinding.Security.Mode = SecurityMode.Message;
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

      return new DynamicTrafficClient(wsHttpBinding, endpointAddressBuilder.ToEndpointAddress());
    }
  }
}
