using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Language.Interface;
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
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "en", "au", 1);
      Assert.AreEqual("Product", request.DictionaryName);
      Assert.AreEqual("WSBName", request.PhraseKey);
      Assert.AreEqual("en", request.Language);
      Assert.AreEqual("au", request.CountrySite);
      Assert.AreEqual(1, request.ContextId);
    }

    [TestMethod]
    public void RequestDataNullProprerties()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null, null, null, null, 1);
      Assert.AreEqual(string.Empty, request.DictionaryName);
      Assert.AreEqual(string.Empty, request.PhraseKey);
      Assert.AreEqual(string.Empty, request.Language);
      Assert.AreEqual(string.Empty, request.CountrySite);
      Assert.AreEqual(1, request.ContextId);
    }

    [TestMethod]
    public void RequestDataCacheKeyToXml()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      XElement element = XElement.Parse(request.ToXML());
      Assert.IsTrue(element.ToString().Contains("WSBName"));
    }

    [TestMethod]
    public void RequestDataCacheKeySame()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestSame = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "PrOduct", "wsbName", "EN", "au", 1);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentDictionary()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestDifferentDictionary = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Cart", "WSBName", "eN", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentDictionary.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentPhraseKey()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestDifferentKey = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "photoalbum", "eN", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentKey.GetCacheMD5());

    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentLanguage()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestDifferentLanguage = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "es", "Au", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentLanguage.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentCountrySite()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestDifferentCountry = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "uk", 1);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentCountry.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferentContext()
    {
      LangaguePhraseRequestData request = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 1);
      LangaguePhraseRequestData requestDifferentContext = new LangaguePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Product", "WSBName", "eN", "Au", 2);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferentContext.GetCacheMD5());
    }



  }
}
