using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using System.Xml;
using Atlantis.Framework.CostcoSetMemberInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoSetMemberInfo.Impl
{
  public class CostcoSetMemberInfoRequest : IRequest
  {
    #region Constants
    private const int PrivateLabelId_GoDaddy = 1;
    #endregion

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      CostcoSetMemberInfoResponseData responseData;
      CostcoSetMemberInfoRequestData requestData = null;
      try
      {
        requestData = (CostcoSetMemberInfoRequestData)oRequestData;

        string response;
        if (requestData.ResellerId == PrivateLabelId_GoDaddy)
        {
          var ws = new GdCostcoMembershipWS.Service1();
          ws.Timeout = Convert.ToInt32(requestData.RequestTimeout.TotalMilliseconds);
          ws.Url = ((WsConfigElement)oConfig).WSURL;
          AddClientCertificate(ws, oConfig);
          response = ws.SetMember(requestData.ShopperID, requestData.CostcoMembershipId, String.Concat("<Validation zip=\"", requestData.ShopperPostalCode, "\"/>"));

          bool success;
          string errMsg;
          bool? discountDomainClub;
          bool? existingMember;
          int? memberLevel;
          _ParseResponse(response, out success, out errMsg, out memberLevel, out discountDomainClub, out existingMember);
          responseData = new CostcoSetMemberInfoResponseData(response, success, errMsg, memberLevel, discountDomainClub, existingMember);
        }
        else
        {
          throw new Exception("Invalid Private Label");
        }

      }
      catch (Exception ex)
      {
        if (oRequestData is CostcoSetMemberInfoRequestData)
        {
          string data = String.Concat("requestData.RequestTimeout(ms)=", requestData.RequestTimeout.Milliseconds, ", requestData.ResellerId=", requestData.ResellerId, ", requestData.ShopperPostalCode=", requestData.ShopperPostalCode, ", requestData.CostcoMembershipId=", requestData.CostcoMembershipId);
          throw BuildException(requestData, "RequestHandler", ex, data);
        }
        else
        {
          throw BuildException(oRequestData, "RequestHandler", ex, null);
        }
      }

      return responseData;
    }

    #endregion

    private static AtlantisException BuildException(RequestData requestData, string sourceFunction, Exception ex, string data)
    {
      return new AtlantisException(requestData, "CostcoSetMemberInfoRequest." + sourceFunction, ex.Message + Environment.NewLine + ex.StackTrace, data, ex);
    }

    private static void _ParseResponse(string response, out bool success, out string description, out int? memberLevel, out bool? discountDomainClub, out bool? existingMember)
    {
      var xmldoc = new XmlDocument();
      xmldoc.LoadXml(response);

      //<MemberInfo memberLevel="0" discountDomainClub="0" existingMember="0">
      //  <Status>ERROR</Status>
      //  <Description>Valid non-Executive Goldstar Primary Membership.</Description>
      //</MemberInfo>

      var memberInfoNode = xmldoc.SelectSingleNode("/MemberInfo");
      if (memberInfoNode != null)
      {
        XmlAttribute attr;
        string val;
        int iVal;

        attr = memberInfoNode.Attributes["memberLevel"];
        val = attr != null ? attr.InnerText : String.Empty;
        if (int.TryParse(val, out iVal))
        {
          memberLevel = iVal;
        }
        else
        {
          memberLevel = new Nullable<int>();
        }

        attr = memberInfoNode.Attributes["discountDomainClub"];
        val = attr != null ? attr.InnerText : String.Empty;
        ParseIntToNullableBool(val, out discountDomainClub);

        attr = memberInfoNode.Attributes["existingMember"];
        val = attr != null ? attr.InnerText : String.Empty;
        ParseIntToNullableBool(val, out existingMember);

        var statusNode = memberInfoNode.SelectSingleNode("Status");
        string status = statusNode != null ? statusNode.InnerText : String.Empty;
        success = String.Compare(status, "success", true) == 0;

        var descNode = memberInfoNode.SelectSingleNode("Description");
        description = descNode != null ? descNode.InnerText : String.Empty;

      }
      else
      {
        // serious error
        throw new XmlException("<MemberInfo> not found");
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

    private static void ParseIntToNullableBool(string intAsString, out bool ? b)
    {
      int iVal;
      if (int.TryParse(intAsString, out iVal))
      {
        switch (iVal)
        {
          case 0:
            b = false;
            break;
          case 1:
            b = true;
            break;
          default:
            b = new Nullable<bool>();
            break;
        }
      }
      else
      {
        b = new Nullable<bool>();
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
