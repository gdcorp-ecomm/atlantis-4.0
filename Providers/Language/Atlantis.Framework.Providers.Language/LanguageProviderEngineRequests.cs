namespace Atlantis.Framework.Providers.Language
{
  public static class LanguageProviderEngineRequests
  {
    static LanguageProviderEngineRequests()
    {
      FileLanguagePhrase = 681;
      CDSLanguagePhrase = 721;
    }

    public static int FileLanguagePhrase { get; private set; }
    public static int CDSLanguagePhrase { get; private set; }
  }
}
