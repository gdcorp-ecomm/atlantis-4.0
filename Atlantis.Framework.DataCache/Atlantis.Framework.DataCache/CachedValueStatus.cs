
namespace Atlantis.Framework.DataCache
{
  internal enum CachedValueStatus : int
  {
    Valid = 0,
    Invalid = 1,
    RefreshInProgress = 2
  }
}
