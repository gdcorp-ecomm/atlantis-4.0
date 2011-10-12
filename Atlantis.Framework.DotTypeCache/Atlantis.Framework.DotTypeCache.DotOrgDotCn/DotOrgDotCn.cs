using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotCn
{
  public class DotOrgDotCn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14251, 14252, 14253, 14254, 14255, 14256, 14257, 14258, 14259, 14260 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14261, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 14290, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 14290, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 14291, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 14292, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 14293, 14272, 14273, 14274, 14275, 14276, 14277, 14278, 14279 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 14262, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 24280, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 24280, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 24282, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 24284, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 24286, 14263, 14264, 14265, 14266, 14267, 14268, 14269, 14270, 14271 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.CN"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }
  }
}
