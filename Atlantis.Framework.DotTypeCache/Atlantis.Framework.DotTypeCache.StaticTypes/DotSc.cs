using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotSc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40451, 40452, 40453, 40454, 40455, 40456, 40457, 40458, 40459, 40460 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50451, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50470, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50470, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50472, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50474, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50476, 50452, 50453, 50454, 50455, 50456, 50457, 50458, 50459, 50460 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "SC"; }
    }
  }
}
