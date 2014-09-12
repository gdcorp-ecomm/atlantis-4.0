using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandlerContext
  {
    string Type { get; }

    string Markup { get; }

    string Data { get; }

    ICollection<string> DebugContextErrors { get; }

    IProviderContainer ProviderContainer { get; }
  }
}
