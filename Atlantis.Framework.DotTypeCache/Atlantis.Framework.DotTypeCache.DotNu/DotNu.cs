using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNu
{
  public class DotNu : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40401, 40402, 40403, 40404, 40405, 40406, 40407, 40408, 40409, 40410 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50401, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50420, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50420, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50422, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50424, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50426, 50402, 50403, 50404, 50405, 50406, 50407, 50408, 50409, 50410 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NU"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }

    public override int MinRenewalLength
    {
      get { return 2; }
    }
  }
}
