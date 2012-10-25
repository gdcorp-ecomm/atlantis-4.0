using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WSTService.Impl.WSTServiceWS;
using Atlantis.Framework.WSTService.Interface;

namespace Atlantis.Framework.WSTService.Impl
{
  public class WSTTemplateInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData;

      try
      {
        WSTTemplateInfoRequestData wstTemplateInfoRequestData = (WSTTemplateInfoRequestData)requestData;
        WsConfigElement wsConfig = (WsConfigElement)configElement;

        using (WSTServiceWS.WSTService wstServiceClient = new WSTServiceWS.WSTService())
        {
          wstServiceClient.Url = wsConfig.WSURL;
          wstServiceClient.Timeout = (int)Math.Truncate(wstTemplateInfoRequestData.RequestTimeout.TotalMilliseconds);

          ThemeInfo[] themeInfo = null;
          if (wstTemplateInfoRequestData.CategoryId > 0)
          {
            themeInfo = wstServiceClient.GetActiveWSTThemesByCategory(wstTemplateInfoRequestData.CategoryId);
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
      }
      catch (Exception ex)
      {
        responseData = new WSTTemplateInfoResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}