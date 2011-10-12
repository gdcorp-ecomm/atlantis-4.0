using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotIn
{
  public class DotIn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9850, 9851, 9852, 9853, 9854, 9855, 9856, 9857, 9858, 9859 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9880, 9889, 9895, 9913, 9901, 9919, 9925, 9931, 9937, 9907 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9881, 9890, 9896, 9914, 9902, 9920, 9926, 9932, 9938, 9908 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9871, 9891, 9897, 9915, 9903, 9921, 9927, 9933, 9939, 9909 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9873, 9892, 9898, 9916, 9904, 9922, 9928, 9934, 9940, 9910 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9875, 9893, 9899, 9917, 9905, 9923, 9929, 9935, 9941, 9911 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9860, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9882, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9883, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9877, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9878, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9879, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19850, 19851, 19852, 19853, 19854, 19855, 19856, 19857, 19858, 19859 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 19880, 19889, 19895, 19913, 19901, 19919, 19925, 19931, 19937, 19907 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 19881, 19890, 19896, 19914, 19902, 19920, 19926, 19932, 19938, 19908 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 19871, 19891, 19897, 19915, 19903, 19921, 19927, 19933, 19939, 19909 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 19873, 19892, 19898, 19916, 19904, 19922, 19928, 19934, 19940, 19910 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 19875, 19893, 19899, 19917, 19905, 19923, 19929, 19935, 19941, 19911 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "IN"; }
    }

  }
}
