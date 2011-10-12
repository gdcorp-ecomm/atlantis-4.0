using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotBe
{
  public class DotBe : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 12541, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 12542, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 12543, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 12512, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 22530, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 22530, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 22532, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 22534, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 22536, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "BE"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }
  }
}
