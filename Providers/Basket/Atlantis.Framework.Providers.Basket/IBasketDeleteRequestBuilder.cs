using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  internal interface IBasketDeleteRequestBuilder
  {
    BasketDeleteRequestData BuildRequestData(string shopperId,
      IBasketDeleteRequest deleteRequest);
  }
}
