using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Parsers.LanguageFile.Tests
{
  [TestClass]
  public class LanguageFileParserTests
  {
    [TestMethod]
    public void Can_Parse_A_Language_File_From_A_String()
    {
      //Arrange
      var languageString = "<phrase key=\"testkey\" />\r\nGreen River\r\n<phrase key=\"testkey\" contextid=\"6\" />\r\nGreen River private label";

      //Act
      var dictionary = LanguageFile.Parse(languageString, "testDictionary", "en");

      //Assert
      Assert.AreEqual(1, dictionary.phraseGroups.Count);
      Assert.AreEqual("Green River", dictionary.phraseGroups["testkey"]);
    }
  }
}
