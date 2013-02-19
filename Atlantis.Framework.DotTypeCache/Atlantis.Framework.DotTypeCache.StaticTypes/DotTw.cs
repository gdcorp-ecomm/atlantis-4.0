using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotTw : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13912, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 23930, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 23930, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23932, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23934, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23936, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "TW"; }
    }
  }
}
