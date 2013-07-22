namespace Atlantis.Framework.CDS.Interface
{
  public interface IWhitelistResult
  {
    bool Exists { get; }

    IUrlData UrlData { get; }
  }
}
