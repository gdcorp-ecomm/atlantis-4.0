using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotBz
{
  public class DotBz : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 15201, 15202, 15203, 15204, 15205, 15206, 15207, 15208, 15209, 15210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 15244, 15249, 15255, 15273, 15261, 15279, 15285, 15291, 15297, 15267 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 15245, 15250, 15256, 15274, 15262, 15280, 15286, 15292, 15298, 15268 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 15232, 15251, 15257, 15275, 15263, 15281, 15287, 15293, 15299, 15269 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 15234, 15252, 15258, 15276, 15264, 15282, 15288, 15294, 15305, 15270 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 15236, 15253, 15259, 15277, 15265, 15283, 15289, 15295, 15306, 15271 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 15211, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 15246, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 15247, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 15241, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 15242, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 15243, 15222, 15223, 15224, 15225, 15226, 15227, 15228, 15229 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 15212, 15213, 15214, 15215, 15216, 15217, 15218, 15219, 15220, 15221 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 25244, 25249, 25255, 25273, 25261, 25279, 25285, 25291, 25297, 25267 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 25245, 25250, 25256, 25274, 25262, 25280, 25286, 25292, 25298, 25268 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 25232, 25251, 25257, 25275, 25263, 25281, 25287, 25293, 25299, 25269 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 25234, 25252, 25258, 25276, 25264, 25282, 25288, 25294, 25305, 25270 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 25236, 25253, 25259, 25277, 25265, 25283, 25289, 25295, 25306, 25271 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "BZ"; }
    }
  }
}
