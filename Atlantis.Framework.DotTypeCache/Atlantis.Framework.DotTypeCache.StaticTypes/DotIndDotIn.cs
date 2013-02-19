using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotIndDotIn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[] { 41300, 41301, 41302, 41303, 41304, 41305, 41306, 41307, 41308, 41309 }),
        new StaticDotTypeTier(6, new int[] { 41330, 41339, 41345, 41363, 41351, 41369, 41375, 41381, 41387, 41357 }),
        new StaticDotTypeTier(21, new int[] { 41331, 41340, 41346, 41364, 41352, 41370, 41376, 41382, 41388, 41358 }),
        new StaticDotTypeTier(50, new int[] { 41321, 41341, 41347, 41365, 41353, 41371, 41377, 41383, 41389, 41359 }),
        new StaticDotTypeTier(101, new int[] { 41323, 41342, 41348, 41366, 41354, 41372, 41378, 41384, 41390, 41360 }),
        new StaticDotTypeTier(201, new int[] { 41325, 41343, 41349, 41367, 41355, 41373, 41379, 41385, 41391, 41361 })
      });
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[] { 41310, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 }),
        new StaticDotTypeTier(6, new int[] { 41332, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 }),
        new StaticDotTypeTier(21, new int[] { 41333, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 }),
        new StaticDotTypeTier(50, new int[] { 41327, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 }),
        new StaticDotTypeTier(101, new int[] { 41328, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 }),
        new StaticDotTypeTier(201, new int[] { 41329, 41311, 41312, 41313, 41314, 41315, 41316, 41317, 41318 })
      });
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[] { 51300, 51301, 51302, 51303, 51304, 51305, 51306, 51307, 51308, 51309 }),
        new StaticDotTypeTier(6, new int[] { 51330, 51339, 51345, 51363, 51351, 51369, 51375, 51381, 51387, 51357 }),
        new StaticDotTypeTier(21, new int[] { 51331, 51340, 51346, 51364, 51352, 51370, 51376, 51382, 51388, 51358 }),
        new StaticDotTypeTier(50, new int[] { 51321, 51341, 51347, 51365, 51353, 51371, 51377, 51383, 51389, 51359 }),
        new StaticDotTypeTier(101, new int[] { 51323, 51342, 51348, 51366, 51354, 51372, 51378, 51384, 51390, 51360 }),
        new StaticDotTypeTier(201, new int[] { 51325, 51343, 51349, 51367, 51355, 51373, 51379, 51385, 51391, 51361 })
      });
    }

    public override string DotType
    {
      get { return "IND.IN"; }
    }
  }
}
