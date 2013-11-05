using System;
using System.Diagnostics;
using Atlantis.Framework.TargetedShopperService.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TargetedShopperService.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TargetedShopperService.Impl.dll")]
  public class ShopperXidDecodeTests
  {
    [TestMethod]
    public void ShopperXidTest1()
    {
      const string decodedInput = "testemail@godaddy.com";

      ShopperXidEncodeRequestData encodeRequest = new ShopperXidEncodeRequestData(decodedInput);
      ShopperXidEncodeResponseData encodeResponse =
        (ShopperXidEncodeResponseData) Engine.Engine.ProcessRequest(encodeRequest, 718);

      string encodedInput = encodeResponse.ResultData;

      Debug.Write(encodedInput);
      Assert.AreEqual(encodeResponse.ResultStatus, "0");

      ShopperXidDecodeRequestData decodeRequest = new ShopperXidDecodeRequestData(encodedInput);
      ShopperXidDecodeResponseData decodeResponse = (ShopperXidDecodeResponseData)Engine.Engine.ProcessRequest(decodeRequest, 717);

      Assert.AreEqual(decodeResponse.ResultStatus, "0");
      Assert.AreEqual(decodeResponse.ResultData, decodedInput);
    }

    [TestMethod]
    public void ShopperXidTest2()
    {
      const string decodedInput = "213,202,333";

      ShopperXidEncodeRequestData encodeRequest = new ShopperXidEncodeRequestData(decodedInput);
      ShopperXidEncodeResponseData encodeResponse =
        (ShopperXidEncodeResponseData)Engine.Engine.ProcessRequest(encodeRequest, 718);

      string encodedInput = encodeResponse.ResultData;

      Debug.Write(encodedInput);
      Assert.AreEqual(encodeResponse.ResultStatus, "0");

      ShopperXidDecodeRequestData decodeRequest = new ShopperXidDecodeRequestData(encodedInput);
      ShopperXidDecodeResponseData decodeResponse = (ShopperXidDecodeResponseData)Engine.Engine.ProcessRequest(decodeRequest, 717);

      Assert.AreEqual(decodeResponse.ResultStatus, "0");
      Assert.AreEqual(decodeResponse.ResultData, decodedInput);
    }

    [TestMethod]
    public void ShopperXidTest3()
    {
      const string decodedInput = "♥¶Æ¶Æ‰♥";

      ShopperXidEncodeRequestData encodeRequest = new ShopperXidEncodeRequestData(decodedInput);
      ShopperXidEncodeResponseData encodeResponse =
        (ShopperXidEncodeResponseData)Engine.Engine.ProcessRequest(encodeRequest, 718);

      string encodedInput = encodeResponse.ResultData;

      Debug.Write(encodedInput);
      Assert.AreEqual(encodeResponse.ResultStatus, "0");

      ShopperXidDecodeRequestData decodeRequest = new ShopperXidDecodeRequestData(encodedInput);
      ShopperXidDecodeResponseData decodeResponse = (ShopperXidDecodeResponseData)Engine.Engine.ProcessRequest(decodeRequest, 717);

      Assert.AreEqual(decodeResponse.ResultStatus, "0");
      Assert.AreEqual(decodeResponse.ResultData, decodedInput);
    }

    [TestMethod]
    public void DecodeXidTestError()
    {
      // Error Test
      // XID: emgAL1okXQdRSAZIfjdlD3t0XmxySGJgcwlqcQhS

      const string encodedInput = "emgAL1okXQdRSAZIfjdlD3t0XmxySGJgcwlqcQhS";

      ShopperXidDecodeRequestData decodeRequest = new ShopperXidDecodeRequestData(encodedInput);
      ShopperXidDecodeResponseData decodeResponse = (ShopperXidDecodeResponseData)Engine.Engine.ProcessRequest(decodeRequest, 717);

      Assert.AreNotEqual(decodeResponse.ResultStatus, "0");

    }
  }
}
