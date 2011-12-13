using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.InstantStoreSetup.Interface;
using Atlantis.Framework.InstantStoreSetup.Impl.QuickSetup;
using Atlantis.Framework.Nimitz;
using System.Xml;

namespace Atlantis.Framework.InstantStoreSetup.Impl
{
  public class InstantStoreSetupRequest : IRequest
  {
    //username: ssdev_internal
    //password: ***REMOVED***
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      InstantStoreSetupRequestData stupAccount = (InstantStoreSetupRequestData)requestData;
      InstantStoreSetupResponseData oResponseData = null;
      try
      {
        QuickSetup.QuickSetup oSvc = new QuickSetup.QuickSetup();
        oSvc.Url = ((WsConfigElement)config).WSURL;
        oSvc.Timeout = (int)stupAccount.RequestTimeout.TotalMilliseconds;
        string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);
        string username = string.Empty;
        string password = string.Empty;
        bool isAuth = GetConnectionCredentials(nimitzAuthXml, out username, out password);
        oSvc.Credentials = new System.Net.NetworkCredential(username, password);
        List<SimpleSiteQuickSetupModel> storesToSetup = new List<SimpleSiteQuickSetupModel>();
        foreach (InstantStoreSetupInfo currentInfo in stupAccount.StoresToSetup)
        {
          if (!string.IsNullOrEmpty(currentInfo.OrionAccountUID))
          {
            SimpleSiteQuickSetupModel omodel = new SimpleSiteQuickSetupModel();
            omodel.BackgroundId = currentInfo.BackgroundID;
            omodel.CategoryIds = currentInfo.CategoryID;
            omodel.DomainName = currentInfo.DomainName;
            omodel.EmailAddress = currentInfo.EmailHash;
            omodel.OrionAccountUid = currentInfo.OrionAccountUID;
            omodel.SiteDescription = currentInfo.SiteDescription;
            omodel.SiteTitle = currentInfo.SiteTitle;
            omodel.PromoCode = currentInfo.PromoCode;
            storesToSetup.Add(omodel);
          }
        }
        if (storesToSetup.Count >= 1)
        {
          oSvc.SetupApplication(storesToSetup.ToArray());
          oResponseData = new InstantStoreSetupResponseData();
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new InstantStoreSetupResponseData(requestData, exAtlantis);
      }
      catch (System.Exception ex)
      {
        oResponseData = new InstantStoreSetupResponseData(requestData, new AtlantisException(requestData, "SetupApplication", string.Empty, ex.Message, ex.StackTrace, string.Empty));
      }
      return (IResponseData)oResponseData;
    }

    private bool GetConnectionCredentials(string nimitzAuthXml, out string authName, out string authToken)
    {
      bool success = false;

      authName = string.Empty;
      authToken = string.Empty;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(nimitzAuthXml);

      XmlNode authNameNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode authTokenNode = xdoc.SelectSingleNode("Connect/Password");

      if (authNameNode != null && authTokenNode != null)
      {

        authName = authNameNode.InnerText;
        authToken = authTokenNode.InnerText;
        success = true;
      }

      return success;
    }
  }

}
