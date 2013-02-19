using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotIdvDotTw : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14151, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14180, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14180, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14182, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14184, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14186, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14161, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14190, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14190, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14191, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14192, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14193, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14162, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24180, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24180, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24182, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24184, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24186, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "IDV.TW"; }
    }
  }
}
