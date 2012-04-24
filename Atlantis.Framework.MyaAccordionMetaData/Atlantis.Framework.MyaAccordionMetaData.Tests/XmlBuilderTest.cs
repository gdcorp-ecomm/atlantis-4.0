using System.Collections.Generic;
using Atlantis.Framework.MyaAccordionMetaData.Interface.MetaDataBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaAccordionMetaData.Tests
{
  [TestClass()]
  public class XmlBuilderTest
  {
    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_TwoValues()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = "123,456";
      var expected = new HashSet<int>() { 123, 456 };
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_OneValue()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = "123";
      var expected = new HashSet<int>() { 123 };
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_EmptyStringValue()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = string.Empty;
      var expected = new HashSet<int>();
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsNull(actual);
    }

    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_Null()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = null;
      var expected = new HashSet<int>();
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsNull(actual);
    }

    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_NotAnInt()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = "a,23e";
      var expected = new HashSet<int>();
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsNull(actual);
    }

    [TestMethod()]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Interface.dll")]
    public void SetProductTypesTest_ExtraComma()
    {
      XmlBuilder_Accessor target = new XmlBuilder_Accessor();
      string typesStr = "123,456,";
      var expected = new HashSet<int>() { 123, 456 };
      HashSet<int> actual = target.SetProductTypes(typesStr);
      Assert.IsTrue(expected.SetEquals(actual));
    }

  }
}
