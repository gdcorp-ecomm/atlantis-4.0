using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotBiz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11301, 11302, 11303, 11304, 11305, 11306, 11307, 11308, 11309, 11310 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 960, 60001, 60011, 60321, 60021, 60331, 60341, 60351, 60361, 60031 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 961, 60002, 60012, 60322, 60022, 60332, 60342, 60352, 60362, 60032 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11332, 60003, 60013, 60323, 60023, 60333, 60343, 60353, 60363, 60033 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11334, 60004, 60014, 60324, 60024, 60334, 60344, 60354, 60364, 60034 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11336, 60005, 60015, 60325, 60025, 60335, 60345, 60355, 60365, 60035 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11311, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 964, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 965, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11341, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11342, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11343, 11322, 11323, 11324, 11325, 11326, 11327, 11328, 11329 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11312, 11313, 11314, 11315, 11316, 11317, 11318, 11319, 11320, 11321 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 962, 70001, 70011, 70321, 70021, 70331, 70341, 70351, 70361, 70031 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 963, 70002, 70012, 70322, 70022, 70332, 70342, 70352, 70362, 70032 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 21332, 70003, 70013, 70323, 70023, 70333, 70343, 70353, 70363, 70033 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 21334, 70004, 70014, 70324, 70024, 70334, 70344, 70354, 70364, 70034 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 21336, 70005, 70015, 70325, 70025, 70335, 70345, 70355, 70365, 70035 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19013, 19000, 19006, 0, 19007, 0, 0, 0, 0, 19008 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.ExpiredAuctionReg, new StaticDotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "BIZ"; }
    }
  }
}
