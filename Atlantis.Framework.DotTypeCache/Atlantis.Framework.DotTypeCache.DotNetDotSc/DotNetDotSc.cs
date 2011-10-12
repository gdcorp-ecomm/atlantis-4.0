using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotSc
{
  public class DotNetDotSc : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40551, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40570, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40570, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40572, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40574, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40576, 40552, 40553, 40554, 40555, 40556, 40557, 40558, 40559, 40560 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50551, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50570, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50570, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50572, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50574, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50576, 50552, 50553, 50554, 50555, 50556, 50557, 50558, 50559, 50560 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.SC"; }
    }
  }
}
