using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNu : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50401, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50420, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50420, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50422, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50424, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50426, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NU"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }

    public override int MinRenewalLength
    {
      get { return 2; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 48; }
    }
  }
}
