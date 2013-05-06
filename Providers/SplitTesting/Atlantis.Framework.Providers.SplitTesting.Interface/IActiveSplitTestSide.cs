using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface IActiveSplitTestSide
  {
    int SideId { get; set; }
    string Name { get; set; }
    double Allocation { get; set; }
  }
}
