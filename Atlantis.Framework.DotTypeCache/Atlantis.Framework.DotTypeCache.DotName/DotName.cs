using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotName
{
  public class DotName : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11711, 11712, 11713, 11714, 11715, 11716, 11717, 11718, 11719, 11720 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 958, 60121, 60131, 60471, 60141, 60481, 60491, 60501, 60511, 60151 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 959, 60122, 60132, 60472, 60142, 60482, 60492, 60502, 60512, 60152 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11532, 60123, 60133, 60473, 60143, 60483, 60493, 60503, 60513, 60153 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11534, 60124, 60134, 60474, 60144, 60484, 60494, 60504, 60514, 60154 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11536, 60125, 60135, 60475, 60145, 60485, 60495, 60505, 60515, 60155 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11750, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 998, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 999, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11747, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11748, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11749, 11751, 11752, 11753, 11754, 11755, 11756, 11757, 11758 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11721, 11722, 11723, 11724, 11725, 11726, 11727, 11728, 11729, 11730 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 996, 70121, 70131, 70471, 70141, 70481, 70491, 70501, 70511, 70151 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 997, 70122, 70132, 70472, 70142, 70482, 70492, 70502, 70512, 70152 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11741, 70123, 70133, 70473, 70143, 70483, 70493, 70503, 70513, 70153 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11743, 70124, 70134, 70474, 70144, 70484, 70494, 70504, 70514, 70154 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11745, 70125, 70135, 70475, 70145, 70485, 70495, 70505, 70515, 70155 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19015, 19170, 19171, 0, 19172, 0, 0, 0, 0, 19173 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "NAME"; }
    }
  }
}
