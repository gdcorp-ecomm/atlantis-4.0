using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotMs
{
  public class DotMs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50351, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50370, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50370, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50372, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50374, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50376, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "MS"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }
  }
}
