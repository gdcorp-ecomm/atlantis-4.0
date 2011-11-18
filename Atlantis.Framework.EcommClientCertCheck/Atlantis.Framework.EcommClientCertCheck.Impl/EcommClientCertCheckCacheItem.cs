
namespace Atlantis.Framework.EcommClientCertCheck.Impl
{
  internal class EcommClientCertCheckCacheItem
  {
    public bool IsAuthorized { get; set; }

    public long ExpirationTicks { get; set; }
  }
}
