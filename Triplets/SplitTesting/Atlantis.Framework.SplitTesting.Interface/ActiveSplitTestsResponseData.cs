﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTestsResponseData : IResponseData
  {
    public static ActiveSplitTestsResponseData Empty {get; private set;}

    static ActiveSplitTestsResponseData()
    {
      Empty = new ActiveSplitTestsResponseData();
    }

    private readonly AtlantisException _exception;
    private readonly IEnumerable<ActiveSplitTest> _activeSplitTests;

    public static ActiveSplitTestsResponseData FromException(AtlantisException exception)
    {
      return new ActiveSplitTestsResponseData(exception);
    }

    private ActiveSplitTestsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public static ActiveSplitTestsResponseData FromCacheXml(string cacheXml)
    {
      var activeSplits = new List<ActiveSplitTest>();

      if (!string.IsNullOrEmpty(cacheXml))
      {
        try
        {
          XElement splitTestList = XElement.Parse(cacheXml);
          foreach (var itemElement in splitTestList.Elements("item"))
          {
            var testidAtt = itemElement.Attribute("SplitTestID");
            var versionnumberAtt = itemElement.Attribute("VersionNumber");
            var eligibilityrulesAtt = itemElement.Attribute("EligibilityRules");
            var runidAtt = itemElement.Attribute("SplitTestRunID");
            var startdateAtt = itemElement.Attribute("TestStartDate");

            if (string.IsNullOrEmpty(testidAtt.Value) || string.IsNullOrEmpty(versionnumberAtt.Value) ||
                string.IsNullOrEmpty(runidAtt.Value) || string.IsNullOrEmpty(startdateAtt.Value))
            {
              const string message = "Xml with invalid SplitTestID, VersionNumber, itemElement or TestStartDate";
              var aex = new AtlantisException("ActiveSplitTestsResponseData.ctor", "0", message, itemElement.ToString(),
                                              null, null);
              Engine.Engine.LogAtlantisException(aex);

              continue;
            }

            var testid = testidAtt.Value;
            var versionnumber = versionnumberAtt.Value;
            var eligibilityrules = eligibilityrulesAtt.Value;
            var runid = runidAtt.Value;
            var startdate = startdateAtt.Value;

            if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(startdate)) > 0)
            {
              var ast = new ActiveSplitTest
                          {
                            TestId = Convert.ToInt32(testid),
                            RunId = Convert.ToInt32(runid),
                            VersionNumber = Convert.ToInt32(versionnumber),
                            EligibilityRules = eligibilityrules,
                            StartDate = Convert.ToDateTime(startdate)
                          };

              activeSplits.Add(ast);
            }
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("ActiveSplitTestsResponseData.FromCacheXml", "0", ex.Message + ex.StackTrace, cacheXml, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      if (activeSplits.Count == 0)
      {
        return Empty;
      }

      return new ActiveSplitTestsResponseData(activeSplits);
    }

    private ActiveSplitTestsResponseData()
    {
      _activeSplitTests = new List<ActiveSplitTest>();
    }

    private ActiveSplitTestsResponseData(IEnumerable<ActiveSplitTest> splitTests)
    {
      _activeSplitTests = splitTests;
    }

    public IEnumerable<ActiveSplitTest> SplitTests
    {
      get { return _activeSplitTests; }
    }

    public string ToXML()
    {
      var rootElement = new XElement("ActiveSplitTestsData");

      if (_activeSplitTests != null)
      {
        foreach (var splittest in _activeSplitTests)
        {
          var testElement = new XElement("SplitTest");
          testElement.Add(new XAttribute("TestId", splittest.TestId));
          testElement.Add(new XAttribute("RunId", splittest.RunId));
          testElement.Add(new XAttribute("VersionNumber", splittest.VersionNumber));
          testElement.Add(new XAttribute("EligibilityRules", splittest.EligibilityRules));

          rootElement.Add(testElement);
        }
      }
      return rootElement.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}