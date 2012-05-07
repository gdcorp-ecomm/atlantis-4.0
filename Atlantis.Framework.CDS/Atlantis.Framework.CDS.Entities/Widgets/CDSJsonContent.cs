using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [DataContract()]
  public class CDSJsonContent
  {
    public CDSJsonContent(string targetDivID, string html)
      {
        Html = html;
        TargetDivID = targetDivID;
      }

      [DataMember()]
      public string Html { get; private set; }

      [DataMember()]
      public string TargetDivID { get; private set; }
  }
}
