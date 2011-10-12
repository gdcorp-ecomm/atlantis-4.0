using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotCn
{
  public class DotNetDotCn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14301, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14330, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14330, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14332, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14334, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14336, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14311, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14340, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14340, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14341, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14342, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14343, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14312, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24330, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24330, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24332, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24334, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24336, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.CN"; }
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
