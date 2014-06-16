using System;
using System.Xml.Linq;
using Atlantis.Framework.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  public class BasketDeleteRequestDataTests
  {
    [TestMethod]
    public void WillConstructorWithParametersSetShopperId()
    {
      const string shopperId = "1234";
      var requestData = new BasketDeleteRequestData(shopperId, null);
      Assert.IsTrue(string.Equals(shopperId, requestData.ShopperID));
    }

    [TestMethod]
    public void WillConstructorWithParametersSetItemsToDelete()
    {
      var item1 = new BasketDeleteItemKey(1, 2);
      var item2 = new BasketDeleteItemKey(3, 4);
      var requestData = new BasketDeleteRequestData(string.Empty, new[] {item1, item2});
      var actualItemKeysToDelete = requestData.GetItemKeysToDelete();
      var expectedItemKeysToDelete = item1 + "|" + item2;

      Assert.IsTrue(string.Equals(expectedItemKeysToDelete, actualItemKeysToDelete));
    }

    [TestMethod]
    public void WillGetItemKeysToDeleteReturnEmptyStringIfItemsToDeleteIsEmpty()
    {
      var requestData = new BasketDeleteRequestData(string.Empty, null);
      Assert.IsTrue(string.Equals(string.Empty, requestData.GetItemKeysToDelete()));
    }

    [TestMethod]
    public void WillAddItem()
    {
      const int rowId = 1;
      const int itemId = 2;
      var requestData = new BasketDeleteRequestData(string.Empty, null);
      requestData.AddItem(rowId, itemId);
      
      var actualItemKeysToDelete = requestData.GetItemKeysToDelete();
      var expectedItemKeysToDelete = rowId + "," + itemId;

      Assert.IsTrue(string.Equals(expectedItemKeysToDelete, actualItemKeysToDelete));
    }

    [TestMethod]
    public void WillAddItems()
    {
      var item1 = new BasketDeleteItemKey(1, 2);
      var item2 = new BasketDeleteItemKey(3, 4);
      var requestData = new BasketDeleteRequestData(string.Empty, null);
      requestData.AddItems(new[]{item1, item2});

      var actualItemKeysToDelete = requestData.GetItemKeysToDelete();
      var expectedItemKeysToDelete = item1 + "|" + item2;

      Assert.IsTrue(string.Equals(expectedItemKeysToDelete, actualItemKeysToDelete));
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void WillGetCacheMD5ThrowExcpetion()
    {
      var requestData = new BasketDeleteRequestData(string.Empty, null);
      requestData.GetCacheMD5();
    }

    [TestMethod]
    public void WillToXMLReturnCorrectValueIfShopperIdIsNull()
    {
      var item1 = new BasketDeleteItemKey(1, 2);
      var item2 = new BasketDeleteItemKey(3, 4);
      var requestData = new BasketDeleteRequestData(string.Empty, new[] { item1, item2 });

      var xelement = new XElement(requestData.GetType().FullName);
      xelement.Add(new XAttribute("BasketType", requestData.BasketType));
      xelement.Add(new XAttribute("IsManager", requestData.IsManager));
      xelement.Add(new XAttribute("ItemsToDelete", requestData.GetItemKeysToDelete()));

      var expectedXmlString = xelement.ToString(SaveOptions.DisableFormatting);
      var actualXmlString = requestData.ToXML();

      Assert.IsTrue(string.Equals(expectedXmlString, actualXmlString));
    }

    [TestMethod]
    public void WillToXMLReturnCorrectValueIfShopperIdIsNotNull()
    {
      var item1 = new BasketDeleteItemKey(1, 2);
      var item2 = new BasketDeleteItemKey(3, 4);
      var requestData = new BasketDeleteRequestData("1234", new[] { item1, item2 });

      var xelement = new XElement(requestData.GetType().FullName);
      xelement.Add(new XAttribute("ShopperId", requestData.ShopperID));
      xelement.Add(new XAttribute("BasketType", requestData.BasketType));
      xelement.Add(new XAttribute("IsManager", requestData.IsManager));
      xelement.Add(new XAttribute("ItemsToDelete", requestData.GetItemKeysToDelete()));

      var expectedXmlString = xelement.ToString(SaveOptions.DisableFormatting);
      var actualXmlString = requestData.ToXML();

      Assert.IsTrue(string.Equals(expectedXmlString, actualXmlString));
    }
  }
}
