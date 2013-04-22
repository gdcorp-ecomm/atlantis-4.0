using Atlantis.Framework.Language.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Tests
{
  [TestClass]
  public class LanguagePhraseTests
  {
    const string _SIMPLEPHRASE = "The quick brown fox jumped over the lazy dog.";
    const string _NULLPHRASE = null;
    const string _TOKENPHRASE = "The quick brown fox jumped over the lazy dog for a price of [@T[productprice:<list productId=\"58\" />]@T].";

    [TestMethod]
    public void ResponseDataValid()
    {
      LanguagePhraseResponseData response = LanguagePhraseResponseData.FromPhrase(_SIMPLEPHRASE);
      Assert.AreEqual(_SIMPLEPHRASE, response.LanguagePhrase);

      Assert.IsNull(response.GetException());
      XElement element = XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataFromNull()
    {
      LanguagePhraseResponseData response = LanguagePhraseResponseData.FromPhrase(_NULLPHRASE);
      Assert.AreEqual(string.Empty, response.LanguagePhrase);

      Assert.IsNull(response.GetException());
      XElement element = XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataEmpty()
    {
      LanguagePhraseResponseData response = LanguagePhraseResponseData.FromPhrase(string.Empty);
      Assert.AreEqual(string.Empty, response.LanguagePhrase);

      Assert.IsNull(response.GetException());
      XElement element = XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataWithTokens()
    {
      LanguagePhraseResponseData response = LanguagePhraseResponseData.FromPhrase(_TOKENPHRASE);
      Assert.AreEqual(_TOKENPHRASE, response.LanguagePhrase);

      Assert.IsNull(response.GetException());
      XElement element = XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void RequestDataValidProprerties()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "en", "au", 1);
      Assert.AreEqual("Product", request.DictionaryName);
      Assert.AreEqual("WSBName", request.PhraseKey);
      Assert.AreEqual("en", request.FullLanguage);
      Assert.AreEqual("au", request.CountrySite);
      Assert.AreEqual(1, request.ContextId);
    }

    [TestMethod]
    public void RequestDataNullProprerties()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null, null, null, null, 1);
      Assert.AreEqual(string.Empty, request.DictionaryName);
      Assert.AreEqual(string.Empty, request.PhraseKey);
      Assert.AreEqual(string.Empty, request.FullLanguage);
      Assert.AreEqual(string.Empty, request.CountrySite);
      Assert.AreEqual(1, request.ContextId);
    }

    [TestMethod]
    public void RequestDataCacheKeyToXml()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      XElement element = XElement.Parse(request.ToXML());
      Assert.IsTrue(element.ToString().Contains("WSBName"));
    }

    [TestMethod]
    public void RequestDataCacheKeySame()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestSame = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "PrOduct", "wsbName", "EN", "au", 1);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentDictionary()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestDifferentDictionary = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Cart", "WSBName", "eN", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentDictionary.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentPhraseKey()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestDifferentKey = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "photoalbum", "eN", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentKey.GetCacheMD5());

    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentLanguage()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestDifferentLanguage = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "es", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentLanguage.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentCountrySite()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestDifferentCountry = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "uk", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentCountry.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentContext()
    {
      LanguagePhraseRequestData request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LanguagePhraseRequestData requestDifferentContext = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 2);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentContext.GetCacheMD5());
    }

    const int _REQUESTTYPE = 681;

    [TestMethod]
    public void RequestDefaultAll()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "en", "www", 1);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("GoDaddy Green River"));
    }

    [TestMethod]
    public void RequestDefaultReseller()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "en", "www", 6);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("Green River"));
    }

    [TestMethod]
    public void RequestDefaultAllButSpanish()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "es", "www", 1);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("Rio Verde"));
    }

    [TestMethod]
    public void RequestDefaultAllButCountry()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "en", "uk", 1);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("Thames River"));
    }

    [TestMethod]
    public void RequestDefaultAllButFullLanguage()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "en-ca", "www", 1);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("GoDaddy Green River"));
    }

    [TestMethod]
    public void RequestUkMexicanSpanishReseller()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "testkey", "es-mx", "uk", 6);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.LanguagePhrase));
      Assert.IsTrue(response.LanguagePhrase.StartsWith("el Thames Reseller River"));
    }

    [TestMethod]
    public void RequestDefaultAllMiss()
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "testdictionary", "missingkey", "en", "uk", 1);
      var response = (LanguagePhraseResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(string.Empty, response.LanguagePhrase);
    }

  }
}
