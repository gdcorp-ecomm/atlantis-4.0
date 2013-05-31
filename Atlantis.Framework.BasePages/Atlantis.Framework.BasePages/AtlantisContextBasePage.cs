using System.IO;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web.UI;

namespace Atlantis.Framework.BasePages
{
  public abstract class AtlantisContextBasePage : Page
  {
    private static readonly Regex _lineBreaksRegex = new Regex(@"(\s*(\r)?\n\s*)+", RegexOptions.Compiled);

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

    private void RenderPage(HtmlTextWriter writer)
    {
      if (StripWhitespaceOnRender)
      {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
        {
          base.Render(htmlwriter);

          string modifiedContent = _lineBreaksRegex.Replace(htmlwriter.InnerWriter.ToString(), "\n").Trim();

          writer.Write(modifiedContent);
        }
      }
      else
      {
        base.Render(writer);
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      RenderPage(writer);
    }
  }
}
