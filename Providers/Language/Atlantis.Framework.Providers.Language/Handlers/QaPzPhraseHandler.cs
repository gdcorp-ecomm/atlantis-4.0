using Atlantis.Framework.Interface;
using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class QaPzPhraseHandler : ILanguagePhraseHandler
  {
    private readonly Lazy<CDSPhraseHandler> _cdsPhraseHandler;
    private readonly Lazy<FilePhraseHandler> _filePhraseHandler;
    private readonly Lazy<ISiteContext> _siteContext;

    public QaPzPhraseHandler(IProviderContainer container, string fullLanguage, string shortLanguage)
    {
      _cdsPhraseHandler = new Lazy<CDSPhraseHandler>(() => new CDSPhraseHandler(container, fullLanguage, shortLanguage));
      _filePhraseHandler = new Lazy<FilePhraseHandler>(() => new FilePhraseHandler(container, fullLanguage));
      _siteContext = new Lazy<ISiteContext>(() => container.Resolve<ISiteContext>());
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      string tempPhrase;
      if (GetCDSOrFilePhrase(dictionaryName, phraseKey, out tempPhrase))
      {
        phrase = PadStrings(tempPhrase);
        return true;
      }
      phrase = tempPhrase;
      return false;
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, bool doGlobalFallback, out string phrase)
    {
      throw new NotSupportedException("The QaPz Phrase Handler does not support this overload.");
    }

    private bool GetCDSOrFilePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      ILanguagePhraseHandler baseHandler = null;
      if (dictionaryName.StartsWith("cds.", StringComparison.OrdinalIgnoreCase))
      {
        baseHandler = _cdsPhraseHandler.Value;
      }
      else
      {
        baseHandler = _filePhraseHandler.Value;
      }
      return baseHandler.TryGetLanguagePhrase(dictionaryName, phraseKey, out phrase);
    }


    private string PadStrings(string tempPhrase)
    {
      if (String.IsNullOrEmpty(tempPhrase))
        return String.Empty;

      int lineLen = tempPhrase.Length;
      if (lineLen < 2)
        return tempPhrase;

      char x = tempPhrase[0];
      char y = tempPhrase[2];

      if (x == 239 && y == 191)
        tempPhrase = tempPhrase.Substring(3);

      string paragraph = ProtectCharacters(tempPhrase, "preprocess");
      paragraph = ProtectEntities(paragraph, "preprocess");

      paragraph = ProcessPhrase(paragraph);

      paragraph = ProtectCharacters(paragraph, "postprocess");
      paragraph = ProtectEntities(paragraph, "postprocess");
      return paragraph;
    }

    private string ProtectEntities(string paragraph, string direction)
    {
      if (direction == "preprocess")
      {

        // protect all placeholders ex: {{companyname}}, {{companyaddress}}, etc.
        // this is done by searching for {plceholder} instead of {{placeholder}}

        //protect all html entities ex: '&dagger;', '&nbsp;', '&deg;', etc.
        paragraph = Regex.Replace(paragraph, "&(.*?);", AddProtection);
        
        // protect all tokens ex: [@T[companyname:name]@T]
        paragraph = Regex.Replace(paragraph, "\\[@T(.*?)\\@T\\]", AddProtection);
        
        // protect all \u notated characters ex: \u00A9, \u2193, etc.
        paragraph = Regex.Replace(paragraph, @"[^\u0000-\u007F]", AddProtection);

        // protect all [#NUM#], [#showonly=|1|#] placeholders in html docs
        paragraph = Regex.Replace(paragraph, "\\[#(.*?)#\\]", AddProtection);

      }
      else
      {
        paragraph = RemoveProtections(paragraph);
      }
      return paragraph;
    }

    private static string AddProtection(Match match)
    {
      string phraseToProtect = match.ToString();
      phraseToProtect = HttpUtility.HtmlEncode(phraseToProtect);
      return "<protect entity='" + phraseToProtect + "'></protect>";
    }

    private static string RemoveProtections(string paragraph)
    {
      string unprotectParagraph = paragraph.Replace("<protect entity='", String.Empty).Replace("'></protect>", String.Empty);
      return HttpUtility.HtmlDecode(unprotectParagraph);
    }

    private string[] process_chars =
    {
      "•", "∞", "£", "€", "\\u0169", "�", "í", "­", "Ã", "á", "ã", "ô", "Î", "é", "ü", "ï",
      "¿", "½", "\\u0174", "‡", "¢", "°", "‘", "»", " ", "”", "“", "²", "®", "™", "’", "…", "—", "†", "–"
    };

    private string ProtectCharacters(string paragraph, string direction) //not sure what we are trying to do here
    {

      int cc = 1000;
      string replace_str;

      foreach (string process_char in process_chars)
      {
        replace_str = "!!_" + cc.ToString();
        if (direction == "preprocess")
        {
          paragraph = paragraph.Replace(process_char, replace_str);
        }
        else
        {
          paragraph = paragraph.Replace(replace_str, process_char);
        }

        cc = cc + 1;
      }

      return paragraph;
    }

    private string ProcessPhrase(string paragraph)
    {
      string paddedParagraph = String.Empty;
      if (String.IsNullOrWhiteSpace(paragraph))
      {
        return paragraph;
      }

      if (paragraph.Contains("<"))
      {
        paddedParagraph = paragraph;
        string xdocParagraph = String.Format("<root>{0}</root>", paragraph);
        XDocument xdoc = XDocument.Parse(xdocParagraph);

        foreach (XNode docNode in xdoc.DescendantNodes())
        {
          if (docNode.NodeType == System.Xml.XmlNodeType.Text)
          {
            string replacementText = CreatePseudoText(docNode.ToString());
            paddedParagraph = paddedParagraph.Replace(docNode.ToString(), replacementText);
          }
        }
      }
      else
      {
        paddedParagraph += CreatePseudoText(paragraph);
      }

      return paddedParagraph;
    }

    private string CreatePseudoText(string currentText)
    {
      string pseudoText = currentText;
      string paddedParagraph = String.Empty;

      foreach (char character in currentText)
      {
        if (char.IsLetter(character))
        {
          if (char.IsLower(character))
          {
            pseudoText = pseudoText.Replace(character, 'z');
          }
          else
          {
            pseudoText = pseudoText.Replace(character, 'Z');
          }
        }
      }

      int pseudoLength = pseudoText.Length;
      int extensionLength = 0;
      if (pseudoLength > 10)
      {
        extensionLength = pseudoLength / 3;
      }
      else
      {
        extensionLength = pseudoLength * 4;
      }

      string extendedString = GenerateExtension(extensionLength);

      if (pseudoLength > 0)
      {
        string[] pseudoLines = pseudoText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        if (pseudoLines.Length > 1)
        {
          foreach (string line in pseudoLines)
          {
            paddedParagraph += pseudoText + extendedString + Environment.NewLine;
          }
        }
        else
        {
          paddedParagraph += pseudoText + extendedString;
        }
      }
      return paddedParagraph;
    }

    private string GenerateExtension(int extensionLength)
    {

      string newString = String.Empty;
      string baseString = " zzz";
      newString = baseString;
      while (newString.Length < extensionLength)
      {
        newString += baseString;
      }
      return newString;
    }

  }

}
