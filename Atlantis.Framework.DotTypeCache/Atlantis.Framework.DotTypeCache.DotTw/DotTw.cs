using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotTw
{
  public class DotTw : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13901, 13902, 13903, 13904, 13905, 13906, 13907, 13908, 13909, 13910 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13912, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 23930, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 23930, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23932, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23934, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23936, 13913, 13914, 13915, 13916, 13917, 13918, 13919, 13920, 13921 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "TW"; }
    }
  }
}
