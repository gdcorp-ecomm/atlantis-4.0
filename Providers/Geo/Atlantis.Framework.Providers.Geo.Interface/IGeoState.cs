namespace Atlantis.Framework.Providers.Geo.Interface
{
  /// <summary>
  /// IGeoState object
  /// </summary>
  public interface IGeoState
  {
    /// <summary>
    /// State id.  Not consistent through the environments.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// State code
    /// </summary>
    string Code { get; }

    /// <summary>
    /// State name.  Will be localized if ILocalizationProvider is available
    /// </summary>
    string Name { get; }
  }
}
