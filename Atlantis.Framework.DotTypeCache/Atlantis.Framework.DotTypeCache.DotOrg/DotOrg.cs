using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrg
{
  public class DotOrg : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12101, 12102, 12103, 12104, 12105, 12106, 12107, 12108, 12109, 12110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 984, 60201, 60211, 60571, 60221, 60581, 60591, 60601, 60611, 60231 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 985, 60202, 60212, 60572, 60222, 60582, 60592, 60602, 60612, 60232 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12132, 60203, 60213, 60573, 60223, 60583, 60593, 60603, 60613, 60233 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12134, 60204, 60214, 60574, 60224, 60584, 60594, 60604, 60614, 60234 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12136, 60205, 60215, 60575, 60225, 60585, 60595, 60605, 60615, 60235 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12111, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 988, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 989, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12141, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12142, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12143, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12112, 12113, 12114, 12115, 12116, 12117, 12118, 12119, 12120, 12121 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 986, 70201, 70211, 70571, 70221, 70581, 70591, 70601, 70611, 70231 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 987, 70202, 70212, 70572, 70222, 70582, 70592, 70602, 70612, 70232 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22132, 70203, 70213, 70573, 70223, 70583, 70593, 70603, 70613, 70233 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22134, 70204, 70214, 70574, 70224, 70584, 70594, 70604, 70614, 70234 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22136, 70205, 70215, 70575, 70225, 70585, 70595, 70605, 70615, 70235 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19019, 19236, 19237, 0, 19238, 0, 0, 0, 0, 19239 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "ORG"; }
    }
  }
}
