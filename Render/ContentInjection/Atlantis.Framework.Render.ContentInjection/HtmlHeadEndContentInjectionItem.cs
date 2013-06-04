
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection
{
  public class HtmlHeadEndContentInjectionItem : IContentInjectionItem
  {
    private static readonly Regex _placeHolderRegex = new Regex(@"</head>", RegexOptions.Compiled | RegexOptions.Singleline);

    public Regex PlaceHolderRegex
    {
      get { return _placeHolderRegex; }
    }

    public ContentInjectionPosition Position
    {
      get { return ContentInjectionPosition.Before; }
    }

    public string InjectionContent { get; private set; }

    public HtmlHeadEndContentInjectionItem(string html)
    {
      InjectionContent = html;
    }
  }
}