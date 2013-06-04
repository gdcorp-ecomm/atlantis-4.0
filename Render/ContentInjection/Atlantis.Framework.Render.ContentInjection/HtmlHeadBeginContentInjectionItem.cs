
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection.RenderHandlers
{
  public class HtmlHeadBeginContentInjectionItem : IContentInjectionItem
  {
    private static readonly Regex _placeHolderRegex = new Regex(@"<head\s*[\w\s""=:;#-]*>", RegexOptions.Compiled | RegexOptions.Singleline);

    public Regex PlaceHolderRegex
    {
      get { return _placeHolderRegex; }
    }

    public ContentInjectionPosition Position
    {
      get { return ContentInjectionPosition.After; }
    }

    public string InjectionContent { get; private set; }

    public HtmlHeadBeginContentInjectionItem(string html)
    {
      InjectionContent = html;
    }
  }
}