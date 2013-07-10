
namespace Atlantis.Framework.Providers.DomainSearch
{
  public static class DomainGroupTypes
  {
    /// <summary>
    /// Inventory includes Go Daddy Premium and Premium Partners; currently this supports domain spinning and exact matches on domain names.
    /// </summary>
    public const string PREMIUM = "premium";

    /// <summary>
    /// Inventory includes Go Daddy Auctions; currently this supports exact matches on domain names.
    /// </summary>
    public const string AUCTIONS = "auctions";

    /// <summary>
    /// Inventory includes available domains that the service deems similar to the search phrases; currently this supports domain spinning and exact matches on domain names.
    /// </summary>
    public const string SIMILIAR = "similar";

    /// <summary>
    /// Inventory includes our lead generation partners; currently this supports exact matches on domain names.
    /// </summary>
    public const string PRIVATE = "private";

    /// <summary>
    /// Inventory includes entered search phrase combined with Bronson Tlds. If the service determines a TLD is present on the search phrase, 
    /// it will also apply the prefixes and suffixes to the search phrase.
    /// </summary>
    public const string AFFIX = "affix";

    /// <summary>
    /// Inventory includes the entered search phrase combined with ccTlds.
    /// </summary>
    public const string COUNTRY_CODE_TLD = "cctld";

    /// <summary>
    /// Exact match is found on search phrase.
    /// </summary>
    public const string EXACT_MATCH = "exactmatch";
  }
}
