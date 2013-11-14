using System.Collections.Generic;
namespace Atlantis.Framework.CDS.Interface
{
  public interface IWhitelistResult
  {
    bool Exists { get; }

    Dictionary<string, string> UrlData { get; }
  }
}
