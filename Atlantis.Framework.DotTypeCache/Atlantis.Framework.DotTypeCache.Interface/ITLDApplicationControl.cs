namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDApplicationControl
// ReSharper restore InconsistentNaming
  {
    string DotTypeDescription { get; }
    string LandingPageUrl { get; }
    bool IsMultiRegistry { get; }
    bool IsGtld { get; }
  }
}
