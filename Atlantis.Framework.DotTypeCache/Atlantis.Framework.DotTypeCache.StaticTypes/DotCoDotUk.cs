using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCoDotUk : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13311, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13340, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13340, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13341, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13342, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13343, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23332, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23334, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23336, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CO.UK"; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

  }
}
