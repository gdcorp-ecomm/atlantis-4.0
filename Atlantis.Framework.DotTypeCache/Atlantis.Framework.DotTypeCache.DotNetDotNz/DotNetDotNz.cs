using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotNz
{
  public class DotNetDotNz : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13001, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13030, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13030, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13032, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13034, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13036, 13002, 13003, 13004, 13005, 13006, 13007, 13008, 13009, 13010 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13011, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13040, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13040, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13041, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13042, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13043, 13022, 13023, 13024, 13025, 13026, 13027, 13028, 13029 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13012, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 23030, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 23030, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23032, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23034, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23036, 13013, 13014, 13015, 13016, 13017, 13018, 13019, 13020, 13021 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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
  }
}
