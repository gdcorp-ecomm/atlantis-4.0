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
      var languageString =
        "<phrase key=\"testkey\" />\r\nGreen River\r\n<phrase key=\"testkey\" contextid=\"6\" />\r\nGreen River\r\nprivate label";

      //Act
      var dictionary = PhraseDictionary.Parse(languageString, "testDictionary", "en-IE");
      var phrase = dictionary.FindPhrase("testkey");
      var phrase2 = dictionary.FindPhrase("testkey", 0, "www", "en-US");
      var fromPhrasePredicate = dictionary.FindPhrase("testkey", new PhrasePredicate(1, "uk", "es-MX"));
      var fromPhrasePredicate2 = dictionary.FindPhrase("testkey", new PhrasePredicate(6, string.Empty, string.Empty));

      //Assert
      Assert.AreEqual(1, dictionary.phraseGroups.Count);
      Assert.AreEqual("Green River", phrase.PhraseText);
      Assert.AreEqual("Green River", phrase2.PhraseText);
      Assert.AreEqual("Green River", fromPhrasePredicate.PhraseText);
      Assert.AreEqual("Green River\r\nprivate label", fromPhrasePredicate2.PhraseText);
    }

    [TestMethod]
    public void PhraseFileInfoTests()
    {
      var pfi = new PhraseFileInfo("test-es.language");

      Assert.AreEqual("test", pfi.DictionaryName);
      Assert.AreEqual("es", pfi.Language);
      Assert.IsTrue(pfi.IsLanguageDataValid);
    }
  }
}