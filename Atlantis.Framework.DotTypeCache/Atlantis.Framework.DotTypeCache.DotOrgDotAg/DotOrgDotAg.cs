using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotAg
{
  public class DotOrgDotAg : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40151, 40152, 40153, 40154, 40155, 40156, 40157, 40158, 40159, 40160 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50151, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50170, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50170, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50172, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50174, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50176, 50152, 50153, 50154, 50155, 50156, 50157, 50158, 50159, 50160 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.AG"; }
    }
  }
}
