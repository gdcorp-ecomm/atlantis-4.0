using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotAg : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50151, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50170, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50170, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50172, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50174, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50176, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.AG"; }
    }
  }
}
