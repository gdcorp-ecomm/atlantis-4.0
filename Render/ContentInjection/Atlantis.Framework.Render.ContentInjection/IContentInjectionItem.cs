using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.ContentInjection
{
  public interface IContentInjectionItem
  {
    Regex PlaceHolderRegex { get; }

    ContentInjectionPosition Position { get; }
 
    string InjectionContent { get; }
  }
}