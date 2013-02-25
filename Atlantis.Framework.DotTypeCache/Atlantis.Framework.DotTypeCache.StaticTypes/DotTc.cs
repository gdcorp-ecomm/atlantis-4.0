using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotTc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40651, 40652, 40653, 40654, 40655, 40656, 40657, 40658, 40659, 40660 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50651, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50670, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50670, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50672, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50674, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50676, 50652, 50653, 50654, 50655, 50656, 50657, 50658, 50659, 50660 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
