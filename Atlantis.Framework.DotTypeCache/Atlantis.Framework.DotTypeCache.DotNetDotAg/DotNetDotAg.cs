using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotAg
{
  public class DotNetDotAg : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40101, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40120, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40120, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40122, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40124, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40126, 40102, 40103, 40104, 40105, 40106, 40107, 40108, 40109, 40110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50101, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50120, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50120, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50122, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50124, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50126, 50102, 50103, 50104, 50105, 50106, 50107, 50108, 50109, 50110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.AG"; }
    }
  }
}
