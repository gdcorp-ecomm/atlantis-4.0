using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotGenDotIn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41200, 41201, 41202, 41203, 41204, 41205, 41206, 41207, 41208, 41209 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41230, 41239, 41245, 41263, 41251, 41269, 41275, 41281, 41287, 41257 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41231, 41240, 41246, 41264, 41252, 41270, 41276, 41282, 41288, 41258 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41221, 41241, 41247, 41265, 41253, 41271, 41277, 41283, 41289, 41259 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41223, 41242, 41248, 41266, 41254, 41272, 41278, 41284, 41290, 41260 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41225, 41243, 41249, 41267, 41255, 41273, 41279, 41285, 41291, 41261 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41210, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41232, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41233, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41227, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41228, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41229, 41211, 41212, 41213, 41214, 41215, 41216, 41217, 41218 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 51200, 51201, 51202, 51203, 51204, 51205, 51206, 51207, 51208, 51209 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51230, 51239, 51245, 51263, 51251, 51269, 51275, 51281, 51287, 51257 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51231, 51240, 51246, 51264, 51252, 51270, 51276, 51282, 51288, 51258 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 51221, 51241, 51247, 51265, 51253, 51271, 51277, 51283, 51289, 51259 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 51223, 51242, 51248, 51266, 51254, 51272, 51278, 51284, 51290, 51260 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 51225, 51243, 51249, 51267, 51255, 51273, 51279, 51285, 51291, 51261 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "GEN.IN"; }
    }
  }
}
