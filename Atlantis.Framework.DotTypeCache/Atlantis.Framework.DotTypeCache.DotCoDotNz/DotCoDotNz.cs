using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCoDotNz
{
  public class DotCoDotNz : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12901, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12930, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12930, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12932, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12934, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12936, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12911, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12940, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12940, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12941, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12942, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12943, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12912, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 22930, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 22930, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22932, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22934, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22936, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CO.NZ"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }
  }
}
