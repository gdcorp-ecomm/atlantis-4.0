using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Name { get { return "null"; } }

    public string GetPlaceHolderContent(string name, string data, IProviderContainer providerContainer)
    {
      ErrorLogger.LogException(string.Format("Unknown place holder \"{0}\".", name), "RenderPlaceHolderContent", data);

      return string.Empty;
    }
  }
}
