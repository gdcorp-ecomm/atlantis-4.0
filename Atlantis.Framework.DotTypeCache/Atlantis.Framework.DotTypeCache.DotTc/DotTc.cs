using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotTc
{
  public class DotTc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50651, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50670, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50670, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50672, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50674, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50676, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "TC"; }
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
