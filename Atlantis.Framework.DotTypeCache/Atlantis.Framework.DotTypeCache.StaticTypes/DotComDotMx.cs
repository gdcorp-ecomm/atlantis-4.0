using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotMx : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12801, 12802, 12803, 12804, 12805, 12806, 12807, 12808, 12809, 12810 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41801, 41806, 41812, 41830, 41818, 41836, 41842, 41848, 41854, 41824 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41802, 41807, 41813, 41831, 41819, 41837, 41843, 41849, 41855, 41825 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12832, 41808, 41814, 41832, 41820, 41838, 41844, 41850, 41856, 41826 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12834, 41809, 41815, 41833, 41821, 41839, 41845, 41851, 41857, 41827 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12836, 41810, 41816, 41834, 41822, 41840, 41846, 41852, 41858, 41828 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12811, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41803, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41804, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12841, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12842, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12843, 12822, 12823, 12824, 12825, 12826, 12827, 12828, 12829 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 22830, 51805, 51811, 51829, 51817, 51835, 51841, 51847, 51853, 51823 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51801, 51806, 51812, 51830, 51818, 51836, 51842, 51848, 51854, 51824 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51802, 51807, 51813, 51831, 51819, 51837, 51843, 51849, 51855, 51825 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22832, 51808, 51814, 51832, 51820, 51838, 51844, 51850, 51856, 51826 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22834, 51809, 51815, 51833, 51821, 51839, 51845, 51851, 51857, 51827 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22836, 51810, 51816, 51834, 51822, 51840, 51846, 51852, 51858, 51828 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.MX"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 60; }
    }
  }
}
