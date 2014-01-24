using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingProvider : ProviderBase, ISplitTestingProvider
  {
    private const string NOT_ELIGIBLE_SIDE_ID = "0";
    private static readonly Random rand = new Random();
    private static readonly IActiveSplitTestSide _defaultSideNotEligible = new ActiveSplitTestSide { Allocation = 100D, Name = "A", SideId = 0 };
    private static readonly IActiveSplitTestSide _defaultSideA = new ActiveSplitTestSide {Allocation = 100D, Name = "A", SideId = -1};
    private static readonly IActiveSplitTestSide _defaultSideBot = new ActiveSplitTestSide { Allocation = 100D, Name = "A", SideId = -2 };
    
    private readonly IProviderContainer _container;
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private Lazy<SplitTestingState> _splitTestingState;

    private readonly Lazy<ActiveSplitTestsResponseData> _activeSplitTestsResponse;
    private readonly Lazy<SplitTestingCookieOverride> _splitTestCookieOverride;
    private readonly Lazy<IUserAgentDetectionProvider> _splitTestUserAgentDetection;

    private readonly Lazy<SplitTestingRequestCache> _requestCache;

    private readonly ExpressionParserManager _expressionParserManager;

    public SplitTestingProvider(IProviderContainer container)
            : base(container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => _container.Resolve<IShopperContext>());

      if (_container.CanResolve<IUserAgentDetectionProvider>())
      {
        _splitTestUserAgentDetection = new Lazy<IUserAgentDetectionProvider>(() => _container.Resolve<IUserAgentDetectionProvider>());
      }

      _activeSplitTestsResponse = new Lazy<ActiveSplitTestsResponseData>(LoadActiveTests);
      _splitTestCookieOverride = new Lazy<SplitTestingCookieOverride>(() => new SplitTestingCookieOverride(container));

      _requestCache = new Lazy<SplitTestingRequestCache>(() => new SplitTestingRequestCache(container));

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

    private bool IsEligibleForTest(IActiveSplitTest activeSplitTest)
    {
      bool isEligible;
      try
      {
        isEligible = (!string.IsNullOrEmpty(activeSplitTest.EligibilityRules) && _expressionParserManager.EvaluateExpression(activeSplitTest.EligibilityRules)) ||
             string.IsNullOrEmpty(activeSplitTest.EligibilityRules);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("SplitTestingProvider.IsEligibleForTest", string.Empty, ex.Message, ex.StackTrace, _siteContext.Value, _shopperContext.Value));
        isEligible = false;
      }
      return isEligible;
    }

    public IActiveSplitTestSide GetSplitTestingSide(int splitTestId)
    {
      IActiveSplitTestSide splitTestSide = null;
      try {
        if (_siteContext.Value.IsRequestInternal)
        {
          foreach (var overrideSide in _splitTestCookieOverride.Value.CookieValues)
          {
            if (overrideSide.Key == splitTestId.ToString())
            {
              return new ActiveSplitTestSide {Allocation = 100D, Name = overrideSide.Value.ToUpper(), SideId = -1};
            }
          }
        }

        if (_siteContext.Value.Manager != null && _siteContext.Value.Manager.IsManager)
        {
          return _defaultSideA;
        }

        if (_splitTestUserAgentDetection != null && _splitTestUserAgentDetection.Value.IsSearchEngineBot(SplitTestingUserAgent.UserAgent))
        {
          return _defaultSideBot;
        }

        IActiveSplitTest activeSplitTest;
        if (!IsActiveTest(splitTestId, out activeSplitTest) || activeSplitTest == null || activeSplitTest.VersionNumber < 1)
        {
          return _defaultSideA;
        }
        
        var key = CreateStateKey(splitTestId, activeSplitTest);
        var sideId = GetSplitSideIdFromState(key) ?? string.Empty;
        if (sideId == NOT_ELIGIBLE_SIDE_ID || !IsEligibleForTest(activeSplitTest))
        {
          UpdateState(key, NOT_ELIGIBLE_SIDE_ID);
          return _defaultSideNotEligible;
        }
        switch (sideId)
        {
          case "":
            sideId = GetSplitSideIdFromTriplet(activeSplitTest);
            if (!string.IsNullOrEmpty(sideId))
            {
              UpdateRequestCache(activeSplitTest, sideId);
              UpdateState(key, sideId);
            }
            break;
          default:
            UpdateRequestCache(activeSplitTest, sideId);
            UpdateState(key, sideId);
            break;
        }
        splitTestSide = FindIActiveSplitTestSide(splitTestId, sideId);
        
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("SplitTestingProvider.GetSplitTestingSide", string.Empty, ex.Message, ex.StackTrace, _siteContext.Value, _shopperContext.Value));
      }
      return splitTestSide ?? _defaultSideA;
    }

    private IActiveSplitTestSide FindIActiveSplitTestSide(int splitTestId, string sideId)
    {
      IActiveSplitTestSide splitTestSide = null;
      if (!string.IsNullOrEmpty(sideId))
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
      return splitTestSide;
    }

    public Dictionary<IActiveSplitTest, IActiveSplitTestSide> GetTrackingDictionary
    {
      get
      {
        var dict = new Dictionary<IActiveSplitTest, IActiveSplitTestSide>();
        foreach (var info in _requestCache.Value.GetCacheValues())
        {
          var splitInfo = info.Split('.');
          var testId = Convert.ToInt32(splitInfo[0]);
          IActiveSplitTest test;
          if (_activeSplitTestsResponse.Value.TryGetSplitTestByTestId(testId, out test))
          {
            var side = FindIActiveSplitTestSide(testId, splitInfo[3]);
            if (side != null)
            {
              dict.Add(test, side);
            }
          }
        }
        return dict;
      }
    }

    public string GetTrackingData
    {
      get
      {
        const string separator = "^";
        
        var result = new StringBuilder();
        foreach (var info in _requestCache.Value.GetCacheValues())
        {
          result.Append(info).Append(separator);
        }
        if (result.Length > 1)
        {
          result.Remove(result.Length - 1, 1);
        }

        _requestCache.Value.ClearCache();
        return result.ToString();
      }
    }

    /// <summary>
    /// Returns all active tests for the default category as set by <code>SplitTestingConfiguration.DefaultCategoryName</code>
    /// </summary>
    public IEnumerable<IActiveSplitTest> GetAllActiveTests
    {
      get
      {
        return _activeSplitTestsResponse.Value != null ? _activeSplitTestsResponse.Value.SplitTests : new List<IActiveSplitTest>();
      }
    }

    private string GetSplitSideIdFromTriplet(IActiveSplitTest splitTest)
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

    private string GetSplitSideIdFromState(string key)
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
      const string template = "{0}.{1}.{2}.{3}";
      var value = string.Format(template, splitTest.TestId, splitTest.VersionNumber, splitTest.RunId, sideId);
      _requestCache.Value.AddCacheValues(value);
    }

    private static string CreateStateKey(int splitTestId, IActiveSplitTest activeSplitTest)
    {
      var key = string.Format("{0}-{1}", splitTestId.ToString(CultureInfo.InvariantCulture),
                              activeSplitTest.VersionNumber.ToString(CultureInfo.InvariantCulture));
      return key;
    }

    public bool SetOverrideSide(int splitTestId, string overrideSideName)
    {
      bool success = false;
      IActiveSplitTest splitTest;
      if (IsActiveTest(splitTestId, out splitTest))
      {
        success = SetDefaultCookie(splitTest, overrideSideName);
      }
      else
      {
        success = SetOverrideCookie(splitTestId, overrideSideName);
      }
      return success;
    }

    private bool SetDefaultCookie(IActiveSplitTest splitTest, string overrideSideName)
    {
      bool success = false;
      var activeTestDetailsResponse = LoadActiveTestDetails(splitTest.TestId);
      foreach (var side in activeTestDetailsResponse.SplitTestDetails)
      {
        if (side.Name.Equals(overrideSideName, StringComparison.OrdinalIgnoreCase))
        {
          int overrideSideId = side.SideId;
          success = true;
          UpdateState(CreateStateKey(splitTest.TestId, splitTest), overrideSideId.ToString());
          break;
        }
      }
      return success;
    }

    private bool SetOverrideCookie(int splitTestId, string overrideSideName)
    {
      var v = new Dictionary<string, string> {{splitTestId.ToString(), overrideSideName}};
      _splitTestCookieOverride.Value.CookieValues = v;
      return true;
    }
  }
}