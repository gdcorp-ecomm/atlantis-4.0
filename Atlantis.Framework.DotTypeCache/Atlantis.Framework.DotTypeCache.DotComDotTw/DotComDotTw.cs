using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotTw
{
  public class DotComDotTw : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14051, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14080, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14080, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14082, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14084, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14086, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14061, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14090, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14090, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14091, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14092, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14093, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14062, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24080, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24080, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24082, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24084, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24086, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.TW"; }
    }
  }
}
