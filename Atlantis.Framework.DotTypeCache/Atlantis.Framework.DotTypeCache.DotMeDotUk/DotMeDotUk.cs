using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotMeDotUk
{
  public class DotMeDotUk : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13411, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13440, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13440, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13441, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13442, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13443, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23432, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23434, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23436, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ME.UK"; }
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
