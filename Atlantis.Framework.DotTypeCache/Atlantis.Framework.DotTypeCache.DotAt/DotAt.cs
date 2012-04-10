using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotAt
{
  public class DotAt : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12211, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12241, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12242, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12243, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12212, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 22230, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 22230, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22232, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22234, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22236, 12213, 12214, 12215, 12216, 12217, 12218, 12219, 12220, 12221 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    public override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }

  }
}
