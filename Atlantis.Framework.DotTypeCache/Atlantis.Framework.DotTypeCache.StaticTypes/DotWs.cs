using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotWs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11401, 11402, 11403, 11404, 11405, 11406, 11407, 11408, 11409, 11410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 952, 60281, 60291, 60671, 60301, 60681, 60691, 60701, 60711, 60311 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 953, 60282, 60292, 60672, 60302, 60682, 60692, 60702, 60712, 60312 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11432, 60283, 60293, 60673, 60303, 60683, 60693, 60703, 60713, 60313 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11434, 60284, 60294, 60674, 60304, 60684, 60694, 60704, 60714, 60314 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11436, 60285, 60295, 60675, 60305, 60685, 60695, 60705, 60715, 60315 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11411, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 956, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 957, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11441, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11442, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11443, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11412, 11413, 11414, 11415, 11416, 11417, 11418, 11419, 11420, 11421 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 954, 70281, 70291, 70671, 70301, 70681, 70691, 70701, 70711, 70311 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 955, 70282, 70292, 70672, 70302, 70682, 70692, 70702, 70712, 70312 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 21432, 70283, 70293, 70673, 70303, 70683, 70693, 70703, 70713, 70313 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 21434, 70284, 70294, 70674, 70304, 70684, 70694, 70704, 70714, 70314 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 21436, 70285, 70295, 70675, 70305, 70685, 70695, 70705, 70715, 70315 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19014, 19260, 19261, 0, 19262, 0, 0, 0, 0, 19263 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.ExpiredAuctionReg, new StaticDotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "WS"; }
    }
  }
}
