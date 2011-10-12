using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotGs
{
  public class DotGs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40301, 40302, 40303, 40304, 40305, 40306, 40307, 40308, 40309, 40310 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50301, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50320, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50320, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50322, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50324, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50326, 50302, 50303, 50304, 50305, 50306, 50307, 50308, 50309, 50310 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "GS"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }
  }
}
