using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSCThemeImages.Interface;

namespace Atlantis.Framework.QSCThemeImages.Impl
{
  public class QSCThemeImagesRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      QSCThemeImagesResponseData responseData = null;

      try
      {
        QSCThemeImagesRequestData cmsRequest = (QSCThemeImagesRequestData)requestData;
        QSCService.FOSService service = new QSCService.FOSService();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)cmsRequest.RequestTimeout.TotalMilliseconds;
        QSCService.template[] templates= service.getThemeInformation();
        List<ThemeData> themes = new List<ThemeData>();
        foreach (QSCService.template currentTemplate in templates)
        {
          ThemeData newTheme = new ThemeData(currentTemplate.backgroundID, currentTemplate.src, currentTemplate.thumbnailSrc);
          themes.Add(newTheme);
        }
        responseData = new QSCThemeImagesResponseData(themes);
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new QSCThemeImagesResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new QSCThemeImagesResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
