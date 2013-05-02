using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  internal class SplitTestingState 
  {
    private readonly Lazy<SplitTestingCookie> _splitTestCookie;
    private readonly ActiveSplitTestsResponseData _activeSplitTestsResponse;

    internal SplitTestingState(IProviderContainer container, ActiveSplitTestsResponseData activeSplitTestsResponseData)
    {
      _splitTestCookie = new Lazy<SplitTestingCookie>(() => new SplitTestingCookie(container));
      _activeSplitTestsResponse = activeSplitTestsResponseData;

      RefreshData();
    }

    internal Dictionary<string, string> Value
    {
      get { return _splitTestCookie.Value.Value; }

      set { _splitTestCookie.Value.Value = value; }
    }
    
    private void RefreshData()
    {
      var cookieData = _splitTestCookie.Value.Value;

      if (cookieData != null &&
          _activeSplitTestsResponse != null && _activeSplitTestsResponse.SplitTests.Any())
      {
        var keysToRemove = new List<string>();
        var activeTests = _activeSplitTestsResponse.SplitTests.ToList();

        GetCookieDataKeysToRemove(activeTests, keysToRemove);

        foreach (var item in keysToRemove)
        {
          cookieData.Remove(item);
        }

        _splitTestCookie.Value.Value = cookieData;
      }
    }

    private void GetCookieDataKeysToRemove(List<ActiveSplitTest> activeTests, ICollection<string> keysToRemove)
    {
      var cookieData = _splitTestCookie.Value.Value;

      foreach (var data in cookieData)
      {
        var found = false;
        foreach (var activeTest in activeTests)
        {
          var key = string.Format("{0}-{1}", activeTest.TestId.ToString(CultureInfo.InvariantCulture), activeTest.VersionNumber.ToString(CultureInfo.InvariantCulture));

          if (data.Key == key)
          {
            found = true;
            break;
          }
        }

        if (!found)
        {
          keysToRemove.Add(data.Key);
        }
      }
    }

  }
}
