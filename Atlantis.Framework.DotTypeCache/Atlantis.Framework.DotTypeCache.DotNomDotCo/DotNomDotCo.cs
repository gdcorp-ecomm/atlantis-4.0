using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNomDotCo
{
  public class DotNomDotCo : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 56701, 56702, 56703, 56704, 56705, 56706, 56707, 56708, 56709, 56710 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 56720, 56736, 56742, 56760, 56748, 56766, 56772, 56778, 56784, 56754 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 56721, 56737, 56743, 56761, 56749, 56767, 56773, 56779, 56785, 56755 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 56724, 56738, 56744, 56762, 56750, 56768, 56774, 56780, 56786, 56756 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 56726, 56739, 56745, 56763, 56751, 56769, 56775, 56781, 56787, 56757 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 56728, 56740, 56746, 56764, 56752, 56770, 56776, 56782, 56788, 56758 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 56711, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 56729, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 56730, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 56732, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 56733, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 56734, 56712, 56713, 56714, 56715, 56716, 56717, 56718, 56719 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 66701, 66702, 66703, 66704, 66705, 66706, 66707, 66708, 66709, 66710 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 66720, 66736, 66742, 66760, 66748, 66766, 66772, 66778, 66784, 66754 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 66721, 66737, 66743, 66761, 66749, 66767, 66773, 66779, 66785, 66755 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 66724, 66738, 66744, 66762, 66750, 66768, 66774, 66780, 66786, 66756 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 66726, 66739, 66745, 66763, 66751, 66769, 66775, 66781, 66787, 66757 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 66728, 66740, 66746, 66764, 66752, 66770, 66776, 66782, 66788, 66758 });
      
      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NOM.CO"; }
    }

    public override int MinRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MinRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    public override int MaxTransferLength
    {
      get { return 4; }
    }
  }
}
