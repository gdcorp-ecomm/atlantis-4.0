using System.Collections.Generic;

namespace Atlantis.Framework.Render.ContentInjection
{
  public interface IContentInjectionContext
  {
    bool IsValid { get; }

    IEnumerable<IContentInjectionItem> ContentInjectionItems { get; }
  }
}