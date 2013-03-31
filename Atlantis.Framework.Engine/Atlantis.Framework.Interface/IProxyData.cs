namespace Atlantis.Framework.Interface
{
  /// <summary>
  /// Contains information about proxy requests
  /// </summary>
  public interface IProxyData
  {
    /// <summary>
    /// Original IP address of the proxied request
    /// </summary>
    string OriginalIP { get; }

    /// <summary>
    /// Original Host address of the proxied request
    /// </summary>
    string OriginalHost { get; }

    /// <summary>
    /// true if the host can be used for context
    /// </summary>
    bool IsContextualHost { get; }

    /// <summary>
    /// Type of proxy <see cref="ProxyTypes"/>
    /// </summary>
    ProxyTypes ProxyType { get; }

    /// <summary>
    /// Attempt to get any extended data (like language) from the proxy
    /// </summary>
    /// <param name="key">name of value to get</param>
    /// <param name="value">output value</param>
    /// <returns>true if a value is found for the given key</returns>
    bool TryGetExtendedData(string key, out string value);
  }
}
