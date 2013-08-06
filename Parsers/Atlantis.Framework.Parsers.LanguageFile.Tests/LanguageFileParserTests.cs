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
        "<phrase key=\"testkey\" />\r\nGreen River\r\n<phrase key=\"testkey\" contextid=\"6\" />\r\nGreen River\r\nprivate label\r\n<phrase key=\"testkey\" contextid=\"6\">";
      var pd = new PhraseDictionary();

      //Act
      PhraseDictionary.Parse(pd,languageString, "testDictionary", "en-IE");
      var phrase = pd.FindPhrase("testkey");
      var phrase2 = pd.FindPhrase("testkey", 0, "www", "en-US");
      var fromPhrasePredicate = pd.FindPhrase("testkey", new PhrasePredicate(1, "uk", "es-MX"));
      var fromPhrasePredicate2 = pd.FindPhrase("testkey", new PhrasePredicate(6, string.Empty, string.Empty));

      //Assert
      Assert.AreEqual(2, pd.phraseGroups.Count);
      Assert.AreEqual("Green River", phrase.PhraseText);
      Assert.AreEqual("Green River", phrase2.PhraseText);
      Assert.AreEqual("Green River", fromPhrasePredicate.PhraseText);
      Assert.AreEqual("Green River\r\nprivate label", fromPhrasePredicate2.PhraseText);
    }

    [TestMethod]
    public void AutoHandleLanguageFalseWillNotGenerateShortLanguageDefaults()
    {
      //Arrange
      var languageString =
        "<phrase key=\"testkey\" />\r\nGreen River\r\n<phrase key=\"testkey\" contextid=\"6\" />\r\nGreen River\r\nprivate label\r\n<phrase key=\"testkey\" contextid=\"6\">";
      var pd = new PhraseDictionary(false);

      //Act
      PhraseDictionary.Parse(pd, languageString, "testDictionary", "en-IE");
      var phrase = pd.FindPhrase("testkey");
      var phrase2 = pd.FindPhrase("testkey", 0, "www", "en-US");
      var fromPhrasePredicate = pd.FindPhrase("testkey", new PhrasePredicate(1, "uk", "es-MX"));
      var fromPhrasePredicate2 = pd.FindPhrase("testkey", new PhrasePredicate(6, string.Empty, string.Empty));
      var fromPhrasePredicate3 = pd.FindPhrase("testkey", new PhrasePredicate(1, "www", "en-IE"));
      var fromPhrasePredicate4 = pd.FindPhrase("testkey", new PhrasePredicate(6, "www", "en-IE"));

      //Assert
      Assert.AreEqual(2, pd.phraseGroups.Count);
      Assert.AreEqual(null, phrase);
      Assert.AreEqual(null, phrase2);
      Assert.AreEqual(null, fromPhrasePredicate);
      Assert.AreEqual(null, fromPhrasePredicate2);
      Assert.AreEqual("Green River", fromPhrasePredicate3.PhraseText);
      Assert.AreEqual("Green River\r\nprivate label", fromPhrasePredicate4.PhraseText);
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