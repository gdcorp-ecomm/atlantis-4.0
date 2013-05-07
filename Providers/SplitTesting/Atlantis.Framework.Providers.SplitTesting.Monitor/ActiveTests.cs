using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Monitor
{
  internal class ActiveTests : IMonitor
  {
    public ActiveTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
    }

    public ISplitTestingProvider SplitProvider 
    { 
      get
      {
        return HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();   
      }
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();

      var root = new XElement("data");
      root.Add(GetMachineName(), GetProcessId(), GetInterfaceVersion(), GetFileVersion());
      result.Add(root);

      try
      {
        var items = qsc.AllKeys.SelectMany(qsc.GetValues, (k, v) => new { key = k, value = v });
        string categoryName = string.Empty;
        foreach (var item in items)
        {
          if (!string.IsNullOrEmpty(item.key) && !string.IsNullOrEmpty(item.value))
          {
            switch (item.key.ToLowerInvariant())
            {
              case "category":
                categoryName = item.value;
                break;
            }
          }
        }

        if (!string.IsNullOrEmpty(categoryName))
        {
          SplitTestingConfiguration.DefaultCategoryName = categoryName;

          var tests = SplitProvider.GetAllActiveTests;
          var testInfo = new XElement("activetests");
          testInfo.Add(CategoryName(categoryName));

          var activeSplitTests = tests as IActiveSplitTest[] ?? tests.ToArray();
          testInfo.Add(new XAttribute("count", activeSplitTests.Count()));

          foreach (var activeSplitTest in activeSplitTests)
          {
            var testElement = new XElement("activetest");

            testElement.Add(new XAttribute("runid", activeSplitTest.RunId));
            testElement.Add(new XAttribute("testid", activeSplitTest.TestId));
            testElement.Add(new XAttribute("versionnumber", activeSplitTest.VersionNumber));
            testElement.Add(new XAttribute("eligibilityrules", activeSplitTest.EligibilityRules));
            testElement.Add(new XAttribute("startdate", activeSplitTest.StartDate.ToString("G")));

            testInfo.Add(testElement);
          }
          root.Add(testInfo);
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      return result;
    }

    private static XAttribute CategoryName(string name)
    {
      return new XAttribute("categoryname", name);
    }

    private static XAttribute GetProcessId()
    {
      return new XAttribute("processid", Process.GetCurrentProcess().Id);
    }

    private static XAttribute GetMachineName()
    {
      return new XAttribute("machinename", Environment.MachineName);
    }

    private static XAttribute GetInterfaceVersion()
    {
      var interfaceVersion = string.Empty;

      Type type = typeof(ISplitTestingProvider);
      object[] interfaceFileVersions = type.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
      if (interfaceFileVersions.Length > 0)
      {
        var interfaceFileVersion = interfaceFileVersions[0] as AssemblyFileVersionAttribute;
        if (interfaceFileVersion != null)
        {
          interfaceVersion = interfaceFileVersion.Version;
        }
      }

      return new XAttribute("splitproviderinterfaceversion", interfaceVersion);
    }

    private static XAttribute GetFileVersion()
    {
      var fileVersion = string.Empty;

      object[] fileVersions = typeof(SplitTestingProvider).Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
      if ((fileVersions.Length > 0))
      {
        var assemblyFileVersion = fileVersions[0] as AssemblyFileVersionAttribute;
        if (assemblyFileVersion != null)
        {
          fileVersion = assemblyFileVersion.Version;
        }
      }

      return new XAttribute("splitproviderversion", fileVersion);
    }
  }
}
