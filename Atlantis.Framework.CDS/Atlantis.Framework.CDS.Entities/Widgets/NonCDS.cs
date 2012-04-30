using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("<fieldset><legend>Non CDS Widget</legend><p class=\"non-cds\">Non-cds widget cannot be updated", "</p></fieldset>")]
  public class NonCDS : IWidgetModel
  {
    public NonCDS() { }
  }
}
