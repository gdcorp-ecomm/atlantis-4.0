using System.Collections.Generic;

namespace Atlantis.Framework.Render.ContentInjection
{
  public class ContentInjectionContext : IContentInjectionContext
  {
    public IEnumerable<IContentInjectionItem> ContentInjectionItems { get; private set; }

    public bool IsValid
    {
      get { return ContentInjectionItems != null; }
    }

    public ContentInjectionContext(IEnumerable<IContentInjectionItem> htmlInjectionRegions)
    {
      ContentInjectionItems = htmlInjectionRegions;
    }
  }
}