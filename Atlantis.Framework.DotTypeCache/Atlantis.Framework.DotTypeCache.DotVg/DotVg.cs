using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotVg
{
  public class DotVg : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40751, 40752, 40753, 40754, 40755, 40756, 40757, 40758, 40759, 40760 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50751, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50770, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50770, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50772, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50774, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50776, 50752, 50753, 50754, 50755, 50756, 50757, 50758, 50759, 50760 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "VG"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }
  }
}
