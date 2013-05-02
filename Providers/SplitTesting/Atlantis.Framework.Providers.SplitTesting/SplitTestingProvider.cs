using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTest;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingProvider : ProviderBase, ISplitTestingProvider
  {
    private static readonly Random rand = new Random();

    private readonly IProviderContainer _container;
    private Lazy<SplitTestingState> _splitTestingState;

    private readonly Lazy<ActiveSplitTestsResponseData> _activeSplitTestsResponse;

    private readonly Dictionary<ActiveSplitTest, string> _sidesByTestsForRequest;

    private readonly ExpressionParserManager _expressionParserManager;

    public SplitTestingProvider(IProviderContainer container)
      : base(container)
    {
      _container = container;

      _activeSplitTestsResponse = new Lazy<ActiveSplitTestsResponseData>(LoadActiveTests);

      _sidesByTestsForRequest = new Dictionary<ActiveSplitTest, string>();

      _expressionParserManager = new ExpressionParserManager(Container);
      _expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
    }

    private ActiveSplitTestsResponseData LoadActiveTests()
    {
      ActiveSplitTestsResponseData result = null;
      try
      {
        var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, SplitTestingConfiguration.DefaultCategoryName);
        result = (ActiveSplitTestsResponseData)DataCache.DataCache.GetProcessRequest(request, SplitTestingEngineRequests.ActiveSplitTests);

        _splitTestingState = new Lazy<SplitTestingState>(() => new SplitTestingState(_container, result));
      }
      catch (Exception ex)
      {
        SplitTestingConfiguration.LogError(GetType().Name + ".LoadActiveTests", ex);
      }

      return result;
    }

    private ActiveSplitTestDetailsResponseData LoadActiveTestDetails(int splitTestId)
    {
      try
      {
        var request = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, splitTestId);
        return (ActiveSplitTestDetailsResponseData)DataCache.DataCache.GetProcessRequest(request, SplitTestingEngineRequests.ActiveSplitTestDetails);
      }
      catch (Exception ex)
      {
        SplitTestingConfiguration.LogError(GetType().Name + ".LoadActiveTestDetails", ex);
        return null;
      }
    }

    private bool IsActiveTest(int splitTestId, out ActiveSplitTest result)
    {
      var isActive = false;
      result = null;

      if (_activeSplitTestsResponse.Value != null)
      {
        if (_activeSplitTestsResponse.Value.TryGetSplitTestByTestId(splitTestId, out result))
        {
          isActive = true;
        }
      }

      return isActive;
    }

    private bool IsEligibleTest(ActiveSplitTest activeSplitTest)
    {
      return (!string.IsNullOrEmpty(activeSplitTest.EligibilityRules) && _expressionParserManager.EvaluateExpression(activeSplitTest.EligibilityRules)) || 
            string.IsNullOrEmpty(activeSplitTest.EligibilityRules);
    }

    public string GetSplitTestingSide(int splitTestId)
    {
      var side = string.Empty;

      ActiveSplitTest activeSplitTest;
      if (IsActiveTest(splitTestId, out activeSplitTest) && activeSplitTest != null && activeSplitTest.VersionNumber > 0)
      {
        var key = string.Format("{0}-{1}", splitTestId.ToString(CultureInfo.InvariantCulture), activeSplitTest.VersionNumber.ToString(CultureInfo.InvariantCulture));

        side = GetSplitSideFromState(key);
        if (side != "0")
        {
          if (!string.IsNullOrEmpty(side))
          {
            if (!IsEligibleTest(activeSplitTest))
            {
              UpdateRequestCache(activeSplitTest, "0");
              UpdateState(key, "0");
            }
            else
            {
              UpdateRequestCache(activeSplitTest, side);
            }
          }
          else
          {
            side = GetSplitSideFromTriplet(activeSplitTest, key);
            if (!string.IsNullOrEmpty(side))
            {
              UpdateRequestCache(activeSplitTest, side);
              UpdateState(key, side);
            }
          }
        }
      }

      return side;
    }

    public Dictionary<ActiveSplitTest, string> GetActiveTestsForTrackingData
    {
      get
      {
        return _sidesByTestsForRequest;
      }
    }

    public IEnumerable<ActiveSplitTest> GetAllActiveTests
    {
      get
      {
        return _activeSplitTestsResponse.Value != null ? _activeSplitTestsResponse.Value.SplitTests : new List<ActiveSplitTest>();
      }
    }

    private string GetSplitSideFromTriplet(ActiveSplitTest splitTest, string key)
    {
      var side = string.Empty;
      var activeTestDetailsResponse = LoadActiveTestDetails(splitTest.TestId);

      if (activeTestDetailsResponse != null && activeTestDetailsResponse.SplitTestDetails.Any())
      {
        var randomSplit = rand.NextDouble()*100;
        var totalAllocation = 0D;

        var details = activeTestDetailsResponse.SplitTestDetails;
        foreach (var dtl in details)
        {
          totalAllocation = totalAllocation + dtl.Allocation;
          if (totalAllocation >= randomSplit)
          {
            side = dtl.Name;

            break;
          }
        }
      }
      return side;
    }

    private string GetSplitSideFromState(string key)
    {
      var side = string.Empty;
      if (_splitTestingState.Value.Value != null)
      {
        string value;
        if (_splitTestingState.Value.Value.TryGetValue(key, out value))
        {
          side = value;
        }
      }
      return side;
    }

    private void UpdateState(string key, string side)
    {
      var splitTestingStateData = _splitTestingState.Value.Value;
      splitTestingStateData[key] = side;
      _splitTestingState.Value.Value = splitTestingStateData;
    }

    private void UpdateRequestCache(ActiveSplitTest splitTest, string side)
    {
      _sidesByTestsForRequest[splitTest] = side;
    }
  }
}
