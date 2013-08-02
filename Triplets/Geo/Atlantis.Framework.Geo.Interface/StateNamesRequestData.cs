using System.Globalization;

namespace Atlantis.Framework.Geo.Interface
{
  public class StateNamesRequestData : LanguageNamesRequestData
  {
    public int CountryId { get; private set; }

    public StateNamesRequestData(string fullLanguage, int countryId)
      : base(fullLanguage)
    {
      CountryId = countryId;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(FullLanguage, ":", CountryId.ToString(CultureInfo.InvariantCulture));
    }
  }
}
