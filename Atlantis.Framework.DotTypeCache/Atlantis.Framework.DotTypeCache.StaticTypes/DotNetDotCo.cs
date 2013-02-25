using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotCo : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56601, 56602, 56603, 56604, 56605, 56606, 56607, 56608, 56609, 56610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56620, 56636, 56642, 56660, 56648, 56666, 56672, 56678, 56684, 56654 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56621, 56637, 56643, 56661, 56649, 56667, 56673, 56679, 56685, 56655 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56624, 56638, 56644, 56662, 56650, 56668, 56674, 56680, 56686, 56656 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56626, 56639, 56645, 56663, 56651, 56669, 56675, 56681, 56687, 56657 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56628, 56640, 56646, 56664, 56652, 56670, 56676, 56682, 56688, 56658 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56611, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56629, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56630, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56632, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56633, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56634, 56612, 56613, 56614, 56615, 56616, 56617, 56618, 56619 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 66601, 66602, 66603, 66604, 66605, 66606, 66607, 66608, 66609, 66610 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 66620, 66636, 66642, 66660, 66648, 66666, 66672, 66678, 66684, 66654 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 66621, 66637, 66643, 66661, 66649, 66667, 66673, 66679, 66685, 66655 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 66624, 66638, 66644, 66662, 66650, 66668, 66674, 66680, 66686, 66656 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 66626, 66639, 66645, 66663, 66651, 66669, 66675, 66681, 66687, 66657 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 66628, 66640, 66646, 66664, 66652, 66670, 66676, 66682, 66688, 66658 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.CO"; }
    }

    public override int MinRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MinRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 60; }
    }

    public override int MaxTransferLength
    {
      get { return 4; }
    }
  }
}
