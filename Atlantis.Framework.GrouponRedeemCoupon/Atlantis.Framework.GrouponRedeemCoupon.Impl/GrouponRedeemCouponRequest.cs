using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Atlantis.Framework.GrouponRedeemCoupon.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GrouponRedeemCoupon.Impl
{
  class GrouponRedeemCouponRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GrouponRedeemCouponRequestData request;
      GrouponRedeemCouponResponseData response = null;

      string wsResponse = string.Empty;
      InStoreCreditCouponService.Coupons service = null;
      try
      {
        request = (GrouponRedeemCouponRequestData) requestData;
        string wsURL = ((WsConfigElement) config).WSURL;
        service = new InStoreCreditCouponService.Coupons();
        service.Url = wsURL;
        service.Timeout = (int) Math.Truncate(request.RequestTimeout.TotalMilliseconds);

        var cert = GetCertificate(config);
        if (cert != null)
        {
          service.ClientCertificates.Add(cert);
        }

        wsResponse = service.Redeem(request.ShopperID, request.CouponCode);
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(wsResponse);
        XmlNode root = xmlDoc.SelectSingleNode("/result");

        if (root != null)
        {
          int status = -1;
          int.TryParse(root.Attributes["status"].Value, out status);

          if (status == 0)
          {
            int amount;
            int.TryParse(root.Attributes["amount"].Value, out amount);

            response = new GrouponRedeemCouponResponseData(status, amount, root.Attributes["currency"].Value);
          }
        }
        else
        {
          throw new AtlantisException(requestData, "GrouponRedeemCouponRequest", "Received a bad response.",
                                      string.Format("Response XML: {0}", wsResponse));
        }
      }
      catch (Exception ex)
      {
        throw new AtlantisException(requestData, "GrouponRedeemCouponRequest",
                                    "Error invoking GrouponRedeemCouponRequest",
                                    string.Format("Response XML: {0}", wsResponse), ex);
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
      var certificateName = oConfig.GetConfigValue("CertificateName");
      if (!string.IsNullOrEmpty(certificateName))
      {
        var certStore = new X509Store(StoreLocation.LocalMachine);
        certStore.Open(OpenFlags.ReadOnly);
        X509CertificateCollection certs = certStore.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);
        if (certs.Count > 0)
          cert = certs[0];
      }
      return cert;
    }
  }
}
