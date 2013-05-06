using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
    private const string NotEligibleSideId = "0";
    private static readonly Random rand = new Random();
    private static readonly IActiveSplitTestSide _zeroSideInstance = new ActiveSplitTestSide {SideId = 0, Name = "0", Allocation = 0D};

    private readonly IProviderContainer _container;
    private Lazy<SplitTestingState> _splitTestingState;

    private readonly Lazy<ActiveSplitTestsResponseData> _activeSplitTestsResponse;

    private readonly Dictionary<IActiveSplitTest, IActiveSplitTestSide> _sidesByTestsForRequest;

    private readonly ExpressionParserManager _expressionParserManager;

    public SplitTestingProvider(IProviderContainer container)
      : base(container)
    {
      _container = container;

      _activeSplitTestsResponse = new Lazy<ActiveSplitTestsResponseData>(LoadActiveTests);

      _sidesByTestsForRequest = new Dictionary<IActiveSplitTest, IActiveSplitTestSide>();

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

    private bool IsActiveTest(int splitTestId, out IActiveSplitTest result)
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

    private bool IsEligibleTest(IActiveSplitTest activeSplitTest)
    {
      return (!string.IsNullOrEmpty(activeSplitTest.EligibilityRules) && _expressionParserManager.EvaluateExpression(activeSplitTest.EligibilityRules)) || 
            string.IsNullOrEmpty(activeSplitTest.EligibilityRules);
    }

    public IActiveSplitTestSide GetSplitTestingSide(int splitTestId)
    {
      IActiveSplitTestSide splitTestSide = null;

      IActiveSplitTest activeSplitTest;
      if (IsActiveTest(splitTestId, out activeSplitTest) && activeSplitTest != null && activeSplitTest.VersionNumber > 0)
      {
        var key = string.Format("{0}-{1}", splitTestId.ToString(CultureInfo.InvariantCulture), activeSplitTest.VersionNumber.ToString(CultureInfo.InvariantCulture));

        string sideId = GetSplitSideFromState(key);
        if (sideId != NotEligibleSideId)
        {
          if (!IsEligibleTest(activeSplitTest))
          {
            if (!string.IsNullOrEmpty(sideId))
            {
              UpdateRequestCache(activeSplitTest, NotEligibleSideId);
              UpdateState(key, NotEligibleSideId);
            }
          }
          else
          {
            if (!string.IsNullOrEmpty(sideId))
            {
              UpdateRequestCache(activeSplitTest, sideId);
            }
            else
            {
              sideId = GetSplitSideFromTriplet(activeSplitTest);
              if (!string.IsNullOrEmpty(sideId))
              {
                UpdateRequestCache(activeSplitTest, sideId);
                UpdateState(key, sideId);
              }
            }
          }
        }

        splitTestSide = GetActiveSplitTestSide(splitTestId, sideId);

      }

      return splitTestSide;
    }

    private IActiveSplitTestSide GetActiveSplitTestSide(int splitTestId, string sideId)
    {
      IActiveSplitTestSide splitTestSide = null;

      if (!string.IsNullOrEmpty(sideId))
      {
        if (sideId != NotEligibleSideId)
        {
          int iSideId;
          if (Int32.TryParse(sideId, out iSideId) && iSideId > 0)
          {
            var activeTestDetailsResponse = LoadActiveTestDetails(splitTestId);

            if (activeTestDetailsResponse != null && activeTestDetailsResponse.SplitTestDetails.Any())
            {
              var details = activeTestDetailsResponse.SplitTestDetails.ToList();
              foreach (var dtl in details)
              {
                if (dtl.SideId == iSideId)
                {
                  splitTestSide = dtl;
                }
              }
            }
          }
        }
        else
        {
          splitTestSide = _zeroSideInstance;
        }
      }
      return splitTestSide;
    }

    public Dictionary<IActiveSplitTest, IActiveSplitTestSide> GetTrackingDictionary
    {
      get { return _sidesByTestsForRequest; }
    }

    public string GetTrackingData
    {
      get
      {
        const string format = "{0}.{1}.{2}.{3}";
        const string separator = "^";

        var result = new StringBuilder();
        foreach (var info in _sidesByTestsForRequest)
        {
          var splitTest = info.Key;
          var sideId = info.Value.SideId;

          if (result.Length > 0)
          {
            result.Append(separator);
          }
          result.Append(string.Format(format, splitTest.TestId, splitTest.RunId, splitTest.VersionNumber, sideId));

        }
        return result.ToString();
      }
    }

    public IEnumerable<IActiveSplitTest> GetAllActiveTests
    {
      get
      {
        return _activeSplitTestsResponse.Value != null ? _activeSplitTestsResponse.Value.SplitTests : new List<IActiveSplitTest>();
      }
    }

    private string GetSplitSideFromTriplet(IActiveSplitTest splitTest)
    {
      var sideId = string.Empty;
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
            sideId = dtl.SideId.ToString(CultureInfo.InvariantCulture);

            break;
          }
        }
      }
      return sideId;
    }

    private string GetSplitSideFromState(string key)
    {
      var sideId = string.Empty;
      if (_splitTestingState.Value.Value != null)
      {
        string value;
        if (_splitTestingState.Value.Value.TryGetValue(key, out value))
        {
          sideId = value;
        }
      }
      return sideId;
    }

    private void UpdateState(string key, string sideId)
    {
      var splitTestingStateData = _splitTestingState.Value.Value;
      splitTestingStateData[key] = sideId;
      _splitTestingState.Value.Value = splitTestingStateData;
    }

    private void UpdateRequestCache(IActiveSplitTest splitTest, string sideId)
    {
      IActiveSplitTestSide splitTestSide = GetActiveSplitTestSide(splitTest.TestId, sideId);
      _sidesByTestsForRequest[splitTest] = splitTestSide;
    }
  }
}
