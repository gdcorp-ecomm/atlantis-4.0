using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.ShopperSegment.Interface
{
  public interface IShopperSegmentProvider
  {
    IEnumerable<int> GetShopperSegmentIds();

    int GetShopperSegmentId();
  }
}
