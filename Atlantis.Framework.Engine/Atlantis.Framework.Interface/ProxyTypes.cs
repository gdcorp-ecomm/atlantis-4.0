namespace Atlantis.Framework.Interface
{
  /// <summary>
  /// Types of supported proxies
  /// </summary>
  public enum ProxyTypes
  {
    /// <summary>
    /// Default type of none
    /// </summary>
    None = 0,
    /// <summary>
    /// A 'same box' proxy using Microsoft ARR.  Example: www.wildwestdomains.com proxying an origin site on the same box
    /// </summary>
    LocalARR = 1,
    /// <summary>
    /// An ARR proxy (same box or different) that is used for our country sites like ca.godaddy.com
    /// </summary>
    CountrySiteARR = 2,
    /// <summary>
    /// Custom reseller proxy that is proxying custom domain for a reseller like www.maddogdomains.com
    /// </summary>
    CustomResellerARR = 3,
    /// <summary>
    /// Transperfect language proxy (es.godaddy.com)
    /// </summary>
    TransPerfectTranslation = 4,
    /// <summary>
    /// Akamai Dynamic site acceleration
    /// </summary>
    AkamaiDSA = 5,
    /// <summary>
    /// Smartling language proxy
    /// </summary>
    SmartlingTranslation = 6
  }
}
