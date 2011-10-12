using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotCn
{
  public class DotComDotCn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14201, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14230, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14230, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14232, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14234, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14236, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14211, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14240, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14240, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14241, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14242, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14243, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14212, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24230, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24230, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24232, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24234, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24236, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.CN"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }
  }
}
