using System.Text.RegularExpressions;

namespace Atlantis.Framework.ShopperValidator.Interface.RuleConstants
{
  public class RegexConstants
  {

    //Matches any characters NOT in the regex pattern.  The pattern includes all "normal" characters except XSS characters:  <>;()
    //Abnormal characters being characters like : åäöÅÄÖñÑ
    public static readonly Regex InvalidCharacters = new Regex(@"[^\x20-\x27\x2A-\x3A\x3F-\x7E]", RegexOptions.Compiled);

    public static readonly Regex InvalidXssTags = new Regex(@"[<>;()]", RegexOptions.Compiled);

    public static readonly Regex Email = new Regex(@"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,6}$", RegexOptions.Compiled);
    public static readonly Regex ZipUS = new Regex(@"^(\d{5})(-\d{4})?$", RegexOptions.Compiled);
    public static readonly Regex PhoneUsCanada = new Regex(@"^[2-9]\d{" + (LengthConstants.PhoneUsCanadaMaxLength - 1) + @"}$", RegexOptions.Compiled);
    public static readonly Regex NumericOnly = new Regex(@"^\d+$", RegexOptions.Compiled);    
    public static readonly Regex SpecialCharacters = new Regex( @"[\s\-\+\*\?\(\)\[\]\\\|\$\^\!\.\#]", RegexOptions.Compiled); //Whitespace - + * ? ( ) [ ] | $ ^ ! . #
  }
}
