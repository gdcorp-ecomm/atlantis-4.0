using System.Collections.Generic;
namespace Atlantis.Framework.CDS.Interface
{
  internal class UrlWhitelistResult : IWhitelistResult
  {
    public bool Exists { get; set; }

    public Dictionary<string, string> UrlData { get; set; }
  }
}
