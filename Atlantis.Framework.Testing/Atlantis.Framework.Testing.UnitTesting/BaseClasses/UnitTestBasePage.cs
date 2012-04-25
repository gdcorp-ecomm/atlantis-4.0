using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for UnitTestPageBase
  /// </summary>
  public class UnitTestBasePage : System.Web.UI.Page, ITestResults
  {
    private const string UNITTEST_SHOPPERID_QS = "unittestshopper";
    private const string OUTPUT_XML_BASE = "<UnitTestData> <TestResults /> <ExtendedTestData /> </UnitTestData>";

    #region Properties

    public virtual bool RunDestructiveTests
    {
      get { return false; }
    }

    public virtual bool IsInternal
    {
      get { return SiteContext.IsRequestInternal; }
    }

    private string _testShopperId = string.Empty;
    public string TestShopperId
    {
      get
      {
        if (string.IsNullOrEmpty(_testShopperId))
        {
          if (!string.IsNullOrEmpty(Request[UNITTEST_SHOPPERID_QS]))
          {
            _testShopperId = Request[UNITTEST_SHOPPERID_QS];
          }
          else
          {
            _testShopperId = ShopperContext.ShopperId;
          }
        }

        return _testShopperId;

      }
      set { _testShopperId = value; }
    }

    private List<TestResultBase> _testResults;
    public List<TestResultBase> TestResults
    {
      get
      {
        if (_testResults == null)
        {
          _testResults = new List<TestResultBase>();
        }
        return _testResults;
      }
      set { _testResults = value; }
    }

    private Dictionary<string, string> _extLogData;
    public Dictionary<string, string> ExtendedLogData
    {
      get
      {
        if (_extLogData == null)
        {
          _extLogData = new Dictionary<string, string>();
        }
        return _extLogData;
      }
    }

    private ISiteContext _siteContext;
    protected virtual ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }

    #endregion

    #region public

    public string GetResultsXml()
    {
      var doc = new XmlDocument();

      doc.LoadXml(OUTPUT_XML_BASE);

      if (TestResults != null && TestResults.Count > 0)
      {
        var resultsNode = doc.SelectSingleNode("//TestResults");
        if (resultsNode == null)
        {
          resultsNode = doc.CreateElement("TestResults");
          doc.AppendChild(resultsNode);
          resultsNode = doc.SelectSingleNode("//TestResults");
        }

        foreach (var result in TestResults)
        {
          var resultNode = doc.CreateElement("TestResult");
          var successAttr = doc.CreateAttribute("Success");
          var testNameAttr = doc.CreateAttribute("Name");

          successAttr.Value = result.Success.ToString();
          testNameAttr.Value = result.TestName;

          if (resultsNode != null)
          {
            resultNode.Attributes.Append(testNameAttr);
            resultNode.Attributes.Append(successAttr);
            resultNode.InnerText = result.Result;

            resultsNode.AppendChild(resultNode);
          }
        }
      }

      if (ExtendedLogData != null && ExtendedLogData.Count > 0)
      {
        var logDataResultsNode = doc.SelectSingleNode("//ExtendedTestData");
        if (logDataResultsNode == null)
        {
          logDataResultsNode = doc.CreateElement("ExtendedTestData");
          doc.AppendChild(logDataResultsNode);
          logDataResultsNode = doc.SelectSingleNode("//ExtendedTestData");
        }

        foreach (var result in ExtendedLogData)
        {
          var logDataResultNode = doc.CreateElement("TestDataItem");
          var testNameAttr = doc.CreateAttribute("Name");

          testNameAttr.Value = result.Key;

          if (logDataResultsNode != null)
          {
            logDataResultNode.Attributes.Append(testNameAttr);
            logDataResultNode.InnerText = result.Value;

            logDataResultsNode.AppendChild(logDataResultNode);
          }
        }
      }

      return doc.InnerXml;
    }

    public void AddTestResult(bool? successValue, string testName, string message)
    {
      if (TestResults == null)
      {
        TestResults = new List<TestResultBase>();
      }

      TestResults.Add(new TestResultBase(successValue, testName, message));

    }

    public void AddLogData(string methodName, string data)
    {
      if (ExtendedLogData.ContainsKey(methodName))
      {
        var currentValue = string.Empty;
        if (ExtendedLogData.TryGetValue(methodName, out currentValue))
        {
          string newValue = String.Format("{0} \n {1}", currentValue, data);
          ExtendedLogData.Remove(methodName);
          ExtendedLogData.Add(methodName, newValue);
        }
      }
      else
      {
        ExtendedLogData.Add(methodName, data);
      }
    }

    #endregion

    #region protected

    protected override void OnLoad(EventArgs e)
    {
      if (IsInternal)
      {
        RunTests();
      }
      else
      {
        Response.StatusCode = 403;
        Response.End();
      }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
      var results = GetResultsXml();
      if (string.IsNullOrEmpty(results))
      {
        results = OUTPUT_XML_BASE;
      }

      Response.ContentType = "text/xml";
      Response.ContentEncoding = System.Text.Encoding.UTF8;
      Response.StatusCode = 200;

      writer.Write(results);
      
    }

    #endregion

    #region private

    private void RunTests()
    {
      try
      {
        MethodInfo fixtureSetup;
        MethodInfo fixtureTeardown;
        MethodInfo testSetup;
        MethodInfo testTeardown;

        AddLogData("Test Runner", String.Format("Initialized at {0}", DateTime.Now));

        var tests = GetTests(out fixtureSetup, out fixtureTeardown, out testSetup, out testTeardown);

        if (tests.Count > 0)
        {
          RunTestFixtureSetup(fixtureSetup);

          foreach (var test in tests)
          {

            try
            {
              RunTestSetup(testSetup);

              RunTest(test);

              RunTestTeardown(testTeardown);

            }
            catch (Exception ex)
            {
              AddTestResult(false, test.Name, "The test suite runner encountered an error. " + ex.Message);
            }

          }

          RunTestFixtureTeardown(fixtureTeardown);

        }

      }
      catch (Exception e)
      {
        AddTestResult(false, e.GetType().ToString(), "The test suite runner encountered an error. " + e.Message);
      }

      AddLogData("Test Runner", String.Format("Completed at {0}", DateTime.Now));
    }

    private List<MethodInfo> GetTests(out MethodInfo fixtureSetup, out MethodInfo fixtureTeardown, out MethodInfo testSetup, out MethodInfo testTeardown)
    {
      var tests = new List<MethodInfo>();
      var testType = this.GetType();

      fixtureSetup = null;
      fixtureTeardown = null;
      testSetup = null;
      testTeardown = null;


      foreach (MethodInfo mi in testType.GetMethods())
      {
        var fixtureSetupAttr = (TestFixtureSetupAttribute)Attribute.GetCustomAttribute(mi, typeof(TestFixtureSetupAttribute));
        if (fixtureSetupAttr != null && fixtureSetup == null)
        {
          fixtureSetup = mi;
        }

        var fixtureTeardownAttr = (TestFixtureTeardownAttribute)Attribute.GetCustomAttribute(mi, typeof(TestFixtureTeardownAttribute));
        if (fixtureTeardownAttr != null && fixtureTeardown == null)
        {
          fixtureTeardown = mi;
        }

        var testSetupAttr = (TestSetupAttribute)Attribute.GetCustomAttribute(mi, typeof(TestSetupAttribute));
        if (testSetupAttr != null && testSetup == null)
        {
          testSetup = mi;
        }

        var testTeardownAttr = (TestTeardownAttribute)Attribute.GetCustomAttribute(mi, typeof(TestTeardownAttribute));
        if (testTeardownAttr != null && testTeardown == null)
        {
          testTeardown = mi;

        }

        var testAttr = (TestAttribute)Attribute.GetCustomAttribute(mi, typeof(TestAttribute));
        if (testAttr != null)
        {
          tests.Add(mi);
        }
      }

      return tests;
    }

    private void RunTestFixtureSetup(MethodInfo testFixtureSetup)
    {
      var testType = this.GetType();
      if (testFixtureSetup != null)
      {
        testType.InvokeMember(testFixtureSetup.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, this, new object[] { });
      }
    }

    private void RunTestSetup(MethodInfo testSetup)
    {
      var testType = this.GetType();

      if (testSetup != null)
      {
        testType.InvokeMember(testSetup.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, this,
                                    new object[] { });
      }
    }

    private void RunTest(MemberInfo test)
    {
      var testType = this.GetType();
      var currentTestAttr = (TestAttribute)Attribute.GetCustomAttribute(test, typeof(TestAttribute));
      var expectedException = (TestExpectedException)Attribute.GetCustomAttribute(test, typeof(TestExpectedException));
      var isDestructive = currentTestAttr == null ? false : currentTestAttr.IsDestructive;
      var ignore = currentTestAttr == null ? false : currentTestAttr.Ignore;

      try
      {

        if (ignore)
        {
          AddTestResult(null, test.Name, "IGNORE - This test was flagged to be ignored by the test suite runner.");
        }
        else if (!RunDestructiveTests && isDestructive)
        {
          AddTestResult(null, test.Name, "DESTRUCTIVE - This test was flagged as destructive and the test suite is not set to run destructive tests.");
        }
        else
        {
          testType.InvokeMember(test.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, this, new object[] { });
          AddTestResult(true, test.Name, "Test completed successfully.");
        }

      }
      catch (Exception testException)
      {
        if (expectedException != null && testException.InnerException != null &&
            expectedException.ExceptionType == testException.InnerException.GetType())
        {

          AddTestResult(true, test.Name, testException.InnerException.Message);

        }
        else
        {
          AddTestResult(false, test.Name,
                        testException.InnerException != null
                          ? testException.InnerException.Message
                          : testException.Message);
        }

      }
    }

    private void RunTestTeardown(MethodInfo testTeardown)
    {
      var testType = this.GetType();
      if (testTeardown != null)
      {
        testType.InvokeMember(testTeardown.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, this,
                                    new object[] { });
      }
    }

    private void RunTestFixtureTeardown(MethodInfo testFixtureTeardown)
    {
      var testType = this.GetType();
      if (testFixtureTeardown != null)
      {
        testType.InvokeMember(testFixtureTeardown.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, this, new object[] { });
      }
    }

    #endregion

  }
}