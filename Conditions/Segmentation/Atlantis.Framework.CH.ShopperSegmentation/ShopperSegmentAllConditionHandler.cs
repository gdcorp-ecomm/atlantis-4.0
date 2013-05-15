using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Segmentation.Interface;

namespace Atlantis.Framework.CH.ShopperSegment
{
  public class ShopperSegmentAllConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "shopperSegmentAll"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool returnValue = false;

      if (!ReferenceEquals(null, parameters) && 0 < parameters.Count)
      {
        ISegmentationProvider segmentProvider = providerContainer.Resolve<ISegmentationProvider>();
        if (!ReferenceEquals(null, segmentProvider))
        {
          int segmentId = segmentProvider.GetShopperSegmentId();

          foreach (var item in parameters)
          {
            var parsed = -1;
            if (int.TryParse(item, out parsed))
            {
              returnValue = parsed == segmentId;
            }
            if (!returnValue)
            {
              break;
            }
          }
        }
      }

      return returnValue;
    }
  }
}

