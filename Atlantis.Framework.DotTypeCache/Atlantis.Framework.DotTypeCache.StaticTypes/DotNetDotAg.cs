using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotAg : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40101, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40120, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40120, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40122, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40124, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40126, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50101, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50120, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50120, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50122, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50124, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50126, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.AG"; }
    }
  }
}
