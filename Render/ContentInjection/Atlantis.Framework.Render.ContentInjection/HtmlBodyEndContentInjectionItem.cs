
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection
{
  public class HtmlBodyEndContentInjectionItem : IContentInjectionItem
  {
    private static readonly Regex _placeHolderRegex = new Regex(@"</body>", RegexOptions.Compiled | RegexOptions.Singleline);

    public Regex PlaceHolderRegex
    {
      get { return _placeHolderRegex; }
    }

    public ContentInjectionPosition Position
    {
      get { return ContentInjectionPosition.Before; }
    }

    public string InjectionContent { get; private set; }

    public HtmlBodyEndContentInjectionItem(string html)
    {
      InjectionContent = html;
    }
  }
}