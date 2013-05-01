using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.UnitTesting.Attributes;
using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using Atlantis.Framework.Testing.UnitTesting.Exceptions;
using Atlantis.Framework.Testing.UnitTesting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Atlantis.Framework.Testing.UnitTesting
{
  public sealed class TestRunner
  {

    #region Properties

    public bool RunDestructiveTests { get; set; }

    public string[] UnitTestAssemblies { get; set; }

    private TestResultData _testData;
    public TestResultData TestData
    {
      get
      {
        if (_testData == null)
        {
          _testData = new TestResultData();
        }
        return _testData;
      }
    }

    private ISiteContext _siteContext;

    private ISiteContext SiteContext
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

    private UnitTestBase _currentTestClass = null;
    internal UnitTestBase CurrentTestClass
    {
      get { return _currentTestClass; }
    }

    #endregion


    #region public

    private List<UnitTestBase> GetListOfTestClasses(string[] assemblyNames, string unitTestingNamespace, Type reqAttr)
    {
      var listTestInsts = new List<UnitTestBase>();
      foreach (var asmName in assemblyNames)
      {
        var asm = Assembly.Load(asmName);
        var testInsts = asm.GetTypes().Where(
          o =>
          (o.Namespace ?? String.Empty).StartsWith(unitTestingNamespace, StringComparison.Ordinal)
          && o.IsSubclassOf(typeof(UnitTestBase)) // ensure the typecast will work
          && o.GetCustomAttributes(typeof(TestFixtureAttribute), false).Length > 0
          && (reqAttr == null // only if the required attribute is supplied, then search the classes and methods
              || o.GetCustomAttributes(reqAttr, false).Length > 0 // if the required attribute is present on the class, then include the class
              || o.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(
                m => m.GetCustomAttributes(reqAttr, false).Length > 0 // if the required attribute is present on any method in the  class, then include the class
                ))).Select<Type, UnitTestBase>(t => (UnitTestBase)Activator.CreateInstance(t));

        listTestInsts.AddRange(testInsts);
      }
      return listTestInsts;
    }

    public void ExecuteTests(string unitTestingNamespace, string relclassToTest, HashSet<string> testMethods)
    {

      GetTestRunnerExecutionInformation();

      string classToTest = String.Concat(unitTestingNamespace, ".", relclassToTest);
      try
      {

        // find the class in the list of client assemblies
        Type typeOfClassToTest = null;
        foreach (var asm in UnitTestAssemblies)
        {
          typeOfClassToTest = Assembly.Load(asm).GetType(classToTest);
          if (typeOfClassToTest != null)
          {
            break;
          }
        }
        if (typeOfClassToTest == null)
        {
          throw new TestClassNotInAssembliesException(String.Concat("Class '", classToTest, "' not found in these assemblies: ", String.Join(", ", UnitTestAssemblies), "."));
        }

        Type reqAttr = null;

        var testClasses = new List<UnitTestBase>();
        var potentialTestClass = Activator.CreateInstance(typeOfClassToTest);
        var unitTestClass = potentialTestClass as UnitTestBase;
        // see if the class is a unit test
        if (unitTestClass != null)
        {
          testClasses.Add(unitTestClass);
        }
        else
        {
          // see if it is also a collection defined by interface (simpler case)
          var potentialTestCollection = potentialTestClass as IUnitTestCollection;
          if (potentialTestCollection != null)
          {
            reqAttr = potentialTestCollection.CollectionAttribute;
          }
          else
          {
            // see if the TestCollection Attribute is there
            var collectionAttr = (TestCollectionAttribute)Attribute.GetCustomAttribute(typeOfClassToTest, typeof(TestCollectionAttribute), false);
            if (collectionAttr != null)
            {
              // get the required attribute if it is specified
              reqAttr = this.LoadTypeFromAssemblies(collectionAttr.RequiredAttr);
            }
            else
            {
              throw new InvalidTestClassException(String.Concat("Class ", classToTest, " was not derived from UnitTestBase nor was it a test collection"));
            }
          }
          var classesInCollection = GetListOfTestClasses(UnitTestAssemblies, unitTestingNamespace, reqAttr);
          testClasses.AddRange(classesInCollection);
        }

        foreach (var testClass in testClasses)
        {
          _currentTestClass = testClass;

          MethodInfo fixtureSetup;
          MethodInfo fixtureTeardown;
          MethodInfo testSetup;
          MethodInfo testTeardown;

          if (CurrentTestClass != null)
            CurrentTestClass.Log += AddLogData;

          AddLogData(this, new UnitTestLogDataEventArgs("Test Runner", String.Format("Initialized at {0}", DateTime.Now)));

          var tests = GetTests(out fixtureSetup, out fixtureTeardown, out testSetup, out testTeardown, reqAttr);
          if (testMethods.Count > 0)
          {
            tests = tests.FindAll(x => testMethods.Contains(x.Name));
          }

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
      }
      catch (Exception e)
      {
        AddTestResult(false, e.GetType().ToString(), "The test suite runner encountered an error. " + e.Message);
      }

      AddLogData(this, new UnitTestLogDataEventArgs("Test Runner", String.Format("Completed at {0}", DateTime.Now)));

    }

    #endregion


    #region private

    private Type LoadTypeFromAssemblies(string className)
    {
      Type classType = null;
      if (!String.IsNullOrEmpty(className))
      {
        foreach (var asm in UnitTestAssemblies)
        {
          classType = Assembly.Load(asm).GetType(className);
          if (classType != null)
          {
            break;
          }
        }
      }
      return classType;
    }

    private void GetTestRunnerExecutionInformation()
    {
      var currentAssembly = Assembly.GetExecutingAssembly();
      var processid = Process.GetCurrentProcess().Id;
      var serverName = System.Environment.MachineName;
      var unitTestFrameworkAssemblyFileVersion = FileVersionInfo.GetVersionInfo(currentAssembly.Location).FileVersion;

      TestData.ProcessId = processid;
      TestData.ExecutingServerName = serverName;
      TestData.AssemblyFileVersion = unitTestFrameworkAssemblyFileVersion;
    }

    private void AddTestResult(bool? successValue, string testName, string message)
    {
      if (TestData.TestResults == null)
      {
        TestData.TestResults = new List<TestResultBase>();
      }

      TestData.TestResults.Add(new TestResultBase(successValue, testName, message));

      switch (successValue)
      {
        case true:
          TestData.Summary.PassedTests += 1;
          break;
        case false:
          TestData.Summary.FailedTests += 1;
          break;
        default:
          TestData.Summary.IgnoredTests += 1;
          break;
      }

      TestData.Summary.TotalTests += 1;

    }

    private void AddLogData(object sender, UnitTestLogDataEventArgs e)
    {
      if (TestData.ExtendedLogData.ContainsKey(e.MethodName))
      {
        TestData.ExtendedLogData[e.MethodName].Add(e.Data);
      }
      else
      {
        TestData.ExtendedLogData.Add(e.MethodName, new TestExtendedLogDataEntries() { e.Data });
      }

    }

    private List<MethodInfo> GetTests(out MethodInfo fixtureSetup, out MethodInfo fixtureTeardown, out MethodInfo testSetup, out MethodInfo testTeardown, Type reqAttr)
    {
      var tests = new List<MethodInfo>();
      var testType = CurrentTestClass.GetType();

      fixtureSetup = null;
      fixtureTeardown = null;
      testSetup = null;
      testTeardown = null;

      var fixtureAttr = (TestFixtureAttribute)Attribute.GetCustomAttribute(testType, typeof(TestFixtureAttribute));
      if (fixtureAttr == null)
      {
        throw new InvalidTestFixtureException("Expected [TestFixture] attribute to be present on testable class.");
      }
      else
      {
        RunDestructiveTests = fixtureAttr.AllowDestructiveTests;
      }


      bool bAlwaysAddMethod = reqAttr == null || Attribute.GetCustomAttribute(testType, reqAttr, false) != null;
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
          if (bAlwaysAddMethod || Attribute.GetCustomAttribute(mi, reqAttr) != null)
          {
            tests.Add(mi);
          }
        }
      }

      return tests;
    }

    private void RunTestFixtureSetup(MethodInfo testFixtureSetup)
    {
      var testType = CurrentTestClass.GetType();
      if (testFixtureSetup != null)
      {
        testFixtureSetup.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    private void RunTestSetup(MethodInfo testSetup)
    {
      var testType = CurrentTestClass.GetType();

      if (testSetup != null)
      {
        testSetup.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);

      }
    }

    private void RunTest(MemberInfo test)
    {
      var testType = CurrentTestClass.GetType();
      var currentTestAttr = (TestAttribute)Attribute.GetCustomAttribute(test, typeof(TestAttribute));
      var expectedException = (TestExpectedException)Attribute.GetCustomAttribute(test, typeof(TestExpectedException));
      var isDestructive = currentTestAttr == null ? false : currentTestAttr.Options.HasFlag(TestOptions.Destructive);
      var ignore = currentTestAttr == null ? false : currentTestAttr.Ignore;
      var isProductionEnv = SiteContext.ServerLocation == ServerLocationType.Ote ||
                            SiteContext.ServerLocation == ServerLocationType.Prod;

      var canRunInProd = (!isDestructive &&
                         (currentTestAttr == null ? false : currentTestAttr.Options.HasFlag(TestOptions.Production)) &&
                         isProductionEnv);

      try
      {

        if (ignore)
        {
          AddTestResult(null, test.Name, "IGNORE - This test was flagged to be ignored by the test suite runner.");
        }
        else if ((!RunDestructiveTests && isDestructive) || (RunDestructiveTests && !canRunInProd))
        {
          AddTestResult(null, test.Name, "DESTRUCTIVE - This test was flagged as destructive and the test suite is not set to run destructive tests or the enviornment you are trying to execute this test in is not allowed.");
        }
        else
        {
          if (!isProductionEnv || canRunInProd)
          {
            ((MethodInfo)test).Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
            AddTestResult(true, test.Name, "Test completed successfully.");
          }
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
      var testType = CurrentTestClass.GetType();
      if (testTeardown != null)
      {
        testTeardown.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    private void RunTestFixtureTeardown(MethodInfo testFixtureTeardown)
    {
      var testType = CurrentTestClass.GetType();
      if (testFixtureTeardown != null)
      {
        testFixtureTeardown.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    #endregion


  }

}
