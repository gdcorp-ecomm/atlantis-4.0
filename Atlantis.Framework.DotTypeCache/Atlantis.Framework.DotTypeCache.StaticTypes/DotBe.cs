using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotBe : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12501, 12502, 12503, 12504, 12505, 12506, 12507, 12508, 12509, 12510 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12511, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12541, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12542, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12543, 12522, 12523, 12524, 12525, 12526, 12527, 12528, 12529 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12512, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 22530, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 22530, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22532, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22534, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22536, 12513, 12514, 12515, 12516, 12517, 12518, 12519, 12520, 12521 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
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

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
