using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotBz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41700, 41701, 41702, 41703, 41704, 41705, 41706, 41707, 41708, 41709 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41730, 41739, 41745, 41763, 41751, 41769, 41775, 41781, 41787, 41757 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41731, 41740, 41746, 41764, 41752, 41770, 41776, 41782, 41788, 41758 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41721, 41741, 41747, 41765, 41753, 41771, 41777, 41783, 41789, 41759 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41723, 41742, 41748, 41766, 41754, 41772, 41778, 41784, 41790, 41760 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41725, 41743, 41749, 41767, 41755, 41773, 41779, 41785, 41791, 41761 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41710, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41732, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41733, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41727, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41728, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41729, 41711, 41712, 41713, 41714, 41715, 41716, 41717, 41718 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 51700, 51701, 51702, 51703, 51704, 51705, 51706, 51707, 51708, 51709 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51730, 51739, 51745, 51763, 51751, 51769, 51775, 51781, 51787, 51757 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51731, 51740, 51746, 51764, 51752, 51770, 51776, 51782, 51788, 51758 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 51721, 51741, 51747, 51765, 51753, 51771, 51777, 51783, 51789, 51759 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 51723, 51742, 51748, 51766, 51754, 51772, 51778, 51784, 51790, 51760 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 51725, 51743, 51749, 51767, 51755, 51773, 51779, 51785, 51791, 51761 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.BZ"; }
    }
  }
}
