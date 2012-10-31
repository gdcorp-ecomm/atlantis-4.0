using System;
using System.Reflection;
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
      dynamic model = null;

      try
      {
        ITemplateDataSourceProvider templateDataSourceProvider = TemplateDataSourceProviderFactory.GetInstance(templatePlaceHolder.DataSource, providerContainer);
        if (templateDataSourceProvider != null)
        {
          model = templateDataSourceProvider.GetDataSource(templatePlaceHolder.DataSource);
        }
      }
      catch(Exception ex)
      {
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to get model for template. Exception: {0}", ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
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
