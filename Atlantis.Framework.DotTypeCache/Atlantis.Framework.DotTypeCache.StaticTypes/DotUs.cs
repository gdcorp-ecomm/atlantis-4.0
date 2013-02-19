using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotUs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11601, 11602, 11603, 11604, 11605, 11606, 11607, 11608, 11609, 11610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 990, 60241, 60251, 60621, 60261, 60631, 60641, 60651, 60661, 60271 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 991, 60242, 60252, 60622, 60262, 60632, 60642, 60652, 60662, 60272 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11632, 60243, 60253, 60623, 60263, 60633, 60643, 60653, 60663, 60273 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11634, 60244, 60254, 60624, 60264, 60634, 60644, 60654, 60664, 60274 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11636, 60245, 60255, 60625, 60265, 60635, 60645, 60655, 60665, 60275 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11611, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 994, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 995, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11641, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11642, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11643, 11622, 11623, 11624, 11625, 11626, 11627, 11628, 11629 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11612, 11613, 11614, 11615, 11616, 11617, 11618, 11619, 11620, 11621 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 992, 70241, 70251, 70621, 70261, 70631, 70641, 70651, 70661, 70271 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 993, 70242, 70252, 70622, 70262, 70632, 70642, 70652, 70662, 70272 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 21632, 70243, 70253, 70623, 70263, 70633, 70643, 70653, 70663, 70273 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 21634, 70244, 70254, 70624, 70264, 70634, 70644, 70654, 70664, 70274 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 21636, 70245, 70255, 70625, 70265, 70635, 70645, 70655, 70665, 70275 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19016, 19252, 19253, 0, 19254, 0, 0, 0, 0, 19255 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.ExpiredAuctionReg, new StaticDotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "US"; }
    }
  }
}
