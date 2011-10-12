using System;
using Atlantis.Framework.DotTypeCache.Interface;
namespace Atlantis.Framework.DotTypeCache.DotFr
{
  public class DotFr : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 56301, 56302, 56303, 56304, 56305, 56306, 56307, 56308, 56309, 56310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 56320, 56336, 56342, 56360, 56348, 56366, 56372, 56378, 56384, 56354 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 56321, 56337, 56343, 56361, 56349, 56367, 56373, 56379, 56385, 56355 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 56324, 56338, 56344, 56362, 56350, 56368, 56374, 56380, 56386, 56356 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 56326, 56339, 56345, 56363, 56351, 56369, 56375, 56381, 56387, 56357 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 56328, 56340, 56346, 56364, 56352, 56370, 56376, 56382, 56388, 56358 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 56311, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 56329, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 56330, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 56332, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 56333, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 56334, 56312, 56313, 56314, 56315, 56316, 56317, 56318, 56319 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 66301, 66302, 66303, 66304, 66305, 66306, 66307, 66308, 66309, 66310 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 66320, 66336, 66342, 66360, 66348, 66366, 66372, 66378, 66384, 66354 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 66321, 66337, 66343, 66361, 66349, 66367, 66373, 66379, 66385, 66355 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 66324, 66338, 66344, 66362, 66350, 66368, 66374, 66380, 66386, 66356 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 66326, 66339, 66345, 66363, 66351, 66369, 66375, 66381, 66387, 66357 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 66328, 66340, 66346, 66364, 66352, 66370, 66376, 66382, 66388, 66358 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "FR"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }
  }
}
