using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Atlantis.Framework.Language.Tests
{
  [TestClass]
  public class CDSLanguagePhraseTests
  {
    [TestMethod, TestCategory("Integration Test"), TestCategory("Triplet")]
    public void Valid_Url_Returns_Data()
    {
      //Arrange
      var requestData = new CDSLanguageRequestData("sales/integrationtests/hosting/web-hosting", "en");

      //Act
      var response = (CDSLanguageResponseData)Engine.Engine.ProcessRequest(requestData, 682);
      var phrase = response.Phrases.FindPhrase("testkey");

      // Assert
      Assert.IsNotNull(response);
      Assert.IsNotNull(response.Phrases);
      Assert.IsNotNull(phrase);
      Assert.AreEqual("Purple River", phrase.PhraseText);
    }

    [TestMethod, TestCategory("Integration Test"), TestCategory("Triplet")]
    public void InValid_Url_Returns_Missing_Document_Response()
    {
      //Arrange
      var requestData = new CDSLanguageRequestData("sales/integrationtests!/hosting/web-hosting", "en");

      //Act
      var response = (CDSLanguageResponseData)Engine.Engine.ProcessRequest(requestData, 682);

      //Assert
      Assert.IsNotNull(response);
      Assert.AreEqual(0, response.Phrases.phraseGroups.Count);
    }

    [TestMethod, TestCategory("Integration Test"), TestCategory("Triplet")]
    [ExpectedException(typeof(AtlantisException))]
    public void Error_Call_To_Service_Throws_Exception()
    {
      //Arrange
      var requestData = new CDSLanguageRequestData("sales/integrationtests!/hosting/web-hosting", "en");
      
      try
      {
        //Act
        var response = (CDSLanguageResponseData)Engine.Engine.ProcessRequest(requestData, 000);
      }
      catch (Exception ex)
      {
        //Assert
        Assert.AreEqual("The remote name could not be resolved: 'cms.devtestingstuff.glbt1.gdg'", ex.Message);
        throw;
      }

    }
  }
}
