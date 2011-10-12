using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotNz
{
  public class DotOrgDotNz : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13101, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13130, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13130, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13132, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13134, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13136, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13111, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13140, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13140, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13141, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13142, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13143, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13112, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 23130, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 23130, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23132, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23134, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23136, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.NZ"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }
  }
}
