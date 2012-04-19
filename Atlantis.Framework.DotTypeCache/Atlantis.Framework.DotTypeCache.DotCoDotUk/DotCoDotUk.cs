using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCoDotUk
{
  public class DotCoDotUk : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13301, 13302, 13303, 13304, 13305, 13306, 13307, 13308, 13309, 13310 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13311, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13340, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13340, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13341, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13342, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13343, 13322, 13323, 13324, 13325, 13326, 13327, 13328, 13329 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13312, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23332, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23334, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23336, 13313, 13314, 13315, 13316, 13317, 13318, 13319, 13320, 13321 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CO.UK"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MinRenewalLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }
  }
}
