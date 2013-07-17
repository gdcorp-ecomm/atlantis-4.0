using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get { return "null"; } }

    public string GetPlaceHolderContent(string type, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string errorMessage = string.Format("Unknown place holder type: \"{0}\".", type);
      
      debugContextErrors.Add(errorMessage);
      ErrorLogger.LogException(errorMessage, "NullPlaceHolderHandler.GetPlaceHolderContent()", data);

      return string.Empty;
    }
  }
}
