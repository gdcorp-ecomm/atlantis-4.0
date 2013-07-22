namespace Atlantis.Framework.CDS.Interface
{
  internal class UrlWhitelistResult : IWhitelistResult
  {
    public bool Exists { get; set; }

    public IUrlData UrlData { get; set; }
  }
}
