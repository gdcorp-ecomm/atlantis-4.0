using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandlerFactory
  {
    IPlaceHolderHandler ConstructHandler(string type, string markup, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer);
  }
}
