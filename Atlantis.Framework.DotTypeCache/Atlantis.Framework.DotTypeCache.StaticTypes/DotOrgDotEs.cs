﻿using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotEs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56001, 56002, 56003, 56004, 56005, 56006, 56007, 56008, 56009, 56010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56001, 56036, 56042, 56060, 56048, 56066, 56072, 56078, 56084, 56054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56001, 56037, 56043, 56061, 56049, 56067, 56073, 56079, 56085, 56055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56001, 56038, 56044, 56062, 56050, 56068, 56074, 56080, 56086, 56056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56001, 56039, 56045, 56063, 56051, 56069, 56075, 56081, 56087, 56057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56001, 56040, 56046, 56064, 56052, 56070, 56076, 56082, 56088, 56058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56011, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56029, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56030, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56032, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56033, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56034, 56012, 56013, 56014, 56015, 56016, 56017, 56018, 56019 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 66001, 66002, 66003, 66004, 66005, 66006, 66007, 66008, 66009, 66010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 66020, 66036, 66042, 66060, 66048, 66066, 66072, 66078, 66084, 66054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 66021, 66037, 66043, 66061, 66049, 66067, 66073, 66079, 66085, 66055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 66024, 66038, 66044, 66062, 66050, 66068, 66074, 66080, 66086, 66056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 66026, 66039, 66045, 66063, 66051, 66069, 66075, 66081, 66087, 66057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 66028, 66040, 66046, 66064, 66052, 66070, 66076, 66082, 66088, 66058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.ES"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
