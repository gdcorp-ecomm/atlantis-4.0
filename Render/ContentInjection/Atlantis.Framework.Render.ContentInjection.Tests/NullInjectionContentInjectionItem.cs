using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection.Tests
{
  public class NullInjectionContentInjectionItem : IContentInjectionItem
  {
    public Regex PlaceHolderRegex { get { return new Regex(@"<body\s*[\w\s""=]*>", RegexOptions.Compiled | RegexOptions.Singleline); } }

    public ContentInjectionPosition Position { get { return ContentInjectionPosition.Replace; } }

    public string InjectionContent { get { return null; } }
  }
}
