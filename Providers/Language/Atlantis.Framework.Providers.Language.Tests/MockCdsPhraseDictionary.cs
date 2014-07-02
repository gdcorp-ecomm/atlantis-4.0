using System;
using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Providers.Language.Tests
{
  static internal class MockCdsPhraseDictionary
  {
    static internal PhraseDictionary GetPhraseDictionary(string marketId)
    {
      PhraseDictionary pd = new PhraseDictionary();

      switch (marketId.ToLower())
      {
        case "fr-fr":
          PhraseDictionary.Parse(pd, "<phrase key=\"desk\" />\r\nUn bureau fabriqué en France", "mockCdsDictionary", "fr-FR");
          break;

        case "fr":
          PhraseDictionary.Parse(pd, "<phrase key=\"desk\" />\r\nUn beau bureau en noyer\r\n<phrase key=\"chair\" />\r\nUne chaise assez bleu", "mockCdsDictionary", "fr");
          break;

        case "en":
          PhraseDictionary.Parse(pd, "<phrase key=\"desk\" />\r\nA desk\r\n<phrase key=\"chair\" />\r\nA chair\r\n<phrase key=\"employee\" />\r\nA Go Daddy employee", "mockCdsDictionary", "en");
          break;

        default:
          throw new NotSupportedException(string.Format("Market ID not supported [{0}]", marketId));
      }

      return pd;
    }
  }
}
