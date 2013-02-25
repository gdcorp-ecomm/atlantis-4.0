using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotCo : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56501, 56502, 56503, 56504, 56505, 56506, 56507, 56508, 56509, 56510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56520, 56536, 56542, 56560, 56548, 56566, 56572, 56578, 56584, 56554 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56521, 56537, 56543, 56561, 56549, 56567, 56573, 56579, 56585, 56555 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56524, 56538, 56544, 56562, 56550, 56568, 56574, 56580, 56586, 56556 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56526, 56539, 56545, 56563, 56551, 56569, 56575, 56581, 56587, 56557 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56528, 56540, 56546, 56564, 56552, 56570, 56576, 56582, 56588, 56558 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56511, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56529, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56530, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56532, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56533, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56534, 56512, 56513, 56514, 56515, 56516, 56517, 56518, 56519 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 66501, 66502, 66503, 66504, 66505, 66506, 66507, 66508, 66509, 66510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 66520, 66536, 66542, 66560, 66548, 66566, 66572, 66578, 66584, 66554 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 66521, 66537, 66543, 66561, 66549, 66567, 66573, 66579, 66585, 66555 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 66524, 66538, 66544, 66562, 66550, 66568, 66574, 66580, 66586, 66556 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 66526, 66539, 66545, 66563, 66551, 66569, 66575, 66581, 66587, 66557 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 66528, 66540, 66546, 66564, 66552, 66570, 66576, 66582, 66588, 66558 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.CO"; }
    }

    public override int MinRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MinRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 60; }
    }

    public override int MaxTransferLength
    {
      get { return 4; }
    }

  }
}
