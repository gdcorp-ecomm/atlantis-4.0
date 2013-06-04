using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.ContentInjection
{
  internal class NullContentInjectionContext : IContentInjectionContext
  {
    private static readonly IList<IContentInjectionItem> _emptyContentInjectionItems = new List<IContentInjectionItem>(0);

    public bool IsValid
    {
      get { return false; }
    }

    public IEnumerable<IContentInjectionItem> ContentInjectionItems
    {
      get { return _emptyContentInjectionItems; }
    }

    internal NullContentInjectionContext()
    {
      AtlantisException aex = new AtlantisException("NullContentInjectionContext.Constructor()",
                                                     "0",
                                                     "IContentInjectionContext cannot be null",
                                                     "Render Handler: ContentInjectionRenderHandler",
                                                     null,
                                                     null);

      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
