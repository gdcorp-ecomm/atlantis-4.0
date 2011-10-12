using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotBz
{
  public class DotComDotBz : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41600, 41601, 41602, 41603, 41604, 41605, 41606, 41607, 41608, 41609 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41630, 41639, 41645, 41663, 41651, 41669, 41675, 41681, 41687, 41657 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41631, 41640, 41646, 41664, 41652, 41670, 41676, 41682, 41688, 41658 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41621, 41641, 41647, 41665, 41653, 41671, 41677, 41683, 41689, 41659 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41623, 41642, 41648, 41666, 41654, 41672, 41678, 41684, 41690, 41660 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41625, 41643, 41649, 41667, 41655, 41673, 41679, 41685, 41691, 41661 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41610, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41632, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41633, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41627, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41628, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41629, 41611, 41612, 41613, 41614, 41615, 41616, 41617, 41618 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 51600, 51601, 51602, 51603, 51604, 51605, 51606, 51607, 51608, 51609 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 51630, 51639, 51645, 51663, 51651, 51669, 51675, 51681, 51687, 51657 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 51631, 51640, 51646, 51664, 51652, 51670, 51676, 51682, 51688, 51658 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 51621, 51641, 51647, 51665, 51653, 51671, 51677, 51683, 51689, 51659 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 51623, 51642, 51648, 51666, 51654, 51672, 51678, 51684, 51690, 51660 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 51625, 51643, 51649, 51667, 51655, 51673, 51679, 51685, 51691, 51661 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.BZ"; }
    }

  }
}
