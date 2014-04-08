using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface ISplitTestInfo
  {
    string SplitTestid { get; }
    string SplitTestSideName { get; }
  }
}
