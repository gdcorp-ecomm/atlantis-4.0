using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNomDotEs
{
  public class DotNomDotEs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57001, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57001, 57036, 57042, 57060, 57048, 57066, 57072, 57078, 57084, 57054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57001, 57037, 57043, 57061, 57049, 57067, 57073, 57079, 57085, 57055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57001, 57038, 57044, 57062, 57050, 57068, 57074, 57080, 57086, 57056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57001, 57039, 57045, 57063, 57051, 57069, 57075, 57081, 57087, 57057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57001, 57040, 57046, 57064, 57052, 57070, 57076, 57082, 57088, 57058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57011, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57029, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57030, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57032, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57033, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57034, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 67001, 67002, 67003, 67004, 67005, 67006, 67007, 67008, 67009, 67010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 67020, 67036, 67042, 67060, 67048, 67066, 67072, 67078, 67084, 67054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 67021, 67037, 67043, 67061, 67049, 67067, 67073, 67079, 67085, 67055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 67024, 67038, 67044, 67062, 67050, 67068, 67074, 67080, 67086, 67056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 67026, 67039, 67045, 67063, 67051, 67069, 67075, 67081, 67087, 67057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 67028, 67040, 67046, 67064, 67052, 67070, 67076, 67082, 67088, 67058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NOM.ES"; }
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
  }
}
