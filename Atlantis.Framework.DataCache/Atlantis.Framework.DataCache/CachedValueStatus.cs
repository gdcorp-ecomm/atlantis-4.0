
namespace Atlantis.Framework.DataCache
{
  internal enum CachedValueStatus : int
  {
    NotExpired = 0,
    Expired = 1,
    RefreshInProgress = 2
  }
}
