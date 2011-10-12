using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotTk
{
  public class DotTk : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50701, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50720, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50720, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50722, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50724, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50726, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "TK"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }
  }
}
