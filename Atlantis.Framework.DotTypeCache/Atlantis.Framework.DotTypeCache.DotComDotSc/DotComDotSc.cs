using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotSc
{
  public class DotComDotSc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40501, 40502, 40503, 40504, 40505, 40506, 40507, 40508, 40509, 40510 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50501, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50520, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50520, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50522, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50524, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50526, 50502, 50503, 50504, 50505, 50506, 50507, 50508, 50509, 50510 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.SC"; }
    }
  }
}
