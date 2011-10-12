using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotAg
{
  public class DotAg : DotTypeStaticBase
  {
    public override string DotType
    {
      get { return "AG"; }
    }

    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier tier0 = new DotTypeTier(0, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      DotTypeTier tier6to20 = new DotTypeTier(6, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      DotTypeTier tier21to49 = new DotTypeTier(21, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      DotTypeTier tier50to100 = new DotTypeTier(50, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      DotTypeTier tier101to200 = new DotTypeTier(101, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });
      DotTypeTier tier201andup = new DotTypeTier(201, new int[] { 40001, 40002, 40003, 40004, 40005, 40006, 40007, 40008, 40009, 40010 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { tier0, tier6to20, tier21to49, tier50to100, tier101to200, tier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier tier0 = new DotTypeTier(0, new int[] { 50001, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      DotTypeTier tier6to20 = new DotTypeTier(6, new int[] { 50020, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      DotTypeTier tier21to49 = new DotTypeTier(21, new int[] { 50020, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      DotTypeTier tier50to100 = new DotTypeTier(50, new int[] { 50022, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      DotTypeTier tier101to200 = new DotTypeTier(101, new int[] { 50024, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });
      DotTypeTier tier201andup = new DotTypeTier(201, new int[] { 50026, 50002, 50003, 50004, 50005, 50006, 50007, 50008, 50009, 50010 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { tier0, tier6to20, tier21to49, tier50to100, tier101to200, tier201andup });
      return result;
    }
  }
}
