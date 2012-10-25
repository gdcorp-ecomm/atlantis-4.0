using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WSTService.Impl.WSTServiceWS;
using Atlantis.Framework.WSTService.Interface;

namespace Atlantis.Framework.WSTService.Impl
{
  public class WSTTemplateInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      WSTServiceWS.WSTService wstServiceClient = null;
      IResponseData responseData;

      try
      {
        WSTTemplateInfoRequestData wstTemplateInfoRequestData = (WSTTemplateInfoRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        wstServiceClient = new WSTServiceWS.WSTService();
        wstServiceClient.Url = wsConfig.WSURL;
        wstServiceClient.Timeout = (int)Math.Truncate(wstTemplateInfoRequestData.RequestTimeout.TotalMilliseconds);

        ThemeInfo[] themeInfo = null;
        if (wstTemplateInfoRequestData.CategoryID > 0)
        {
          themeInfo = wstServiceClient.GetActiveWSTThemesByCategory(wstTemplateInfoRequestData.CategoryID);
        }
        else
        {
          themeInfo = wstServiceClient.GetAllActiveWSTThemes();
        }

        List<WSTTemplateInfo> templates = new List<WSTTemplateInfo>();
        foreach (ThemeInfo theme in themeInfo)
        {
          templates.Add(new WSTTemplateInfo(theme.CategoryId,
                                      theme.ThemeId,
                                      theme.ThemeLocation,
                                      theme.ThemeName,
                                      theme.ThemeThumbnailUrl,
                                      theme.ThemeUrl));
        }
        responseData = new WSTTemplateInfoResponseData(templates);
      }
      catch (Exception ex)
      {
        responseData = new WSTTemplateInfoResponseData(oRequestData, ex);
      }
      finally
      {
        if (wstServiceClient != null)
        {
          wstServiceClient.Dispose();
        }
      }

      return responseData;
    }
  }
}