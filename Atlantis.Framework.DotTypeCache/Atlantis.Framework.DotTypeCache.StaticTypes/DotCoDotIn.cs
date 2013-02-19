using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCoDotIn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41000, 41001, 41002, 41003, 41004, 41005, 41006, 41007, 41008, 41009 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41030, 41039, 41045, 41063, 41051, 41069, 41075, 41081, 41087, 41057 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41031, 41040, 41046, 41064, 41052, 41070, 41076, 41082, 41088, 41058 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41021, 41041, 41047, 41065, 41053, 41071, 41077, 41083, 41089, 41059 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41023, 41042, 41048, 41066, 41054, 41072, 41078, 41084, 41090, 41060 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41025, 41043, 41049, 41067, 41055, 41073, 41079, 41085, 41091, 41061 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41010, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41032, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41033, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41027, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41028, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41029, 41011, 41012, 41013, 41014, 41015, 41016, 41017, 41018 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 51000, 51001, 51002, 51003, 51004, 51005, 51006, 51007, 51008, 51009 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51030, 51039, 51045, 51063, 51051, 51069, 51075, 51081, 51087, 51057 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51031, 51040, 51046, 51064, 51052, 51070, 51076, 51082, 51088, 51058 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 51021, 51041, 51047, 51065, 51053, 51071, 51077, 51083, 51089, 51059 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 51023, 51042, 51048, 51066, 51054, 51072, 51078, 51084, 51090, 51060 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 51025, 51043, 51049, 51067, 51055, 51073, 51079, 51085, 51091, 51061 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CO.IN"; }
    }
  }
}
