using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.InstantStoreImages.Interface;
using Atlantis.Framework.InstantStoreImages.Impl.QuickSetup;
using Atlantis.Framework.Nimitz;
using System.Xml;

namespace Atlantis.Framework.InstantStoreImages.Impl
{
  public class InstantStoreImagesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      InstantStoreImagesRequestData getBalance = (InstantStoreImagesRequestData)requestData;
      InstantStoreImageResponseData oResponseData = null;
      try
      {
        QuickSetup.QuickSetup oSvc = new QuickSetup.QuickSetup();
        oSvc.Url = ((WsConfigElement)config).WSURL;
        oSvc.Timeout = (int)getBalance.RequestTimeout.TotalMilliseconds;
        string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);
        string username=string.Empty;
        string password=string.Empty;
        bool isAuth= GetConnectionCredentials(nimitzAuthXml,out username,out password);
        oSvc.Credentials = new System.Net.NetworkCredential(username, password);
        //BackgroundImageQuickSetupModel[] results = oSvc.GetBackgroundImages();
        ThemeQuickSetupModel[] results= oSvc.GetThemes();
        List<ImageData> resultData = new List<ImageData>();
        foreach (ThemeQuickSetupModel currentModel in results)
        {
          resultData.Add(new ImageData(currentModel.BackgroundId, currentModel.Src, currentModel.ThumbnailSrc,currentModel.DefaultTitle,
            currentModel.DefaultDescription,currentModel.Categories));
        }
        oResponseData = new InstantStoreImageResponseData(resultData);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new InstantStoreImageResponseData(requestData, exAtlantis);
      }
      return (IResponseData) oResponseData;
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
