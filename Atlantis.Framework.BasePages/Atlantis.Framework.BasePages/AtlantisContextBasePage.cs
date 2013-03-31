using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Atlantis.Framework.BasePages
{
  public abstract class AtlantisContextBasePage : Page
  {
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

    public static bool StripWhitespaceOnRenderDefault = false;

    private bool _stripWhitespaceOnRender = StripWhitespaceOnRenderDefault;
    protected bool StripWhitespaceOnRender
    {
      get { return _stripWhitespaceOnRender; }
      set { _stripWhitespaceOnRender = value; }
    }

    private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"(\s*(\r)?\n\s*)+", RegexOptions.Compiled);

    protected override void Render(HtmlTextWriter writer)
    {
      if(StripWhitespaceOnRender)
      {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
        {
          base.Render(htmlwriter);
          string html = htmlwriter.InnerWriter.ToString();

          html = REGEX_LINE_BREAKS.Replace(html, "\n");

          writer.Write(html.Trim());
        }
      }
      else
      {
        base.Render(writer);
      }
    }
  }
}
