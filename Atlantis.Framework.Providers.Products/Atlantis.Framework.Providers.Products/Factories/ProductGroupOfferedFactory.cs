using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Products.Handlers;

namespace Atlantis.Framework.Providers.Products.Factories
{
  internal static class ProductGroupOfferedFactory
  {
    private const int Office365 = 99;

    public static IProductGroupOfferedHandler GetHandler(IProviderContainer container, int productGroupType)
    {
      IProductGroupOfferedHandler handler;

      switch (productGroupType)
      {
        case Office365:
          handler = new O365ProductGroupOfferedHandler(container);
          break;
        default:
          handler = new ProductGroupOfferedHandler(container);
          break;
      }

      return handler;
    }
  }
}
