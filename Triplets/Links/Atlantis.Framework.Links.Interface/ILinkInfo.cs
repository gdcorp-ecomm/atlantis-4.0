namespace Atlantis.Framework.Links.Interface
{
  public interface ILinkInfo
  {
    string BaseUrl { get; }
    LinkTypeLanguageSupport LanguageSupportType { get; }
    string LanguageParameter { get; }
    LinkTypeCountrySupport CountrySupportType { get; }
    string CountryParameter { get; }
  }
}
