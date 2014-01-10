using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class ShopperIdAndStatus : ProviderBase
  {
    public string ShopperId { get; private set; }
    public ShopperStatusType Status { get; private set; }

    private readonly Lazy<IDebugContext> _debugContext;

    internal ShopperIdAndStatus(IProviderContainer container)
      : base(container)
    {
      ShopperId = String.Empty;
      Status = ShopperStatusType.Public;
      _debugContext = new Lazy<IDebugContext>(LoadDebugContext);
   }

    private IDebugContext LoadDebugContext()
    {
      IDebugContext debug;
      Container.TryResolve(out debug);
      return debug;
    }

    internal void SetShopperAndStatus(string shopperId, ShopperStatusType status, string source)
    {
      ShopperId = shopperId ?? string.Empty;
      Status = status;

      AtlantisExceptionWebState.ShopperId = ShopperId;

      if (_debugContext.Value != null)
      {
        source = source ?? string.Empty;
        var shopperIdKey = "ShopperContext.ShopperId." + source;
        var shopperStatusKey = "ShopperContext.Status." + source;
        
        _debugContext.Value.LogDebugTrackingData(shopperIdKey, ShopperId);
        _debugContext.Value.LogDebugTrackingData(shopperStatusKey, Status.ToString());
      }
    }
  }
}
