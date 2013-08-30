namespace Atlantis.Framework.Links.Interface
{
  public class LinkInfoImpl : ILinkInfo
  {
    public string BaseUrl { get; set; }
    public LinkTypeLanguageSupport LanguageSupportType { get; set; }
    public string LanguageParameter { get; set; }
    public LinkTypeCountrySupport CountrySupportType { get; set; }
    public string CountryParameter { get; set; }
  }
}
