using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotSc
{
  public class DotSc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50451, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50470, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50470, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50472, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50474, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50476, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "SC"; }
    }
  }
}
