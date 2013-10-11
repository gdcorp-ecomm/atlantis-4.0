using Atlantis.Framework.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  public class BasketErrorTests
  {
    [TestMethod]
    public void BasketErrorNull()
    {
      var error = BasketError.FromErrorXml(null);
      Assert.AreEqual(string.Empty, error.Description);
      Assert.AreEqual(string.Empty, error.Level);
      Assert.AreEqual(string.Empty, error.Number);
      Assert.AreEqual(string.Empty, error.Source);
    }

    [TestMethod]
    public void BasketErrorEmptyElement()
    {
      var element = new XElement("ERROR");
      var error = BasketError.FromErrorXml(element);
      Assert.AreEqual(string.Empty, error.Description);
      Assert.AreEqual(string.Empty, error.Level);
      Assert.AreEqual(string.Empty, error.Number);
      Assert.AreEqual(string.Empty, error.Source);
    }

    [TestMethod]
    public void BasketErrorWithData()
    {
      var element = XElement.Parse("<ERROR><NUMBER>911</NUMBER><DESC>Test Error.</DESC><SOURCE>BasketErrorTests</SOURCE><LEVEL>ERROR</LEVEL></ERROR>");
      var error = BasketError.FromErrorXml(element);
      Assert.AreEqual("Test Error.", error.Description);
      Assert.AreEqual("ERROR", error.Level);
      Assert.AreEqual("911", error.Number);
      Assert.AreEqual("BasketErrorTests", error.Source);
    }

  }
}
