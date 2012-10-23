using System;
using System.Web;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class CdsTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      string template;

      try
      {
        ISiteContext siteContext = providerContainer.Resolve<ISiteContext>();
        IShopperContext shopperContext = providerContainer.Resolve<IShopperContext>();
        ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);

        CDSRequestData requestData = new CDSRequestData(shopperContext.ShopperId,
                                                        HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : string.Empty,
                                                        string.Empty,
                                                        siteContext.Pathway,
                                                        siteContext.PageCount,
                                                        templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer));

        CDSResponseData responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, 424);

        template = responseData.ResponseData;
      }
      catch (Exception ex)
      {
        // TODO: Log silent and return an empty string?
        throw new Exception(string.Format("Unable to retreive CDS template. Key: {0}, Exception: {1}", templateSource.RequestKey, ex.Message));
      }

      return template;
    }
  }
}