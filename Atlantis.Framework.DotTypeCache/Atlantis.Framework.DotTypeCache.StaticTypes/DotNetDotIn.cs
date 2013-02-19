using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotIn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41400, 41401, 41402, 41403, 41404, 41405, 41406, 41407, 41408, 41409 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41430, 41439, 41445, 41463, 41451, 41469, 41475, 41481, 41487, 41457 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41431, 41440, 41446, 41464, 41452, 41470, 41476, 41482, 41488, 41458 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41421, 41441, 41447, 41465, 41453, 41471, 41477, 41483, 41489, 41459 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41423, 41442, 41448, 41466, 41454, 41472, 41478, 41484, 41490, 41460 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41425, 41443, 41449, 41467, 41455, 41473, 41479, 41485, 41491, 41461 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 41410, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 41432, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 41433, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 41427, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 41428, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 41429, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 51400, 51401, 51402, 51403, 51404, 51405, 51406, 51407, 51408, 51409 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 51430, 51439, 51445, 51463, 51451, 51469, 51475, 51481, 51487, 51457 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 51431, 51440, 51446, 51464, 51452, 51470, 51476, 51482, 51488, 51458 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 51421, 51441, 51447, 51465, 51453, 51471, 51477, 51483, 51489, 51459 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 51423, 51442, 51448, 51466, 51454, 51472, 51478, 51484, 51490, 51460 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 51425, 51443, 51449, 51467, 51455, 51473, 51479, 51485, 51491, 51461 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.IN"; }
    }
  }
}
