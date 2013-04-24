using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Providers.Language
{
  public class LanguageRenderHandler : IRenderHandler
  {
    const string _DICTIONARYKEY = "dictionary";
    const string _PHRASEKEY = "phrasekey";
    const string _DEFAULTTOKENPATTERN = @"\[@L\[(?<dictionary>[a-zA-z0-9\.\-]*?):(?<phrasekey>[a-zA-z0-9\.\-]*?)\]@L\]";
    private static Regex _languagePhraseTokenPattern;

    static LanguageRenderHandler()
    {
      _languagePhraseTokenPattern = new Regex(_DEFAULTTOKENPATTERN, RegexOptions.Singleline | RegexOptions.Compiled);
    }

    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      ILanguageProvider languageProvider;
      if (providerContainer.TryResolve(out languageProvider))
      {
        var matches = _languagePhraseTokenPattern.Matches(renderContent.Content);
        if (matches.Count > 0)
        {
          StringBuilder contentBuilder = new StringBuilder(renderContent.Content);

          foreach (Match phraseMatch in matches)
          {
            string replacement = string.Empty;

            try
            {
              string dictionary = phraseMatch.Groups[_DICTIONARYKEY].Captures[0].Value;
              string phrasekey = phraseMatch.Groups[_PHRASEKEY].Captures[0].Value;

              replacement = languageProvider.GetLanguagePhrase(dictionary, phrasekey);
            }
            catch (Exception ex)
            {
              AtlantisException exception = new AtlantisException("LanguageRenderHandler.ProcessContent", "0", ex.Message + ex.StackTrace, phraseMatch.Value, null, null);
              Engine.Engine.LogAtlantisException(exception);           
            }

            contentBuilder.Replace(phraseMatch.Value, replacement);
          }

          renderContent.Content = contentBuilder.ToString();
        }
      }
    }
  }
}
