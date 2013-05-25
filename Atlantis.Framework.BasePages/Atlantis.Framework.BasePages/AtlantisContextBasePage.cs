using System.IO;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web.UI;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.BasePages
{
  public abstract class AtlantisContextBasePage : Page
  {
    private readonly RenderPipelineManager _renderPipelineManager = new RenderPipelineManager();

    public static bool StripWhitespaceOnRenderDefault = false;

    private bool _stripWhitespaceOnRender = StripWhitespaceOnRenderDefault;
    protected bool StripWhitespaceOnRender
    {
      get { return _stripWhitespaceOnRender; }
      set { _stripWhitespaceOnRender = value; }
    }

    private ISiteContext _siteContext;
    protected virtual ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }

    private void StripWhiteSpaceCheck()
    {
      if (StripWhitespaceOnRender)
      {
        _renderPipelineManager.AddRenderHandler(new StripWhiteSpaceRenderHandler());
      }
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

    protected void AddRenderHandler(params IRenderHandler[] renderHandlers)
    {
      _renderPipelineManager.AddRenderHandler(renderHandlers);
    }

    protected void AddRenderHandler(IRenderHandler renderHandler)
    {
      _renderPipelineManager.AddRenderHandler(renderHandler);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      StripWhiteSpaceCheck();
      RenderPage(writer);
    }
  }
}
