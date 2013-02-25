using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotAt : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12241, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12242, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12243, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12212, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 22230, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 22230, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22232, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22234, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22236, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "AT"; }
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
