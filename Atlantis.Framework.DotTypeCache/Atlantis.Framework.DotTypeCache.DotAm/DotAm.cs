using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotAm
{
  public class DotAm : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40201, 40202, 40203, 40204, 40205, 40206, 40207, 40208, 40209, 40210 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50201, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50220, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50220, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50222, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50224, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50226, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "AM"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
