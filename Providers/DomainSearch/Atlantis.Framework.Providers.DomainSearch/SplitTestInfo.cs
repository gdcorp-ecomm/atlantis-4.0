using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class SplitTestInfo : ISplitTestInfo
  {public string SplitTestid { get; private set; }
    public string SplitTestSideName { get; private set; }

    private SplitTestInfo(string splitTestid, string splitTestSideName)
    {
      SplitTestid = splitTestid;
      SplitTestSideName = splitTestSideName;
    }

    public static ISplitTestInfo GetSplitTestInfo(string splitTestid, string splitTestSideName)
    {
      return new SplitTestInfo(splitTestid, splitTestSideName);
    }
  }
}
