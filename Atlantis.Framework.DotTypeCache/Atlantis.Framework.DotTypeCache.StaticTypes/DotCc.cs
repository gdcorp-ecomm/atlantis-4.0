using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11101, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 11121, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 11121, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11122, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11123, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11124, 11102, 11103, 11104, 11105, 11106, 11107, 11108, 11109, 11110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 11111, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 11171, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 11172, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 11173, 11112, 11113, 11114, 11115, 11116, 11117, 11118, 11119 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 111101, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 21122, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 21123, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 21124, 111102, 111103, 111104, 111105, 111106, 111107, 111108, 111109, 111110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19012, 19024, 19053, 0, 19069, 0, 0, 0, 0, 19070 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.ExpiredAuctionReg, new StaticDotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "CC"; }
    }
  }
}
