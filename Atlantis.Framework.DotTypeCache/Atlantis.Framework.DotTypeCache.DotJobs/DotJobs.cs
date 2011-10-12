using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotJobs
{
  public class DotJobs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 601, 602, 603, 604, 605, 606, 607, 608, 609, 610 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 660, 602, 603, 604, 605, 606, 607, 608, 609, 610 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 660, 602, 603, 604, 605, 606, 607, 608, 609, 610 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 662, 602, 603, 604, 605, 606, 607, 608, 609, 610 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 664, 602, 603, 604, 605, 606, 607, 608, 609, 610 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 666, 602, 603, 604, 605, 606, 607, 608, 609, 610 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 611, 613, 614, 615, 616, 617, 618, 619, 620 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 670, 613, 614, 615, 616, 617, 618, 619, 620 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 670, 613, 614, 615, 616, 617, 618, 619, 620 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 671, 613, 614, 615, 616, 617, 618, 619, 620 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 672, 613, 614, 615, 616, 617, 618, 619, 620 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 673, 613, 614, 615, 616, 617, 618, 619, 620 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 10601, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 10660, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 10660, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 10662, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 10664, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 10666, 10602, 10603, 10604, 10605, 10606, 10607, 10608, 10609, 10610 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "JOBS"; }
    }
  }
}
