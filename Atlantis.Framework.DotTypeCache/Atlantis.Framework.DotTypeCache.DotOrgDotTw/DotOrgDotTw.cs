using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotTw
{
  public class DotOrgDotTw : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14101, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14130, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14130, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14132, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14134, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14136, 14102, 14103, 14104, 14105, 14106, 14107, 14108, 14109, 14110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14111, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14140, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14140, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14141, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14142, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14143, 14122, 14123, 14124, 14125, 14126, 14127, 14128, 14129 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14112, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24130, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24130, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24132, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24134, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24136, 14113, 14114, 14115, 14116, 14117, 14118, 14119, 14120, 14121 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.TW"; }
    }
  }
}
