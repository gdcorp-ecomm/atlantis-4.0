using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14001, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14030, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14030, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14032, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14034, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14036, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14041, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14042, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14043, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24032, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24034, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24036, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CN"; }
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
