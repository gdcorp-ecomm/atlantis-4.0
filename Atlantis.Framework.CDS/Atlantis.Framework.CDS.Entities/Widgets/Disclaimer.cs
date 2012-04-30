using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Disclaimer : IWidgetModel
  {
    public List<string> Disclaimers { get; set; }
    public bool HasModal { get; set; }
    public ModalData CurrentModal { get; set; }

    public class ModalData
    {
      public string Symbols { get; set; }
      public string Url { get; set; }
      public string Text { get; set; }

      public int TargetDivWidth { get; set; }
    }
  }
}
