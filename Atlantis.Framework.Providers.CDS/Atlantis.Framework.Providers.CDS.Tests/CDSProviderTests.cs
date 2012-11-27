using System;
using Atlantis.Framework.Providers.Interface.CDS;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Impl;

namespace Atlantis.Framework.Providers.CDS.Tests
{
  [TestClass]
  public class CDSProviderTests
  {
    public CDSProviderTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<Atlantis.Framework.Interface.ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<Atlantis.Framework.Interface.IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICDSProvider, CDSProvider>();
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetJson()
    {
      CDSRequest request = new CDSRequest();
      var cdsProvider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      string cdsJson = cdsProvider.GetJSON("content/es/sales/1/homepage/new/default");
      Assert.IsNotNull(cdsJson);
    }
  }
}
