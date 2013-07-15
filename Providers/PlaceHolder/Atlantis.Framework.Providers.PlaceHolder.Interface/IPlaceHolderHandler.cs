using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandler
  {
    string Type { get; }

    string GetPlaceHolderContent(string type, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer);
  }
}
