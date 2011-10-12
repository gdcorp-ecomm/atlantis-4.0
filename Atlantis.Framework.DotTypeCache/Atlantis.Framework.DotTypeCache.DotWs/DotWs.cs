using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotWs
{
  public class DotWs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11401, 11402, 11403, 11404, 11405, 11406, 11407, 11408, 11409, 11410 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 952, 60281, 60291, 60671, 60301, 60681, 60691, 60701, 60711, 60311 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 953, 60282, 60292, 60672, 60302, 60682, 60692, 60702, 60712, 60312 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11432, 60283, 60293, 60673, 60303, 60683, 60693, 60703, 60713, 60313 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11434, 60284, 60294, 60674, 60304, 60684, 60694, 60704, 60714, 60314 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11436, 60285, 60295, 60675, 60305, 60685, 60695, 60705, 60715, 60315 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11411, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 956, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 957, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11441, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11442, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11443, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11412, 11413, 11414, 11415, 11416, 11417, 11418, 11419, 11420, 11421 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 954, 70281, 70291, 70671, 70301, 70681, 70691, 70701, 70711, 70311 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 955, 70282, 70292, 70672, 70302, 70682, 70692, 70702, 70712, 70312 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 21432, 70283, 70293, 70673, 70303, 70683, 70693, 70703, 70713, 70313 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 21434, 70284, 70294, 70674, 70304, 70684, 70694, 70704, 70714, 70314 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 21436, 70285, 70295, 70675, 70305, 70685, 70695, 70705, 70715, 70315 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19014, 19260, 19261, 0, 19262, 0, 0, 0, 0, 19263 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "WS"; }
    }
  }
}
