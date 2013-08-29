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

      if (!ReferenceEquals(null, parameters) && 0 < parameters.Count)
      {
        ISegmentationProvider segmentProvider = providerContainer.Resolve<ISegmentationProvider>();
        if (!ReferenceEquals(null, segmentProvider))
        {
          returnValue = parameters.Contains(segmentProvider.GetShopperSegmentId().SegmentationName());
        }
      }
      return returnValue;
    }
  }
}
