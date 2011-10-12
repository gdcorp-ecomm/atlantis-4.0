using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotEs
{
  public class DotEs : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 54001, 54002, 54003, 54004, 54005, 54006, 54007, 54008, 54009, 54010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 54001, 54036, 54042, 54060, 54048, 54066, 54072, 54078, 54084, 54054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 54001, 54037, 54043, 54061, 54049, 54067, 54073, 54079, 54085, 54055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 54001, 54038, 54044, 54062, 54050, 54068, 54074, 54080, 54086, 54056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 54001, 54039, 54045, 54063, 54051, 54069, 54075, 54081, 54087, 54057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 54001, 54040, 54046, 54064, 54052, 54070, 54076, 54082, 54088, 54058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 54011, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 54029, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 54030, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 54032, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 54033, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 54034, 54012, 54013, 54014, 54015, 54016, 54017, 54018, 54019 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 64001, 64002, 64003, 64004, 64005, 64006, 64007, 64008, 64009, 64010 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 64020, 64036, 64042, 64060, 64048, 64066, 64072, 64078, 64084, 64054 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 64021, 64037, 64043, 64061, 64049, 64067, 64073, 64079, 64085, 64055 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 64024, 64038, 64044, 64062, 64050, 64068, 64074, 64080, 64086, 64056 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 64026, 64039, 64045, 64063, 64051, 64069, 64075, 64081, 64087, 64057 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 64028, 64040, 64046, 64064, 64052, 64070, 64076, 64082, 64088, 64058 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ES"; }
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
