using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotInfo
{
  public class DotInfo : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 201, 202, 203, 204, 205, 206, 207, 208, 209, 210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 972, 60081, 60091, 60421, 60101, 60431, 60441, 60451, 60461, 60111 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 973, 60082, 60092, 60422, 60102, 60432, 60442, 60452, 60462, 60112 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 462, 60083, 60093, 60423, 60103, 60433, 60443, 60453, 60463, 60113 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 464, 60084, 60094, 60424, 60104, 60434, 60444, 60454, 60464, 60114 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 466, 60085, 60095, 60425, 60105, 60435, 60445, 60455, 60465, 60115 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 211, 121, 122, 123, 124, 125, 126, 127, 128 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 976, 121, 122, 123, 124, 125, 126, 127, 128 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 977, 121, 122, 123, 124, 125, 126, 127, 128 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 471, 121, 122, 123, 124, 125, 126, 127, 128 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 472, 121, 122, 123, 124, 125, 126, 127, 128 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 473, 121, 122, 123, 124, 125, 126, 127, 128 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 10201, 10202, 10203, 10204, 10205, 10206, 10207, 10208, 10209, 10210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 974, 70081, 70091, 70421, 70101, 70431, 70441, 70451, 70461, 70111 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 975, 70082, 70092, 70422, 70102, 70432, 70442, 70452, 70462, 70112 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 10462, 70083, 70093, 70423, 70103, 70433, 70443, 70453, 70463, 70113 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 10464, 70084, 70094, 70424, 70104, 70434, 70444, 70454, 70464, 70114 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 10466, 70085, 70095, 70425, 70105, 70435, 70445, 70455, 70465, 70115 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19011, 19156, 19157, 0, 19158, 0, 0, 0, 0, 19162 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "INFO"; }
    }
  }
}
