using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotIn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 9850, 9851, 9852, 9853, 9854, 9855, 9856, 9857, 9858, 9859 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9880, 9889, 9895, 9913, 9901, 9919, 9925, 9931, 9937, 9907 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9881, 9890, 9896, 9914, 9902, 9920, 9926, 9932, 9938, 9908 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 9871, 9891, 9897, 9915, 9903, 9921, 9927, 9933, 9939, 9909 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 9873, 9892, 9898, 9916, 9904, 9922, 9928, 9934, 9940, 9910 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 9875, 9893, 9899, 9917, 9905, 9923, 9929, 9935, 9941, 9911 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 9860, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9882, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9883, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 9877, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 9878, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 9879, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19850, 19851, 19852, 19853, 19854, 19855, 19856, 19857, 19858, 19859 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 19880, 19889, 19895, 19913, 19901, 19919, 19925, 19931, 19937, 19907 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 19881, 19890, 19896, 19914, 19902, 19920, 19926, 19932, 19938, 19908 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 19871, 19891, 19897, 19915, 19903, 19921, 19927, 19933, 19939, 19909 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 19873, 19892, 19898, 19916, 19904, 19922, 19928, 19934, 19940, 19910 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 19875, 19893, 19899, 19917, 19905, 19923, 19929, 19935, 19941, 19911 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "IN"; }
    }

  }
}
