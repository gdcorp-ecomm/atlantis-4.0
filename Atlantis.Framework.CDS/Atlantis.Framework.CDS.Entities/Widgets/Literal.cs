using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Literal : IWidgetModel
  {
    public string Content { get; set; }
    public string Styles { get; set; }
  }
}
