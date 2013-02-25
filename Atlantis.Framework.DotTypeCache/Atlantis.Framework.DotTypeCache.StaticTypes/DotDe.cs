using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotDe : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12601, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9500, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9501, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12632, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12634, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12636, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12611, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9502, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9503, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12641, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12642, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12643, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12612, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 19500, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 19501, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22632, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22634, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22636, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "DE"; }
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
