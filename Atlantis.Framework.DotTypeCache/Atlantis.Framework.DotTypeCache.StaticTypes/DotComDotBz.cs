using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotBz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41600, 41601, 41602, 41603, 41604, 41605, 41606, 41607, 41608, 41609 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41630, 41639, 41645, 41663, 41651, 41669, 41675, 41681, 41687, 41657 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41631, 41640, 41646, 41664, 41652, 41670, 41676, 41682, 41688, 41658 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41621, 41641, 41647, 41665, 41653, 41671, 41677, 41683, 41689, 41659 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41623, 41642, 41648, 41666, 41654, 41672, 41678, 41684, 41690, 41660 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41625, 41643, 41649, 41667, 41655, 41673, 41679, 41685, 41691, 41661 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41610, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41632, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41633, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41627, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41628, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41629, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 51600, 51601, 51602, 51603, 51604, 51605, 51606, 51607, 51608, 51609 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51630, 51639, 51645, 51663, 51651, 51669, 51675, 51681, 51687, 51657 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51631, 51640, 51646, 51664, 51652, 51670, 51676, 51682, 51688, 51658 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 51621, 51641, 51647, 51665, 51653, 51671, 51677, 51683, 51689, 51659 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 51623, 51642, 51648, 51666, 51654, 51672, 51678, 51684, 51690, 51660 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 51625, 51643, 51649, 51667, 51655, 51673, 51679, 51685, 51691, 51661 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.BZ"; }
    }

  }
}
