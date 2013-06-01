using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Pipeline
{
  public class RenderPipelineBasePage : Page
  {
    private readonly RenderPipelineManager _renderPipelineManager = new RenderPipelineManager();

    private ISiteContext _siteContext;
    protected virtual ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>()); }
    }

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get { return _shopperContext ?? (_shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>()); }
    }

    private void RenderPage(HtmlTextWriter writer)
    {
      if (_renderPipelineManager.RenderHandlerCount > 0)
      {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
        {
          base.Render(htmlwriter);

          IRenderContent renderContent = new BasePageRenderContent(htmlwriter.InnerWriter.ToString());

          IProcessedRenderContent processedRenderContent = _renderPipelineManager.RenderContent(renderContent, HttpProviderContainer.Instance);

          writer.Write(processedRenderContent.Content);
        }
      }
      else
      {
        base.Render(writer);
      }
    }

    protected void AddRenderHandlers(IRenderHandler[] renderHandlers)
    {
      _renderPipelineManager.AddRenderHandler(renderHandlers);
    }

    protected void AddRenderHandlers(IEnumerable<IRenderHandler> renderHandlers)
    {
      _renderPipelineManager.AddRenderHandler(renderHandlers.ToArray());
    }

    protected void ProcessTemplate(IRenderTemplateContent renderTemplateContent)
    {
      RenderTemplateManager.ProcessTemplate(renderTemplateContent);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      RenderPage(writer);
    }
  }
}
