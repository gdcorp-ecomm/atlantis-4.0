using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Name { get { return "null"; } }

    public string GetPlaceHolderContent(string name, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string errorMessage = string.Format("Unknown place holder type \"{0}\".", name);
      
      debugContextErrors.Add(errorMessage);
      ErrorLogger.LogException(errorMessage, "RenderPlaceHolderContent", data);

      return string.Empty;
    }
  }
}
