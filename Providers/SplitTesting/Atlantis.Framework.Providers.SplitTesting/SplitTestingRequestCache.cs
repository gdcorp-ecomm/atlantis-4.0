using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingRequestCache
  {
    readonly IProviderContainer _container;
    readonly Lazy<string> _cacheKey;
    readonly Lazy<ISiteContext> _siteContext;
    readonly Lazy<IShopperContext> _shopperContext;

    internal SplitTestingRequestCache(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => _container.Resolve<IShopperContext>());
      _cacheKey = new Lazy<string>(LoadCacheKeyName);
    }

    public string LoadCacheKeyName()
    {
      var shopperId = _shopperContext.Value.ShopperId.ToString(CultureInfo.InvariantCulture);
      if (string.IsNullOrEmpty(shopperId))
      {
        shopperId = "0";
      }

      return "SplitTestingRequestCache" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture) + "_" + shopperId;
    }

    public ISet<string> GetCacheValues()
    {
      var result = new HashSet<string>();
      if (HttpContext.Current != null)
      {
        result = HttpContext.Current.Session[_cacheKey.Value] as HashSet<string> ?? result;
      }
      return result;
    }

    public void AddCacheValues(string value)
    {
        if (HttpContext.Current != null)
        {
          var cachedValues = HttpContext.Current.Session[_cacheKey.Value] as HashSet<string>;
          if (cachedValues == null)
          {
            cachedValues = new HashSet<string>();
            HttpContext.Current.Session[_cacheKey.Value] = cachedValues;
          }
          cachedValues.Add(value);
        }
     }

    public void ClearCache() { HttpContext.Current.Session.Remove(_cacheKey.Value); }

 }
}
