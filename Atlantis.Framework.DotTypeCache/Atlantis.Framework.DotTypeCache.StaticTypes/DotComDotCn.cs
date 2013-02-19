using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotCn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14201, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14230, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14230, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14232, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14234, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14236, 14202, 14203, 14204, 14205, 14206, 14207, 14208, 14209, 14210 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14211, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14240, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14240, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14241, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14242, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14243, 14222, 14223, 14224, 14225, 14226, 14227, 14228, 14229 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14212, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24230, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24230, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24232, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24234, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24236, 14213, 14214, 14215, 14216, 14217, 14218, 14219, 14220, 14221 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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
  }
}
