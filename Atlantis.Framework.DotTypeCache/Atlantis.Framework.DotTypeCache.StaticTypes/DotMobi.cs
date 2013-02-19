using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotMobi : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40801, 40802, 40803, 40804, 40805, 40806, 40807, 40808, 40809, 40810 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40840, 40851, 40861, 40871, 40881, 40891, 40901, 40911, 40921, 40931 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40841, 40852, 40862, 40872, 40882, 40892, 40902, 40912, 40922, 40932 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40822, 40853, 40863, 40873, 40883, 40893, 40903, 40913, 40923, 40933 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40824, 40854, 40864, 40874, 40884, 40894, 40904, 40914, 40924, 40934 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40826, 40855, 40865, 40875, 40885, 40895, 40905, 40915, 40925, 40935 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40811, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40831, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40832, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40833, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40835, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40836, 40812, 40813, 40814, 40815, 40816, 40817, 40818, 40819 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50801, 50802, 50803, 50804, 50805, 50806, 50807, 50808, 50809, 50810 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50840, 50851, 50861, 50871, 50881, 50891, 50901, 50911, 50921, 50931 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50841, 50852, 50862, 50872, 50882, 50892, 50902, 50912, 50922, 50932 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50822, 50853, 50863, 50873, 50883, 50893, 50903, 50913, 50923, 50933 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50824, 50854, 50864, 50874, 50884, 50894, 50904, 50914, 50924, 50934 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50826, 50855, 50865, 50875, 50885, 50895, 50905, 50915, 50925, 50935 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "MOBI"; }
    }
  }
}
