using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotSc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50501, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50520, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50520, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50522, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50524, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50526, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.SC"; }
    }
  }
}
