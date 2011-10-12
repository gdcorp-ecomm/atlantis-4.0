using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCn
{
  public class DotCn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14001, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14030, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14030, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14032, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14034, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14036, 14002, 14003, 14004, 14005, 14006, 14007, 14008, 14009, 14010 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14011, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14041, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14042, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14043, 14022, 14023, 14024, 14025, 14026, 14027, 14028, 14029 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14012, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24032, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24034, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24036, 14013, 14014, 14015, 14016, 14017, 14018, 14019, 14020, 14021 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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
      get { return 5; }
    }
  }
}
