using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  internal class UrlWhitelistResult : IWhitelistResult
  {
    #region IWhitelistResult Members

    public bool Exists { get; set; }

    public IUrlData UrlData { get; set; }

    #endregion
  }
}
