using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotSc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50601, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50620, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50620, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50622, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50624, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50626, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.SC"; }
    }
  }
}
