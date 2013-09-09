using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class GetShopperFieldManagerTests
  {
    [TestMethod]
    public void GetNeededFieldsEmpty()
    {
      var fieldManager = new GetShopperFieldManager();
      var neededFields = fieldManager.GetNeededFieldsForShopper("832652");
      Assert.AreEqual(0, neededFields.Count);
    }

    [TestMethod]
    public void GetNeededFieldsEmptyShopperId()
    {
      var fieldManager = new GetShopperFieldManager();
      var neededFields = fieldManager.GetNeededFieldsForShopper(string.Empty);
      Assert.AreEqual(0, neededFields.Count);
    }

    [TestMethod]
    public void GetNeededFieldsNullShopperId()
    {
      var fieldManager = new GetShopperFieldManager();
      var neededFields = fieldManager.GetNeededFieldsForShopper(null);
      Assert.AreEqual(0, neededFields.Count);
    }

    [TestMethod]
    public void RegisterNeededFieldsBasic()
    {
      var fieldManager = new GetShopperFieldManager();
      fieldManager.RegisterNeededFields(new[] { "first_name", "last_name" });
      fieldManager.RegisterNeededField("city");
      fieldManager.RegisterNeededField("first_name");
      Assert.AreEqual(3, fieldManager.GetNeededFieldsForShopper(null).Count);
    }

    private const string _SHOPPER_RESPONSE_XML = "<Shopper ID=\"822497\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields><Field Name=\"last_name\">Bojangles</Field></Shopper>";

    [TestMethod]
    public void RegisterNeededFieldsWithAlreadyReceivedFields()
    {
      var fieldManager = new GetShopperFieldManager();
      fieldManager.RegisterNeededFields(new[] { "first_name", "last_name", "city" });

      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_XML);
      fieldManager.RegisterResponseFields(getShopperResponse);

      var neededFields = fieldManager.GetNeededFieldsForShopper("822497");
      var neededFieldsOtherShopper = fieldManager.GetNeededFieldsForShopper("832652");

      Assert.AreNotEqual(neededFields.Count, neededFieldsOtherShopper.Count);
      Assert.AreEqual(1, neededFields.Count);
      Assert.IsTrue(neededFields.Contains("city"));
    }

    private const string _SHOPPEREMPTY_RESPONSE_XML = "<Shopper ID=\"\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields><Field Name=\"last_name\">Bojangles</Field></Shopper>";

    [TestMethod]
    public void RegisterNeededFieldsWithEmptyShopperReceivedFields()
    {
      var fieldManager = new GetShopperFieldManager();
      fieldManager.RegisterNeededFields(new[] { "first_name", "last_name", "city" });

      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPEREMPTY_RESPONSE_XML);
      fieldManager.RegisterResponseFields(getShopperResponse);

      var neededFields = fieldManager.GetNeededFieldsForShopper("822497");
      Assert.AreEqual(3, neededFields.Count);
    }

    private const string _SHOPPER_RESPONSE_FIRSTNAME = "<Shopper ID=\"822497\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields></Shopper>";
    private const string _SHOPPER_RESPONSE_SECONDNAME = "<Shopper ID=\"822497\"><Fields><Field Name=\"last_name\">Bojangles</Field></Fields></Shopper>";

    [TestMethod]
    public void RegisterNeededFieldsWithEmptyShopperManyReceivedFields()
    {
      var fieldManager = new GetShopperFieldManager();
      fieldManager.RegisterNeededFields(new[] { "first_name", "last_name", "city" });

      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_FIRSTNAME);
      fieldManager.RegisterResponseFields(getShopperResponse);

      getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_SECONDNAME);
      fieldManager.RegisterResponseFields(getShopperResponse);

      var neededFields = fieldManager.GetNeededFieldsForShopper("822497");
      var neededFieldsOtherShopper = fieldManager.GetNeededFieldsForShopper("832652");

      Assert.AreNotEqual(neededFields.Count, neededFieldsOtherShopper.Count);
      Assert.AreEqual(1, neededFields.Count);
      Assert.IsTrue(neededFields.Contains("city"));
    }



  }
}
