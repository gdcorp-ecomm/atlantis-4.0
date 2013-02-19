using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotAg : StaticDotType
  {
    public override string DotType
    {
      get { return "AG"; }
    }

    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier tier0 = new StaticDotTypeTier(0, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      StaticDotTypeTier tier6to20 = new StaticDotTypeTier(6, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      StaticDotTypeTier tier21to49 = new StaticDotTypeTier(21, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      StaticDotTypeTier tier50to100 = new StaticDotTypeTier(50, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      StaticDotTypeTier tier101to200 = new StaticDotTypeTier(101, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      StaticDotTypeTier tier201andup = new StaticDotTypeTier(201, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { tier0, tier6to20, tier21to49, tier50to100, tier101to200, tier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier tier0 = new StaticDotTypeTier(0, new int[] { 50001, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      StaticDotTypeTier tier6to20 = new StaticDotTypeTier(6, new int[] { 50020, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      StaticDotTypeTier tier21to49 = new StaticDotTypeTier(21, new int[] { 50020, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      StaticDotTypeTier tier50to100 = new StaticDotTypeTier(50, new int[] { 50022, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      StaticDotTypeTier tier101to200 = new StaticDotTypeTier(101, new int[] { 50024, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      StaticDotTypeTier tier201andup = new StaticDotTypeTier(201, new int[] { 50026, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { tier0, tier6to20, tier21to49, tier50to100, tier101to200, tier201andup });
      return result;
    }
  }
}
