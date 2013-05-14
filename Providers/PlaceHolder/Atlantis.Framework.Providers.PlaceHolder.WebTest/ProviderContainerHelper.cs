using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public static class ProviderContainerHelper
  {
    private static IProviderContainer _instance;
    public static IProviderContainer Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new MockProviderContainer();
          _instance.RegisterProvider<ISiteContext, MockSiteContext>();
          _instance.RegisterProvider<IShopperContext, MockShopperContext>();
          _instance.RegisterProvider<IManagerContext, MockManagerContext>();
          _instance.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
        }
        return _instance;
      }
    }
  }
}