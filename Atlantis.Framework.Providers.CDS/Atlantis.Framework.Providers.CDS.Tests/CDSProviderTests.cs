using System;
using System.Diagnostics;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.CDS;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.CDS.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.CDS.dll")]
  public class CDSProviderTests
  {
    public CDSProviderTests()
    {
      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());

      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICDSProvider, CDSProvider>();
    }

    [TestMethod]
    public void GetJson()
    {
      var cdsProvider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      string cdsJson = cdsProvider.GetJson("content/en/sales/1/cdstesting/renderpipeline/condition-test", HttpProviderContainer.Instance);
      
      Assert.IsTrue(cdsJson.Contains("Your Country Site is United States or India"));
      Assert.IsTrue(cdsJson.Contains("$1000"));
      
      Debug.WriteLine(cdsJson);
      Console.WriteLine(cdsJson);
    }
  }
}
