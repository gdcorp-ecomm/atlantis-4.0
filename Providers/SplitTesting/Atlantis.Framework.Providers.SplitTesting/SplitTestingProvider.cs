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

    private readonly Lazy<SplitTestingCookie> _splitTestCookie;
    private readonly Dictionary<string, string> _splitTestCookieData;

    private readonly Lazy<ActiveSplitTestsResponseData> _activeSplitTestsResponse;
    private readonly Dictionary<string, string> _activeSidesByTests;

    private readonly ExpressionParserManager _expressionParserManager;

    public SplitTestingProvider(IProviderContainer container)
      : base(container)
    {
      _splitTestCookie = new Lazy<SplitTestingCookie>(() => new SplitTestingCookie(Container));
      _splitTestCookieData = _splitTestCookie.Value.Value;

      _activeSplitTestsResponse = new Lazy<ActiveSplitTestsResponseData>(LoadActiveTests);
      _activeSidesByTests = new Dictionary<string, string>();

      _expressionParserManager = new ExpressionParserManager(Container);
      _expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      RefreshCookieData();
    }

    private ActiveSplitTestsResponseData LoadActiveTests()
    {
      try
      {
        var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, SplitTestingConfiguration.DefaultCategoryName);
        return (ActiveSplitTestsResponseData)DataCache.DataCache.GetProcessRequest(request, SplitTestingEngineRequests.ActiveSplitTests);
      }
      catch (Exception ex)
      {
        SplitTestingConfiguration.LogError(GetType().Name + ".LoadActiveTests", ex);
        return null;
      }
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

    private void RefreshCookieData()
    {
      if (_splitTestCookieData != null && 
          _activeSplitTestsResponse.Value != null && _activeSplitTestsResponse.Value.SplitTests.Any())
      {
        var keysToRemove = new List<string>();
        var activeTests = _activeSplitTestsResponse.Value.SplitTests.ToList();

        GetCookieDataKeysToRemove(activeTests, keysToRemove);

        foreach (var item in keysToRemove)
        {
          _splitTestCookieData.Remove(item);
        }

        _splitTestCookie.Value.Value = _splitTestCookieData;
      }
    }

    private void GetCookieDataKeysToRemove(List<ActiveSplitTest> activeTests, ICollection<string> keysToRemove)
    {
      foreach (var cookieData in _splitTestCookieData)
      {
        var found = false;
        foreach (var activeTest in activeTests)
        {
          var key = string.Format("{0}-{1}", activeTest.TestId.ToString(CultureInfo.InvariantCulture), activeTest.VersionNumber.ToString(CultureInfo.InvariantCulture));

          if (cookieData.Key == key)
          {
            found = true;
            break;
          }
        }

        if (!found)
        {
          keysToRemove.Add(cookieData.Key);
        }
      }
    }

    private bool IsActiveTest(int splitTestId, out int versionNumber)
    {
      var result = false;
      versionNumber = 0;

      if (_activeSplitTestsResponse.Value != null && _activeSplitTestsResponse.Value.SplitTests.Any())
      {
        var activeSplitTests = _activeSplitTestsResponse.Value.SplitTests;

        try
        {
          foreach (var activeSplitTest in activeSplitTests)
          {
            if (activeSplitTest.TestId == splitTestId)
            {
              if (( !string.IsNullOrEmpty(activeSplitTest.EligibilityRules) && _expressionParserManager.EvaluateExpression(activeSplitTest.EligibilityRules) ) ||
                    string.IsNullOrEmpty(activeSplitTest.EligibilityRules)
                 )
              {
                versionNumber = activeSplitTest.VersionNumber;
                result = true;
                break;
              }
            }
          }
        }
        catch (Exception ex)
        {
          SplitTestingConfiguration.LogError(GetType().Name + ".IsActiveTest", ex);
        }
      }

      return result;
    }

    public string GetSplitTestingSide(int splitTestId)
    {
      var side = string.Empty;

      int versionNumber;
      if (IsActiveTest(splitTestId, out versionNumber) && versionNumber > 0)
      {
        var key = string.Format("{0}-{1}", splitTestId.ToString(CultureInfo.InvariantCulture), versionNumber.ToString(CultureInfo.InvariantCulture));

        side = GetFromCookie(key);
        if (string.IsNullOrEmpty(side))
        {
          side = GetFromCurrentRequestCache(key);
        }

        if (string.IsNullOrEmpty(side))
        {
          side = GetFromTriplet(splitTestId, key);
        }
      }

      return side;
    }

    private string GetFromTriplet(int splitTestId, string key)
    {
      string side = null;
      var activeTestDetailsResponse = LoadActiveTestDetails(splitTestId);

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

            if (_activeSidesByTests != null)
            {
              _activeSidesByTests[key] = side;
            }

            _splitTestCookieData[key] = side;
            _splitTestCookie.Value.Value = _splitTestCookieData;
            break;
          }
        }
      }
      return side;
    }

    private string GetFromCurrentRequestCache(string key)
    {
      string side = string.Empty;
      if (_activeSidesByTests != null)
      {
        string value;
        if (_activeSidesByTests.TryGetValue(key, out value))
        {
          side = value;
        }
      }
      return side;
    }

    private string GetFromCookie(string key)
    {
      string side = string.Empty;
      if (_splitTestCookie.Value.Value != null)
      {
        string value;
        if (_splitTestCookie.Value.Value.TryGetValue(key, out value))
        {
          side = value;
        }
      }
      return side;
    }
  }
}
