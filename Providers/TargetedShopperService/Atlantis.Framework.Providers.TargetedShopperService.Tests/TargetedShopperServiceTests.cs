using System;
using System.Diagnostics;
using Atlantis.Framework.Providers.TargetedShopperService.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.TargetedShopperService.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TargetedShopperService.Impl.dll")]
  public class TargetedShooperServiceTests
  {

    private readonly MockProviderContainer _container = new MockProviderContainer();

    [TestInitialize]
    public void InitializeProviders()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container.RegisterProvider<ITargetedShopperServiceProvider, TargetedShopperServiceProvider>();

    }

    [TestMethod]
    public void ShopperXidTest1()
    {
      const string decodedInput = "testemail@godaddy.com";

      var tssProvider = _container.Resolve<ITargetedShopperServiceProvider>();
      string encodedInput = tssProvider.ShopperEncodeXid(decodedInput);
      Assert.AreNotEqual(encodedInput, String.Empty);
      Debug.Write(String.Format("Decoded Input : {0}\nEncoded Input: {1}\n", decodedInput, encodedInput));

      string decodedOutput = tssProvider.ShopperDecodeXid(encodedInput);
      Debug.Write(String.Format("Encoded Output : {0}\nDecoded Output : {1}\n", encodedInput, decodedOutput));
      Assert.AreNotEqual(decodedOutput, String.Empty);
      Assert.AreEqual(decodedInput, decodedOutput);
    }

    [TestMethod]
    public void ShopperXidTest2()
    {
      const string decodedInput = "213,202,333";

      var tssProvider = _container.Resolve<ITargetedShopperServiceProvider>();
      string encodedInput = tssProvider.ShopperEncodeXid(decodedInput);
      Assert.AreNotEqual(encodedInput, String.Empty);
      Debug.Write(String.Format("Decoded Input : {0}\nEncoded Input: {1}\n", decodedInput, encodedInput));

      string decodedOutput = tssProvider.ShopperDecodeXid(encodedInput);
      Debug.Write(String.Format("Encoded Output : {0}\nDecoded Output : {1}\n", encodedInput, decodedOutput));
      Assert.AreNotEqual(decodedOutput, String.Empty);
      Assert.AreEqual(decodedInput, decodedOutput);
    }

    [TestMethod]
    public void ShopperXidTest3()
    {
      const string decodedInput = "♥¶Æ¶Æ‰♥";

      var tssProvider = _container.Resolve<ITargetedShopperServiceProvider>();
      string encodedInput = tssProvider.ShopperEncodeXid(decodedInput);
      Assert.AreNotEqual(encodedInput, String.Empty);
      Debug.Write(String.Format("Decoded Input : {0}\nEncoded Input: {1}\n", decodedInput, encodedInput));

      string decodedOutput = tssProvider.ShopperDecodeXid(encodedInput);
      Debug.Write(String.Format("Encoded Output : {0}\nDecoded Output : {1}\n", encodedInput, decodedOutput));
      Assert.AreNotEqual(decodedOutput, String.Empty);
      Assert.AreEqual(decodedInput, decodedOutput);
    }

    [TestMethod]
    public void ShopperXidTest4EmptyString()
    {
      string decodedInput = String.Empty;

      var tssProvider = _container.Resolve<ITargetedShopperServiceProvider>();
      string encodedInput = tssProvider.ShopperEncodeXid(decodedInput);
      Assert.AreEqual(encodedInput, String.Empty);
      Debug.Write(String.Format("Decoded Input : {0}\nEncoded Input: {1}\n", decodedInput, encodedInput));

      string decodedOutput = tssProvider.ShopperDecodeXid(encodedInput);
      Debug.Write(String.Format("Encoded Output : {0}\nDecoded Output : {1}\n", encodedInput, decodedOutput));
      Assert.AreEqual(decodedOutput, String.Empty);
      Assert.AreEqual(decodedInput, decodedOutput);
    }

    [TestMethod]
    public void ShopperXidTest5ErrorDecode()
    {
      const string encodedInput = "emgAL1okXQdRSAZIfjdlD3t0XmxySGJgcwlqcQhS";

      var tssProvider = _container.Resolve<ITargetedShopperServiceProvider>();

      string decodedOutput = tssProvider.ShopperDecodeXid(encodedInput);
      Debug.Write(String.Format("Encoded Output : {0}\nDecoded Output : {1}\n", encodedInput, decodedOutput));
      Assert.AreEqual(decodedOutput, String.Empty);
    }
  }
}
