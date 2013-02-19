using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotTv : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13201, 13202, 13203, 13204, 13205, 13206, 13207, 13208, 13209, 13210 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9069, 9085, 9091, 9097, 9103, 9109, 9115, 9121, 9127, 9133 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9070, 9086, 9092, 9098, 9104, 9110, 9116, 9122, 9128, 9134 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13232, 9087, 9093, 9099, 9105, 9111, 9117, 9123, 9129, 9135 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13234, 9088, 9094, 9100, 9106, 9112, 9118, 9124, 9130, 9136 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13236, 9089, 9095, 9101, 9107, 9113, 9119, 9125, 9131, 9137 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13211, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9073, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9074, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13241, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13242, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13243, 13222, 13223, 13224, 13225, 13226, 13227, 13228, 13229 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13212, 13213, 13214, 13215, 13216, 13217, 13218, 13219, 13220, 13221 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 9071, 19085, 19091, 19097, 19103, 19109, 19115, 19121, 19127, 19133 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 9072, 19086, 19092, 19098, 19104, 19110, 19116, 19122, 19128, 19134 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23232, 19087, 19093, 19099, 19105, 19111, 19117, 19123, 19129, 19135 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23234, 19088, 19094, 19100, 19106, 19112, 19118, 19124, 19130, 19136 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23236, 19089, 19095, 19101, 19107, 19113, 19119, 19125, 19131, 19137 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 19028, 19244, 19245, 0, 19246, 0, 0, 0, 0, 19247 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.ExpiredAuctionReg, new StaticDotTypeTier[] { DotTypeTier0 });
      return result;
    }

    public override string DotType
    {
      get { return "TV"; }
    }
  }
}
