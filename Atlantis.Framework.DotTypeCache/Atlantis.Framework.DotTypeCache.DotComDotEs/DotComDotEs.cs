using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotEs
{
  public class DotComDotEs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 55001, 55002, 55003, 55004, 55005, 55006, 55007, 55008, 55009, 55010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 55001, 55036, 55042, 55060, 55048, 55066, 55072, 55078, 55084, 55054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 55001, 55037, 55043, 55061, 55049, 55067, 55073, 55079, 55085, 55055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 55001, 55038, 55044, 55062, 55050, 55068, 55074, 55080, 55086, 55056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 55001, 55039, 55045, 55063, 55051, 55069, 55075, 55081, 55087, 55057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 55001, 55040, 55046, 55064, 55052, 55070, 55076, 55082, 55088, 55058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 55011, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 55029, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 55030, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 55032, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 55033, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 55034, 55012, 55013, 55014, 55015, 55016, 55017, 55018, 55019 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 65001, 65002, 65003, 65004, 65005, 65006, 65007, 65008, 65009, 65010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 65020, 65036, 65042, 65060, 65048, 65066, 65072, 65078, 65084, 65054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 65021, 65037, 65043, 65061, 65049, 65067, 65073, 65079, 65085, 65055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 65024, 65038, 65044, 65062, 65050, 65068, 65074, 65080, 65086, 65056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 65026, 65039, 65045, 65063, 65051, 65069, 65075, 65081, 65087, 65057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 65028, 65040, 65046, 65064, 65052, 65070, 65076, 65082, 65088, 65058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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
  }
}
