using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotNz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13001, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13030, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13030, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13032, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13034, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13036, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13011, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13040, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13040, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13041, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13042, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13043, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13012, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 23030, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 23030, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23032, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23034, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23036, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.NZ"; }
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
