using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class PatternFilter
  {
    [DataMember]
    public PatternCharacterType CharacterOne { get; set; }

    [DataMember]
    public PatternCharacterType CharacterTwo { get; set; }

    [DataMember]
    public PatternCharacterType CharacterThree  { get; set; }

    [DataMember]
    public PatternCharacterType CharacterFour { get; set; }

    private static string GetPatternCharacterTypeString(PatternCharacterType patternCharacterType)
    {
      string character;

      switch (patternCharacterType)
      {
        case PatternCharacterType.Consonant:
          character = "c";
          break;
        case PatternCharacterType.Vowel:
          character = "v";
          break;
        case PatternCharacterType.Number:
          character = "n";
          break;
        default:
          character = string.Empty;
          break;
      }

      return character;
    }

    public override string ToString()
    {
      return string.Format("{0}{1}{2}{3}", GetPatternCharacterTypeString(CharacterOne),
                                           GetPatternCharacterTypeString(CharacterTwo), 
                                           GetPatternCharacterTypeString(CharacterThree),
                                           GetPatternCharacterTypeString(CharacterFour));

    }
  }
}
