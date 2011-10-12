using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotIn
{
  public class DotNetDotIn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41400, 41401, 41402, 41403, 41404, 41405, 41406, 41407, 41408, 41409 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41430, 41439, 41445, 41463, 41451, 41469, 41475, 41481, 41487, 41457 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41431, 41440, 41446, 41464, 41452, 41470, 41476, 41482, 41488, 41458 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41421, 41441, 41447, 41465, 41453, 41471, 41477, 41483, 41489, 41459 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41423, 41442, 41448, 41466, 41454, 41472, 41478, 41484, 41490, 41460 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41425, 41443, 41449, 41467, 41455, 41473, 41479, 41485, 41491, 41461 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41410, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41432, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41433, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41427, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41428, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41429, 41411, 41412, 41413, 41414, 41415, 41416, 41417, 41418 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 51400, 51401, 51402, 51403, 51404, 51405, 51406, 51407, 51408, 51409 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 51430, 51439, 51445, 51463, 51451, 51469, 51475, 51481, 51487, 51457 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 51431, 51440, 51446, 51464, 51452, 51470, 51476, 51482, 51488, 51458 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 51421, 51441, 51447, 51465, 51453, 51471, 51477, 51483, 51489, 51459 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 51423, 51442, 51448, 51466, 51454, 51472, 51478, 51484, 51490, 51460 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 51425, 51443, 51449, 51467, 51455, 51473, 51479, 51485, 51491, 51461 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.IN"; }
    }
  }
}
