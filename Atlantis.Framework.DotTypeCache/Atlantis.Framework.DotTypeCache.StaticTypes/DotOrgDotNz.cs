using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotNz : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13101, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13130, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13130, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13132, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13134, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13136, 13102, 13103, 13104, 13105, 13106, 13107, 13108, 13109, 13110 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13111, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13140, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13140, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13141, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13142, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13143, 13122, 13123, 13124, 13125, 13126, 13127, 13128, 13129 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13112, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 23130, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 23130, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23132, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23134, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23136, 13113, 13114, 13115, 13116, 13117, 13118, 13119, 13120, 13121 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.NZ"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1;}
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
