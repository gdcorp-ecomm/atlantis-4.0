using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNomDotEs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 57001, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 57001, 57036, 57042, 57060, 57048, 57066, 57072, 57078, 57084, 57054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 57001, 57037, 57043, 57061, 57049, 57067, 57073, 57079, 57085, 57055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 57001, 57038, 57044, 57062, 57050, 57068, 57074, 57080, 57086, 57056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 57001, 57039, 57045, 57063, 57051, 57069, 57075, 57081, 57087, 57057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 57001, 57040, 57046, 57064, 57052, 57070, 57076, 57082, 57088, 57058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 57011, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 57029, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 57030, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 57032, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 57033, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 57034, 57012, 57013, 57014, 57015, 57016, 57017, 57018, 57019 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 67001, 67002, 67003, 67004, 67005, 67006, 67007, 67008, 67009, 67010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 67020, 67036, 67042, 67060, 67048, 67066, 67072, 67078, 67084, 67054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 67021, 67037, 67043, 67061, 67049, 67067, 67073, 67079, 67085, 67055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 67024, 67038, 67044, 67062, 67050, 67068, 67074, 67080, 67086, 67056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 67026, 67039, 67045, 67063, 67051, 67069, 67075, 67081, 67087, 67057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 67028, 67040, 67046, 67064, 67052, 67070, 67076, 67082, 67088, 67058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
