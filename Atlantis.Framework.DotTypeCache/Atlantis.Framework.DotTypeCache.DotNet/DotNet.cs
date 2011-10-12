using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNet
{
  public class DotNet : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12001, 12002, 12003, 12004, 12005, 12006, 12007, 12008, 12009, 12010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 978, 60161, 60171, 60521, 60181, 60531, 60541, 60551, 60561, 60191 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 979, 60162, 60172, 60522, 60182, 60532, 60542, 60552, 60562, 60192 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12032, 60163, 60173, 60523, 60183, 60533, 60543, 60553, 60563, 60193 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12034, 60164, 60174, 60524, 60184, 60534, 60544, 60554, 60564, 60194 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12036, 60165, 60175, 60525, 60185, 60535, 60545, 60555, 60565, 60195 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12011, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 982, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 983, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12041, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12042, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12043, 12022, 12023, 12024, 12025, 12026, 12027, 12028, 12029 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12012, 12013, 12014, 12015, 12016, 12017, 12018, 12019, 12020, 12021 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 980, 70161, 70171, 70521, 70181, 70531, 70541, 70551, 70561, 70191 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 981, 70162, 70172, 70522, 70182, 70532, 70542, 70552, 70562, 70192 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22032, 70163, 70173, 70523, 70183, 70533, 70543, 70553, 70563, 70193 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22034, 70164, 70174, 70524, 70184, 70534, 70544, 70554, 70564, 70194 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22036, 70165, 70175, 70525, 70185, 70535, 70545, 70555, 70565, 70195 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19018, 19228, 19229, 0, 19230, 0, 0, 0, 0, 19231 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "NET"; }
    }
  }
}
