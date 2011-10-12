using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotSc
{
  public class DotOrgDotSc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40601, 40602, 40603, 40604, 40605, 40606, 40607, 40608, 40609, 40610 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50601, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50620, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50620, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50622, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50624, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50626, 50602, 50603, 50604, 50605, 50606, 50607, 50608, 50609, 50610 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.SC"; }
    }
  }
}
