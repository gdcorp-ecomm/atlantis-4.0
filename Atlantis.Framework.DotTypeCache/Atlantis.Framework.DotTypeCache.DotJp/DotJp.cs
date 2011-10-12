using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotJp
{
  public class DotJp : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12701, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12730, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12730, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12732, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12734, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12736, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12711, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12740, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12740, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12741, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12742, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12743, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12712, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 22730, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 22730, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22732, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22734, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22736, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "JP"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }
  }
}
