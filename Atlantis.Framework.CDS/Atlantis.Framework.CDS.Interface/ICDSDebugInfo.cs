using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  public interface ICDSDebugInfo
  {
    ContentId VersionId { get; }
    ContentId DocumentId { get; }
  }
}
