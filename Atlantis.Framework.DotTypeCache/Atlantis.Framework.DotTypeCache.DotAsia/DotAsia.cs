using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotAsia
{
  public class DotAsia : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 2770, 2771, 2772, 2773, 2774, 2775, 2776, 2777, 2778, 2779 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9330, 9351, 9357, 9375, 9363, 9381, 9387, 9393, 9399, 9369 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9331, 9352, 9358, 9376, 9364, 9382, 9388, 9394, 9400, 9370 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9336, 9353, 9359, 9377, 9365, 9383, 9389, 9395, 9401, 9371 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9338, 9354, 9360, 9378, 9366, 9384, 9390, 9396, 9402, 9372 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9340, 9355, 9361, 9379, 9367, 9385, 9391, 9397, 9403, 9373 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 2780, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9332, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9333, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 2794, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 2795, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 2796, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12770, 12771, 12772, 12773, 12774, 12775, 12776, 12777, 12778, 12779 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 19330, 19351, 19357, 19375, 19363, 19381, 19387, 19393, 19399, 19369 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 19331, 19352, 19358, 19376, 19364, 19382, 19388, 19394, 19400, 19370 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 19336, 19353, 19359, 19377, 19365, 19383, 19389, 19395, 19401, 19371 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 19338, 19354, 19360, 19378, 19366, 19384, 19390, 19396, 19402, 19372 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 19340, 19355, 19361, 19379, 19367, 19385, 19391, 19397, 19403, 19373 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ASIA"; }
    }
  }
}
