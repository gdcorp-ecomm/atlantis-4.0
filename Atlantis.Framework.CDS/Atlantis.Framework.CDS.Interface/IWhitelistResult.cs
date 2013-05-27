using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  public interface IWhitelistResult
  {
    bool Exists { get; }
    IUrlData UrlData { get; }
  }
}
