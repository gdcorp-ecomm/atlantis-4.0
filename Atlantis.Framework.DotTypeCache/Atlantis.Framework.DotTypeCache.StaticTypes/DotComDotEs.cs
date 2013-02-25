using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotEs : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 55001, 55002, 55003, 55004, 55005, 55006, 55007, 55008, 55009, 55010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 55001, 55036, 55042, 55060, 55048, 55066, 55072, 55078, 55084, 55054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 55001, 55037, 55043, 55061, 55049, 55067, 55073, 55079, 55085, 55055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 55001, 55038, 55044, 55062, 55050, 55068, 55074, 55080, 55086, 55056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 55001, 55039, 55045, 55063, 55051, 55069, 55075, 55081, 55087, 55057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 55001, 55040, 55046, 55064, 55052, 55070, 55076, 55082, 55088, 55058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 55011, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 55029, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 55030, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 55032, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 55033, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 55034, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 65001, 65002, 65003, 65004, 65005, 65006, 65007, 65008, 65009, 65010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 65020, 65036, 65042, 65060, 65048, 65066, 65072, 65078, 65084, 65054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 65021, 65037, 65043, 65061, 65049, 65067, 65073, 65079, 65085, 65055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 65024, 65038, 65044, 65062, 65050, 65068, 65074, 65080, 65086, 65056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 65026, 65039, 65045, 65063, 65051, 65069, 65075, 65081, 65087, 65057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 65028, 65040, 65046, 65064, 65052, 65070, 65076, 65082, 65088, 65058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.ES"; }
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
