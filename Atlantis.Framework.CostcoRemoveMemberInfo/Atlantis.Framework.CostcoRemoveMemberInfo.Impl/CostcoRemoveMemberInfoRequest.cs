using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using System.Xml;
using Atlantis.Framework.CostcoRemoveMemberInfo.Interface;
using Atlantis.Framework.CostcoRemoveMemberInfo.Impl.GdCostcoMembershipWS;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoRemoveMemberInfo.Impl
{
  public class CostcoRemoveMemberInfoRequest : IRequest
  {

    #region Constants
    private const int PrivateLabelId_GoDaddy = 1;
    #endregion

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      CostcoRemoveMemberInfoResponseData responseData;
      CostcoRemoveMemberInfoRequestData requestData = null;

      try
      {
        requestData = (CostcoRemoveMemberInfoRequestData)oRequestData;

        string response = string.Empty;
        using (Service1 ws = new Service1())
        {
          ws.Timeout = Convert.ToInt32(requestData.RequestTimeout.TotalMilliseconds);
          ws.Url = ((WsConfigElement)oConfig).WSURL;
          AddClientCertificate(ws, oConfig);
          response = ws.RemoveMember(requestData.ShopperID);

          bool success;
          string errMsg;

          _ParseResponse(response, out success, out errMsg);
          responseData = new CostcoRemoveMemberInfoResponseData(response, success, errMsg);
        }

      }
      catch (Exception ex)
      {
        
        if (oRequestData is CostcoRemoveMemberInfoRequestData)
        {
          responseData = new CostcoRemoveMemberInfoResponseData(requestData, ex);
        }
        else
        {
          responseData = new CostcoRemoveMemberInfoResponseData(oRequestData, ex);
        }
      }

      return responseData;
    }

    #endregion

    private static void _ParseResponse(string response, out bool success, out string description)
    {
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.LoadXml(response);

      //<RemoveMember>
      //  <Status>ERROR</Status>
      //  <Description>Valid non-Executive Goldstar Primary Membership.</Description>
      //</RemoveMember>

      XmlNode removeMemberNode = xmldoc.SelectSingleNode("/RemoveMember");
      if (removeMemberNode != null)
      {
        XmlNode statusNode = removeMemberNode.SelectSingleNode("Status");
        string status = statusNode != null ? statusNode.InnerText : String.Empty;
        success = String.Compare(status, "success", true) == 0;

        XmlNode descNode = removeMemberNode.SelectSingleNode("Description");
        description = descNode != null ? descNode.InnerText : String.Empty;
      }
      else
      {
        // serious error
        throw new XmlException("<RemoveMember> not found");
      }
    }

    private static void AddClientCertificate(HttpWebClientProtocol service, ConfigElement oConfig)
    {
      X509Certificate cert = GetCertificate(oConfig);
      if (cert != null)
      {
        service.ClientCertificates.Add(cert);
      }
    }

    private static X509Certificate GetCertificate(ConfigElement oConfig)
    {
      X509Certificate cert = null;
      string certificateName = oConfig.GetConfigValue("CertificateName");
      if (!string.IsNullOrEmpty(certificateName))
      {
        X509Store certStore = new X509Store(StoreLocation.LocalMachine);
        certStore.Open(OpenFlags.ReadOnly);
        X509CertificateCollection certs = certStore.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);
        if (certs.Count > 0)
        {
          cert = certs[0];
        }
      }
      return cert;
    }
  }
}
