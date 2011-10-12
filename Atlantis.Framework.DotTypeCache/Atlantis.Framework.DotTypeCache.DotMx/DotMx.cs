using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotMx
{
  public class DotMx : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41901, 41902, 41903, 41904, 41905, 41906, 41907, 41908, 41909, 41910 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41920, 41936, 41942, 41960, 41948, 41966, 41972, 41978, 41984, 41954 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41921, 41937, 41943, 41961, 41949, 41967, 41973, 41979, 41985, 41955 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41924, 41938, 41944, 41962, 41950, 41968, 41974, 41980, 41986, 41956 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41926, 41939, 41945, 41963, 41951, 41969, 41975, 41981, 41987, 41957 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41928, 41940, 41946, 41964, 41952, 41970, 41976, 41982, 41988, 41958 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41911, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41929, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41930, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41932, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41933, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41934, 41912, 41913, 41914, 41915, 41916, 41917, 41918, 41919 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 51901, 51902, 51903, 51904, 51905, 51906, 51907, 51908, 51909, 51910 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 51920, 51936, 51942, 51960, 51948, 51966, 51972, 51978, 51984, 51954 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 51921, 51937, 51943, 51961, 51949, 51967, 51973, 51979, 51985, 51955 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 51924, 51938, 51944, 51962, 51950, 51968, 51974, 51980, 51986, 51956 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 51926, 51939, 51945, 51963, 51951, 51969, 51975, 51981, 51987, 51957 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 51928, 51940, 51946, 51964, 51952, 51970, 51976, 51982, 51988, 51958 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "MX"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxTransferLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }
  }
}
