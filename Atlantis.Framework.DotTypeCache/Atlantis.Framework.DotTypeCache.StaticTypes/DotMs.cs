using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotMs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40351, 40352, 40353, 40354, 40355, 40356, 40357, 40358, 40359, 40360 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50351, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50370, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50370, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50372, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50374, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50376, 50352, 50353, 50354, 50355, 50356, 50357, 50358, 50359, 50360 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
