using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotAsia : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 2770, 2771, 2772, 2773, 2774, 2775, 2776, 2777, 2778, 2779 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9330, 9351, 9357, 9375, 9363, 9381, 9387, 9393, 9399, 9369 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9331, 9352, 9358, 9376, 9364, 9382, 9388, 9394, 9400, 9370 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 9336, 9353, 9359, 9377, 9365, 9383, 9389, 9395, 9401, 9371 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 9338, 9354, 9360, 9378, 9366, 9384, 9390, 9396, 9402, 9372 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 9340, 9355, 9361, 9379, 9367, 9385, 9391, 9397, 9403, 9373 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 2780, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9332, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9333, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 2794, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 2795, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 2796, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12770, 12771, 12772, 12773, 12774, 12775, 12776, 12777, 12778, 12779 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 19330, 19351, 19357, 19375, 19363, 19381, 19387, 19393, 19399, 19369 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 19331, 19352, 19358, 19376, 19364, 19382, 19388, 19394, 19400, 19370 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 19336, 19353, 19359, 19377, 19365, 19383, 19389, 19395, 19401, 19371 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 19338, 19354, 19360, 19378, 19366, 19384, 19390, 19396, 19402, 19372 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 19340, 19355, 19361, 19379, 19367, 19385, 19391, 19397, 19403, 19373 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ASIA"; }
    }
  }
}
