using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Parsers.LanguageFile;
using System;

namespace Atlantis.Framework.Providers.Language.Tests
{
  public class MockCDSLanguageRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      CDSLanguageRequestData requestData = (CDSLanguageRequestData)oRequestData;
      CDSLanguageResponseData responseData = null;
      string languageString = string.Empty;
      string versionid = string.Empty;
      switch (requestData.DictionaryName)
      {
        case "sales/atlantistests":
          if (requestData.SpoofParam == "?docid=52697dcdf778fc3e88f8934e")
          {
            languageString = "<phrase key=\"sparky\" />\r\none eyed monster\r\n<phrase key=\"cookkey\" />\r\nthird eye";
            versionid = "52697dcdf778fc3e88f8934e";
          }
          else
          {
            languageString = "<phrase key=\"cookkey\" />\r\nthird eye";
          }
          break;
        case "sales/atlantistests2":
          if (requestData.SpoofParam == "?docid=52697d55f778fc3e88f8934d")
          {
            languageString = "<phrase key=\"shindung\" />\r\nTapSomBong";
            versionid = "52697d55f778fc3e88f8934d";
          }
          break;
        case "sales/integrationtests/hosting/web-hosting":
          languageString = "<phrase key=\"testkey\" />\r\nPurple River";
          break;
      }

      var pd = new PhraseDictionary();
      PhraseDictionary.Parse(pd, languageString, "sales/atlantistests", "en-IE");
      responseData = new CDSLanguageResponseData(pd, versionid, "docid");

      return responseData;
    }
  }

}
