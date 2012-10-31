using System;
using System.Reflection;
using System.Web;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class CdsTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      string template = string.Empty;

      try
      {
        ISiteContext siteContext = providerContainer.Resolve<ISiteContext>();
        IShopperContext shopperContext = providerContainer.Resolve<IShopperContext>();
        ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);

        if (templateRequestKeyHandlerProvider != null)
        {
          CDSRequestData requestData = new CDSRequestData(shopperContext.ShopperId,
                                                          HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : string.Empty,
                                                          string.Empty,
                                                          siteContext.Pathway,
                                                          siteContext.PageCount,
                                                          templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer));

          CDSResponseData responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, 424);

          template = responseData.ResponseData;
        }
      }
      catch (Exception ex)
      {
        template = string.Empty;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to retreive CDS template. Key: {0}, Exception: {1}", templateSource.RequestKey, ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return template;
    }
  }
}