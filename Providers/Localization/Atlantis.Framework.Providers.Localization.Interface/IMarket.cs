
namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface IMarket
  {
    /// <summary>
    /// Market ID (es-mx, es-es, etc)
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Market Description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Microsoft code base culture string
    /// </summary>
    string MsCulture { get; }

    /// <summary>
    /// Internal use only Market
    /// </summary>
    bool IsInternalOnly { get; }
  }
}
