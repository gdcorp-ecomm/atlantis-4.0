using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotDe
{
  public class DotDe : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12601, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9500, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9501, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12632, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12634, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12636, 12602, 12603, 12604, 12605, 12606, 12607, 12608, 12609, 12610 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12611, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9502, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9503, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12641, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12642, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12643, 12622, 12623, 12624, 12625, 12626, 12627, 12628, 12629 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12612, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 19500, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 19501, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22632, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22634, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22636, 12613, 12614, 12615, 12616, 12617, 12618, 12619, 12620, 12621 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "DE"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }
  }
}
