using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotCn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14261, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14290, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14290, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14291, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14292, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14293, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14262, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24280, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24280, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24282, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24284, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24286, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.CN"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 10; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 120; }
    }

  }
}
