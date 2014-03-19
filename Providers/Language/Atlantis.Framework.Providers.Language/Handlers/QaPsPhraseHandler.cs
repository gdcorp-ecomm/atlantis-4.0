using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  public class QaPsPhraseHandler : ILanguagePhraseHandler
  {
    private readonly Lazy<CDSPhraseHandler> _cdsPhraseHandler;
    private readonly Lazy<FilePhraseHandler> _filePhraseHandler;
    private readonly Lazy<ISiteContext> _siteContext;


    public QaPsPhraseHandler(IProviderContainer container)
    {
      _cdsPhraseHandler = new Lazy<CDSPhraseHandler>(() => new CDSPhraseHandler(container));
      _filePhraseHandler = new Lazy<FilePhraseHandler>(() => new FilePhraseHandler(container));
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

      string paragraph = protect_chars(tempPhrase, "preprocess");
      paragraph = protect_entities(paragraph, "preprocess");

      string pseudo = "qa-ps";
      String result = process_paragraph_elements(paragraph, pseudo);
      
      paragraph = protect_chars(paragraph, "postprocess");
      paragraph = protect_entities(paragraph, "postprocess");
      return paragraph;
    }

    private static string AddBracketPoundSigns(Match match)
    {
      string v = match.ToString();
      return "[#" + match.ToString() + "#]";
    }

  
    private string protect_entities(string paragraph, string direction)
    {
      if (direction == "preprocess")
      {
        // protect all placeholders ex: {{companyname}}, {{companyaddress}}, etc.
        // this is done by searching for {plceholder} instead of {{placeholder}}

        //protect all html entities ex: '&dagger;', '&nbsp;', '&deg;', etc.
        paragraph = Regex.Replace(paragraph, "&[^\\s]*;", AddBracketPoundSigns);

        // protect all tokens ex: [@T[companyname:name]@T]
        paragraph = Regex.Replace(paragraph, "\\[@T[^\\s]*]@T\\]", AddBracketPoundSigns);

        // protect all \u notated characters ex: \u00A9, \u2193, etc.
        paragraph = Regex.Replace(paragraph, "\\u[^\\s]*", AddBracketPoundSigns);

        // protect all [#NUM#], [#showonly=|1|#] placeholders in html docs
        paragraph = Regex.Replace(paragraph, "\\[#(.*?)#\\]", AddBracketPoundSigns);
      }
      else
      {
        //remove the [#  #] TODO
      }

      return paragraph;
    }


    private char[] process_chars =
    {
      '•', '∞', '£', '€', '\u0169', '�', 'í', '­', 'Ã', 'á', 'ã', 'ô', 'Î', 'é', 'ü', 'ï',
      '¿', '½', '\u0174', '‡', '¢', '°', '‘', '»', ' ', '”', '“', //'世界', -- what is this character?
      '²', '®', '™', '’', '…', '—', '†', '–'
    };


    private string protect_chars(string paragraph , string direction )
    {

      int cc = 1000;
      string replace_str;
      foreach (char process_char in process_chars)
      {
        replace_str = "!!_" + cc.ToString();
        if (direction == "preprocess")
        {
          paragraph = paragraph.Replace(process_char.ToString(), replace_str);
        }
        else
        {
          paragraph = paragraph.Replace(replace_str, process_char.ToString());
        }

        cc = cc + 1;
      }
    
      return paragraph;
    }

    private string gen_extension(int extension_len, string pseudo)
    {

      string newStr = String.Empty;
      //if pseudo == 'qa-ps':
      //baseStr = ' !!!'
      //elif
      //pseudo == 'qa-pz':
      //baseStr = ' zzz'

      //newStr = baseStr
      //while
      //len(newStr) < extension_len:
      //newStr = newStr + baseStr

      return newStr;
    }


    private string process_paragraph_elements(string paragraph, string pseudo)
    {
      //result = []
      //wrdcount = 0

      //#check for an empty paragraph - there might still be a newline, so return the paragraph if "empty"
      //if paragraph.strip() == '':
      //  tango = {'paragraph': paragraph, 'wrdcount': 0}
      //  result.append(tango)
      //  return result

      //tree = etree.parse(StringIO.StringIO(paragraph), etree.HTMLParser())
      //for atag in tree.xpath('//html'):
      //  elements = atag.xpath('descendant-or-self::node()')
      //  i = 0
      //  for element in elements:
      //    i = i + 1
      //    try:
      //       elementStr = str(element)
      //    except UnicodeEncodeError:
      //      print 'UnicodeEncodeError: ', element

      //    if not (elementStr[:8] == '<Element' or elementStr.find('FONT-FAMILY') > 0):
      //      wcount = 0
      //      for j in elementStr.split():
      //        eawrdlen = len(j) / len(j)
      //        wcount = wcount + eawrdlen

      //      wrdcount = wrdcount + wcount


      //      if pseudo == 'qa-ps':
      //        pseudo_element = elementStr.replace('a', 'å').replace('e', 'é').replace('i', 'î').replace('o', 'ò').replace('u', 'ü').replace('A', 'Å').replace('E', 'É').replace('I', 'Î').replace('O', 'Ò').replace('U', 'Ü')
      //      elif pseudo == 'qa-pz':
      //        pseudo_element = elementStr
      //        for number in xrange(ord('a'), ord('z')+1):
      //          pseudo_element = pseudo_element.replace(chr(number), 'z')

      //        for number in xrange(ord('A'), ord('Z')+1):
      //          pseudo_element = pseudo_element.replace(chr(number), 'Z')

      //      pseudo_len = len(pseudo_element)

      //      # need to check for CR, LF, or CRLF at the end of an element - we cannot tag on the pseudo characters after the CR
      //      pseudo_len = len(pseudo_element)
      //      if pseudo_len > 10:
      //        extension_len = pseudo_len / 3
      //      else:
      //        extension_len = pseudo_len * 4

      //      extension_str = gen_extension(extension_len, pseudo)

      //      if pseudo == 'qa-ps':
      //        opening_char = "["
      //        closing_char = "]"
      //      elif pseudo == 'qa-pz':
      //        opening_char = ""
      //        closing_char = ""
      //      else:
      //        opening_char = ""
      //        closing_char = ""

      //      if pseudo_len > 0:
      //        if pseudo_element[pseudo_len - 1] == "\n":
      //          if pseudo_len > 1 and pseudo_element[pseudo_len - 2] == "\r":
      //            pseudo_element = opening_char + pseudo_element[:-2] + extension_str + closing_char + "\r" + "\n"
      //          else:
      //            pseudo_element = opening_char + pseudo_element[:-1] + extension_str + closing_char + "\n"
      //        else:
      //          pseudo_element = opening_char + pseudo_element + extension_str + closing_char
      //      else:
      //        pseudo_element = opening_char + pseudo_element + extension_str + closing_char

      //      paragraph = paragraph.replace(elementStr, pseudo_element, 1)

      //tango = {'paragraph': paragraph, 'wrdcount': wrdcount}
      //result.append(tango)
      //return result
      return paragraph;
    }
  }
	
}
