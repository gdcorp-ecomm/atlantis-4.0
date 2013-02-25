using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCoDotNz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12901, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12930, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12930, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12932, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12934, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12936, 12902, 12903, 12904, 12905, 12906, 12907, 12908, 12909, 12910 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12911, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12940, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12940, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12941, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12942, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12943, 12922, 12923, 12924, 12925, 12926, 12927, 12928, 12929 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12912, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 22930, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 22930, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22932, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22934, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22936, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
