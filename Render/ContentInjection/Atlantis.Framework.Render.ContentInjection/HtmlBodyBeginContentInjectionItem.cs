using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection
{
  public class HtmlBodyBeginContentInjectionItem : IContentInjectionItem
  {
    private static readonly Regex _placeHolderRegex = new Regex(@"<body\s*[\w\s""=:;#-]*>", RegexOptions.Compiled | RegexOptions.Singleline);

    public Regex PlaceHolderRegex
    {
      get { return _placeHolderRegex; }
    }

    public ContentInjectionPosition Position
    {
      get { return ContentInjectionPosition.After; }
    }

    public string InjectionContent { get; private set; }

    public HtmlBodyBeginContentInjectionItem(string html)
    {
      InjectionContent = html;
    }
  }
}