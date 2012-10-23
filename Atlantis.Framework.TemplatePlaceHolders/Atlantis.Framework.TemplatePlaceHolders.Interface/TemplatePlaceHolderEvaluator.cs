using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class TemplatePlaceHolderEvaluator : ITemplatePlaceHolderEvaluator
  {
    private string GetTemplate(ITemplatePlaceHolder templatePlaceHolder, IProviderContainer providerContainer)
    {
      ITemplateSourceManager templateSourceManager = TemplateSourceManagerFactory.GetInstance(templatePlaceHolder.TemplateSource.Source);
      return templateSourceManager.GetTemplateSource(templatePlaceHolder.TemplateSource, providerContainer);
    }

    private dynamic GetModel(ITemplatePlaceHolder templatePlaceHolder, IProviderContainer providerContainer)
    {
      dynamic model;

      try
      {
        ITemplateDataSourceProvider templateDataSourceProvider = TemplateDataSourceProviderFactory.GetInstance(templatePlaceHolder.DataSource, providerContainer);
        model = templateDataSourceProvider.GetDataSource(templatePlaceHolder.DataSource);
      }
      catch(Exception ex)
      {
        throw new Exception(string.Format("Unable to get model for template. Exception: {0}", ex.Message), ex);
      }

      return model;
    }

    public string EvaluatePlaceHolder(ITemplatePlaceHolder templatePlaceHolder, IProviderContainer providerContainer)
    {
      string template = GetTemplate(templatePlaceHolder, providerContainer);
      dynamic model = GetModel(templatePlaceHolder, providerContainer);
      
      IRenderingEngine renderingEngine = RenderingEngineFactory.GetInstance(templatePlaceHolder.TemplateSource.Format);
      return renderingEngine.Render(template, model);
    }
  }
}
