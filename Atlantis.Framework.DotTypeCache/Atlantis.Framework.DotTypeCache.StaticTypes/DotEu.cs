using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotEu : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{2851, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860}),
        new StaticDotTypeTier(6, new int[]{9570, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860}),
        new StaticDotTypeTier(21, new int[]{9571, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860}),
        new StaticDotTypeTier(50, new int[]{2912, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860}),
        new StaticDotTypeTier(101, new int[]{2914, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860}),
        new StaticDotTypeTier(201, new int[]{2916, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860})
      });
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{2861, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870}),
        new StaticDotTypeTier(6, new int[]{9572, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870}),
        new StaticDotTypeTier(21, new int[]{9573, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870}),
        new StaticDotTypeTier(50, new int[]{2921, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870}),
        new StaticDotTypeTier(101, new int[]{2922, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870}),
        new StaticDotTypeTier(201, new int[]{2923, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870})
      });
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{12851, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860}),
        new StaticDotTypeTier(6, new int[]{19570, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860}),
        new StaticDotTypeTier(21, new int[]{19571, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860}),
        new StaticDotTypeTier(50, new int[]{12872, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860}),
        new StaticDotTypeTier(101, new int[]{12874, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860}),
        new StaticDotTypeTier(201, new int[]{12876, 12852, 12853, 12854, 12855, 12856, 12857, 12858, 12859, 12860})
      });
    }

    public override string DotType
    {
      get { return "EU"; }
    }

  }
}
