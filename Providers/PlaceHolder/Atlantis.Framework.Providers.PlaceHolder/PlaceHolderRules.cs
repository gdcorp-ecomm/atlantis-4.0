using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class PlaceHolderRules
  {
    private const string PLACEHOLDER_LIMIT_KEY = "Atlantis.Framework.Providers.PlaceHolder.PlaceHolderRules.Limit";
    private const int PLACEHOLDER_LIMIT_DEFAULT = 10;

    internal static int PlaceHolderLimit(IProviderContainer providerContainer)
    {
      return providerContainer.GetData(PLACEHOLDER_LIMIT_KEY, PLACEHOLDER_LIMIT_DEFAULT);
    }
  }
}
