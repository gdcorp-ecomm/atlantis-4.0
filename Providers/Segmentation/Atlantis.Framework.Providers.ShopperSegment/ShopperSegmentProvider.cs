using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ShopperSegment.Interface;

namespace Atlantis.Framework.Providers.ShopperSegment
{
  public class ShopperSegmentProvider : ProviderBase, IShopperSegmentProvider
  {
    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;

    protected ShopperSegmentProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = Container.Resolve<ISiteContext>();
      _shopperContext = Container.Resolve<IShopperContext>();
    }

    private IShopperContext ShopperContext
    {
      get { return _shopperContext; }
    }

    public IEnumerable<int> GetShopperSegmentIds()
    {
      // TODO: Get information from the triplet for the shopper segment ids
      throw new NotImplementedException();
    }

    public int GetShopperSegmentId()
    {
      int returnValue = -1;

      // TODO: Get information from the triplet for the shopper segment id
      //return QaSpoofable.GetAppSetting(Container.Resolve<ISiteContext>(), "SEGMENTATION_VALUE");

      return returnValue;
    }

    public static string GetAppSetting(ISiteContext siteContext, string appSettingName)
    {
      string returnValue = string.Empty;

      if (siteContext.IsRequestInternal)
      {
        string spoofParam = "QA--" + appSettingName;
        string spoofValue = HttpContext.Current.Request[spoofParam];
        if (!string.IsNullOrEmpty(spoofValue))
        {
          returnValue = spoofValue;
        }
      }
      return returnValue;
    }
  }
}
