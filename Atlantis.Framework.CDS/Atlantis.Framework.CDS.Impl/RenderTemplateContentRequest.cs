using System;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Template;
using Atlantis.Framework.Render.Template.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Impl
{
  public class RenderTemplateContentRequest : IRequest
  {
    private const string APPLICATION_NAME_KEY = "ApplicationName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RenderTemplateContentResponseData responseData = null;

      CDSRequestData cdsRequestData = requestData as CDSRequestData;
      WsConfigElement wsConfig = (WsConfigElement)config;
      cdsRequestData.AppName = wsConfig.GetConfigValue(APPLICATION_NAME_KEY); //used to identify the App in the errorlog entry

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      
      try
      {
        string responseText = service.GetWebResponse();
        
        if (!string.IsNullOrEmpty(responseText))
        {
          ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseText);
          IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(contentVersion);

          responseData = new RenderTemplateContentResponseData(responseText, renderTemplateContent);
        }
        else
        {
          responseData = new RenderTemplateContentResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
        }
      }
      catch (Exception ex)
      {
        responseData = new RenderTemplateContentResponseData(cdsRequestData, ex);
      }

      return responseData;
    }
  }
}
