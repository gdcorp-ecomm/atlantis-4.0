using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotTw : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14101, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14130, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14130, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14132, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14134, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14136, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14111, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14140, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14140, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14141, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14142, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14143, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14112, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24130, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24130, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24132, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24134, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24136, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.TW"; }
    }
  }
}
