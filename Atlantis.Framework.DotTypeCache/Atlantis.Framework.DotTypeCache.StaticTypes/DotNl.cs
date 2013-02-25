using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNl : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 43001, 43002, 43003, 43004, 43005, 43006, 43007, 43008, 43009, 43010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 43020, 43036, 43042, 43060, 43048, 43066, 43072, 43078, 43084, 43054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 43021, 43037, 43043, 43061, 43049, 43067, 43073, 43079, 43085, 43055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 43024, 43038, 43044, 43062, 43050, 43068, 43074, 43080, 43086, 43056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 43026, 43039, 43045, 43063, 43051, 43069, 43075, 43081, 43087, 43057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 43028, 43040, 43046, 43064, 43052, 43070, 43076, 43082, 43088, 43058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 43011, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 43029, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 43030, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 43032, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 43033, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 43034, 43012, 43013, 43014, 43015, 43016, 43017, 43018, 43019 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 53001, 53002, 53003, 53004, 53005, 53006, 53007, 53008, 53009, 53010 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 53020, 53036, 53042, 53060, 53048, 53066, 53072, 53078, 53084, 53054 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 53021, 53037, 53043, 53061, 53049, 53067, 53073, 53079, 53085, 53055 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 53024, 53038, 53044, 53062, 53050, 53068, 53074, 53080, 53086, 53056 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 53026, 53039, 53045, 53063, 53051, 53069, 53075, 53081, 53087, 53057 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 53028, 53040, 53046, 53064, 53052, 53070, 53076, 53082, 53088, 53058 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NL"; }
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
      get { return 18; }
    }
  }
}
