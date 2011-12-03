using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Atlantis.Framework.CostcoGetMemberInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CostcoGetMemberInfo.Impl
{
  class CostcoGetMemberInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CostcoGetMemberInfoRequestData request;
      CostcoGetMemberInfoResponseData response;
      string wsResponse = string.Empty;
      CostcoService.Service1 service = null;
      try
      {
        request = (CostcoGetMemberInfoRequestData)requestData;
        string wsURL = ((WsConfigElement)config).WSURL;
        service = new CostcoService.Service1();
        service.Url = wsURL;
        service.Timeout = (int)Math.Truncate(request.RequestTimeout.TotalMilliseconds);
        var cert = GetCertificate(config);
        if (cert != null)
        {
          service.ClientCertificates.Add(cert);
        }

        wsResponse = service.GetMemberInfo(request.ShopperID);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(wsResponse);
        XmlNode root = xmlDoc.DocumentElement;
        int levelId;
        XmlNode statusNode = null;
        XmlAttribute memberLevelAttr = null;
        if (root != null)
        {
          statusNode = root.SelectSingleNode("/MemberInfo/Status");
          memberLevelAttr = root.Attributes != null ? root.Attributes["memberLevel"] : null;
        }
        if (root != null && statusNode != null && statusNode.InnerText.Equals("SUCCESS", StringComparison.InvariantCultureIgnoreCase) && memberLevelAttr != null && int.TryParse(memberLevelAttr.Value, out levelId))
        {
          response = new CostcoGetMemberInfoResponseData(levelId);
        }
        else
        {
          throw new AtlantisException(requestData, "CostcoGetMemberInfoRequest", "Received a bad response.", string.Format("Response XML: {0}", wsResponse));
        }
      }
      catch (Exception ex)
      {
        throw new AtlantisException(requestData, "CostcoGetMemberInfoRequest", "Error invoking CostcoGetMemberInfoRequest", string.Format("Response XML: {0}", wsResponse), ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }
      return response;
    }

    private X509Certificate GetCertificate(ConfigElement oConfig)
    {
      X509Certificate cert = null;
      string certificateName = oConfig.GetConfigValue("CertificateName");
      if (!string.IsNullOrEmpty(certificateName))
      {
        X509Store certStore = new X509Store(StoreLocation.LocalMachine);
        certStore.Open(OpenFlags.ReadOnly);
        X509CertificateCollection certs = certStore.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);
        if (certs.Count > 0)
          cert = certs[0];
      }
      return cert;
    }
  }
}
