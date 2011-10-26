using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal static class StringExtensions
  {
    public const string WRAPBR = "<br/>";
    public const string WRAPSPACE = " ";

    public static string HtmlWrapWithOutBreakingAnyWords(this string value, int wrapAfter, string wrapString)
    {
      const char SPACE = ' ';
      if (value.Length <= wrapAfter)
      {
        return value;
      }
      else
      {
        int charCount = 0;
        StringBuilder wrapResultBuilder = new StringBuilder(value.Length / wrapAfter + 10);
        do
        {
          int substringLength = 0;
          if ((value.Length - charCount) > wrapAfter)
          {
            if (value[charCount + wrapAfter] != SPACE)
            {
              string line = value.Substring(charCount, wrapAfter);
              int separatorIndex = line.LastIndexOf(SPACE);
              substringLength = (separatorIndex > 0) ? separatorIndex : wrapAfter;
            }
            else
            {
              substringLength = wrapAfter;
            }
          }
          else
          {
            substringLength = (value.Length - charCount);
          }
          wrapResultBuilder.Append(value.Substring(charCount, substringLength));
          charCount += substringLength;
          if (charCount < value.Length - 1)
          {
            wrapResultBuilder.Append(wrapString);
          }
        } while (charCount < value.Length);

        return wrapResultBuilder.ToString();
      }
    }
    public static string HtmlWrapWithOutBreakingAnyWords(this string value, int wrapAfter)
    {
      return HtmlWrapWithOutBreakingAnyWords(value, wrapAfter, WRAPBR);
    }

    public static string HtmlWrap(this string value, int wrapAfter, string wrapString)
    {
      if (value.Length <= wrapAfter)
      {
        return value;
      }
      else
      {
        int lineCount = value.Length / wrapAfter + 1;
        string wrapResult = String.Empty;
        for (int index = 0; index < lineCount; index++)
        {
          int startIndex = (index * wrapAfter);
          int substringLength = (value.Length - startIndex) > wrapAfter ? wrapAfter : (value.Length - startIndex);
          wrapResult += value.Substring(startIndex, substringLength);
          if (index < (lineCount - 1)) wrapResult += wrapString;
        }
        return wrapResult;
      }
    }
    public static string HtmlWrap(this string value, int wrapAfter)
    {
      return HtmlWrap(value, wrapAfter, WRAPBR);
    }

    public static string RemoveSpecialCharacters(this string value)
    {
      return Regex.Replace(value, @"[^A-Za-z0-9#&()-@.,\/\s]", "");
    }

    public static string FormatPhoneNumberByCountry(this string value, string country)
    {
      string result = String.Empty;
      if (value != String.Empty)
      {
        switch (country.ToLowerInvariant())
        {
          case "japan":
            result = Regex.Replace(value, @"[^0-9]", "");
            break;
          default:
            result = value.FormatPhoneNumber();
            break;
        }
      }
      return result;
    }

    public static string FormatPhoneNumber(this string value)
    {
      string result = String.Empty;

      if ((value != null) && (value.Length > 0))
      {
        value = value.Trim();
        if (value.Contains("un"))
        {
          result = value;
        }
        else
        {
          StringBuilder phoneBuilder = new StringBuilder(value);

          StringBuilder part1 = new StringBuilder();
          string part2 = string.Empty;

          int posPlus = -1;
          int posExtp = -1;
          for (int i = 0; i < phoneBuilder.Length; i++)
          {
            char current = phoneBuilder[i];
            switch (current)
            {
              case '+':
                posPlus = i;
                break;
              case 'e':
              case 'E':
              case 'x':
              case 'X':
              case 't':
              case 'T':
              case 'p':
              case 'P':
                posExtp = i;
                break;
              default:
                if (char.IsNumber(current))
                {
                  part1.Append(current);
                }
                break;
            }

          }

          result = value;

          if ((posPlus == -1) || ((posPlus > posExtp) && (posExtp > -1)))
          {
            if (part1.Length == 7)
              result =
                  string.Format(CultureInfo.InvariantCulture, "(602) {0}-{1} {2}", part1.ToString(0, 3), part1.ToString(3, 4), part2);
            else if (part1.Length == 10)
              result =
                  string.Format(CultureInfo.InvariantCulture, "({0}) {1}-{2} {3}", part1.ToString(0, 3), part1.ToString(3, 3), part1.ToString(6, 4), part2);
          }

          if (result.Length > 17)
            result = result.Substring(0, 17);
        }
      }

      return result;

    }
  }
}
