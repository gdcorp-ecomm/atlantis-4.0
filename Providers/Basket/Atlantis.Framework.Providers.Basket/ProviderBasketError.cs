using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public class ProviderBasketError : IBasketError
  {
    public string Number { get; internal set; }
    public string Description { get; internal set; }
  }
}
