using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotIdvDotTw
{
  public class DotIdvDotTw : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14151, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14180, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14180, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14182, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14184, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14186, 14152, 14153, 14154, 14155, 14156, 14157, 14158, 14159, 14160 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14161, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14190, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14190, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14191, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14192, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14193, 14172, 14173, 14174, 14175, 14176, 14177, 14178, 14179 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14162, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24180, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24180, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24182, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24184, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24186, 14163, 14164, 14165, 14166, 14167, 14168, 14169, 14170, 14171 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "IDV.TW"; }
    }
  }
}
