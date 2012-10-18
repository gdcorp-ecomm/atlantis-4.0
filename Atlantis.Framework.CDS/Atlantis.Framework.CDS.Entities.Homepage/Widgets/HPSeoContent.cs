using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPSeoContent : IWidgetModel
  {
    public string ContentButtonText { get; set; }
    public string SectionTitle { get; set; }
    public string ContentIconImage { get; set; }
    public string ContentTitle { get; set; }
    public string ContentText { get; set; }
  }
}
