namespace Atlantis.Framework.Providers.Localization.Interface
{
  /// <summary>
  /// Provider interface used to obtain redirect country site and translation language.
  /// </summary>
  public interface ILocalizationRedirectProvider
  {
    /// <summary>
    /// Returns redirect response fields.
    /// </summary>
    ILocalizationRedirectResponse RedirectResponse { get; }
  }
}
