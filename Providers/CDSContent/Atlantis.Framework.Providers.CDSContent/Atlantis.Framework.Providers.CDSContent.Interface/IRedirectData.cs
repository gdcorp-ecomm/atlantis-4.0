using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public interface IRedirectData
  {
    string Type { get; }
    string Location { get; }
  }
}
