using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotTw : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14051, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14080, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14080, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14082, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14084, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14086, 14052, 14053, 14054, 14055, 14056, 14057, 14058, 14059, 14060 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14061, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14090, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14090, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14091, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14092, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14093, 14072, 14073, 14074, 14075, 14076, 14077, 14078, 14079 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14062, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24080, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24080, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24082, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24084, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24086, 14063, 14064, 14065, 14066, 14067, 14068, 14069, 14070, 14071 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.TW"; }
    }
  }
}
