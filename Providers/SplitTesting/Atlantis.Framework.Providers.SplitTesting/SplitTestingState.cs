﻿using System;
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
    
    private Dictionary<string, string> _splitTestingStateData;

    internal SplitTestingState(IProviderContainer container, ActiveSplitTestsResponseData activeSplitTestsResponseData)
    {
      _splitTestCookie = new Lazy<SplitTestingCookie>(() => new SplitTestingCookie(container));
      _activeSplitTestsResponse = activeSplitTestsResponseData;

      RefreshData();
    }

    internal Dictionary<string, string> Value
    {
      get
      {
        return _splitTestingStateData;
      }

      set
      {
        _splitTestingStateData = value;
        _splitTestCookie.Value.Value = value;
      }
    }
    
    private void RefreshData()
    {
      _splitTestingStateData = _splitTestCookie.Value.Value;

      if (_splitTestingStateData != null &&
          _activeSplitTestsResponse != null && _activeSplitTestsResponse.SplitTests.Any())
      {
        var keysToRemove = new List<string>();
        var activeTests = _activeSplitTestsResponse.SplitTests.ToList();

        GetCookieDataKeysToRemove(activeTests, keysToRemove);

        if (keysToRemove.Count > 0)
        {
          foreach (var item in keysToRemove)
          {
            _splitTestingStateData.Remove(item);
          }

          _splitTestCookie.Value.Value = _splitTestingStateData;
        }
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
