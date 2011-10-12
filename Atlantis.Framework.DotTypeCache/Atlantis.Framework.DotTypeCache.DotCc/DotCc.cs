using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCc
{
  public class DotCc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11101, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 11121, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 11121, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11122, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11123, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11124, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 11171, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 11172, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 11173, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 21122, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 21123, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 21124, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19012, 19024, 19053, 0, 19069, 0, 0, 0, 0, 19070 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "CC"; }
    }
  }
}
