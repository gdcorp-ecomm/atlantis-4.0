using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection.Tests
{
  class NullPlaceHolderRegexInjectionItem : IContentInjectionItem
  {
    public Regex PlaceHolderRegex { get { return null; } }

    public ContentInjectionPosition Position { get { return ContentInjectionPosition.Replace; } }

    public string InjectionContent { get { return "Hello World"; } }
  }
}
