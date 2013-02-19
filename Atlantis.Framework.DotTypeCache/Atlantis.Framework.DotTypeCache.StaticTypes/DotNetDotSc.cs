using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotSc : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40551, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40570, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40570, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40572, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40574, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40576, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50551, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50570, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50570, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50572, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50574, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50576, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.SC"; }
    }
  }
}
