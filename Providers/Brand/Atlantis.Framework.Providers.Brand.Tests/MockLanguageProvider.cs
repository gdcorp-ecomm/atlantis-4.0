using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.Language.Interface;

namespace Atlantis.Framework.Providers.Brand.Tests
{
  public class MockLanguageProvider : ProviderBase, ILanguageProvider
  {

    public MockLanguageProvider(IProviderContainer container)
      : base(container)
    {
    }


    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      return phraseKey + "XXX";
    }
  }
}
