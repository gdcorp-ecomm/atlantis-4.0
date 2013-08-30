using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Segmentation.Interface;

namespace Atlantis.Framework.CH.Segmentation
{
  public class ShopperSegmentAnyConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "shopperSegmentAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool returnValue = false;

      if (parameters.Count > 0)
      {
        IShopperSegmentationProvider segmentProvider = providerContainer.Resolve<IShopperSegmentationProvider>();
        if (!ReferenceEquals(null, segmentProvider))
        {
          var segment = segmentProvider.GetShopperSegmentId();
          foreach (var param in parameters)
          {
            if (segment.Equals(param, StringComparison.OrdinalIgnoreCase))
            {
              returnValue = true;
              break;
            }
          }
        }
      }
      return returnValue;
    }
  }
}
