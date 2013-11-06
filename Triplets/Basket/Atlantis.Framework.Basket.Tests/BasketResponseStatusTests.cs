using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Basket.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  public class BasketResponseStatusTests
  {
    [TestMethod]
    public void BasketResponseStatusNull()
    {
      var status = BasketResponseStatus.FromResponseElement(null);
      Assert.AreEqual(BasketResponseStatusType.Errors, status.Status);
      Assert.IsTrue(ReferenceEquals(BasketResponseStatus.NullResponseError, status));
    }

    [TestMethod]
    public void BasketResponseStatusOneError()
    {
      var responseElement = XElement.Parse(Resources.InvalidShopperResponse);
      var status = BasketResponseStatus.FromResponseElement(responseElement);
      Assert.IsFalse(ReferenceEquals(BasketResponseStatus.NullResponseError, status));
      Assert.AreEqual(1, status.Errors.Count());
      Assert.IsTrue(status.HasErrors);
    }

    [TestMethod]
    public void BasketResponseStatusUnknown()
    {
      var responseElement = XElement.Parse(Resources.UnknownResponse);
      var status = BasketResponseStatus.FromResponseElement(responseElement);
      Assert.IsTrue(ReferenceEquals(BasketResponseStatus.UnknownError, status));
      Assert.IsTrue(status.HasErrors);
    }

    [TestMethod]
    public void BasketResponseStatusSuccess()
    {
      var responseElement = XElement.Parse(Resources.SuccessResponse);
      var status = BasketResponseStatus.FromResponseElement(responseElement);
      Assert.IsTrue(ReferenceEquals(BasketResponseStatus.Success, status));
      Assert.IsFalse(status.HasErrors);
    }

  }
}
