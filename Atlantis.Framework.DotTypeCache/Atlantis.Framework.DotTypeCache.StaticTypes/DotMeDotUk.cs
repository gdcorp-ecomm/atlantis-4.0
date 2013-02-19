using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotMeDotUk : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408, 13409, 13410 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13411, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13440, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13440, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13441, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13442, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13443, 13422, 13423, 13424, 13425, 13426, 13427, 13428, 13429 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13412, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23432, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23434, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23436, 13413, 13414, 13415, 13416, 13417, 13418, 13419, 13420, 13421 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ME.UK"; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

  }
}
