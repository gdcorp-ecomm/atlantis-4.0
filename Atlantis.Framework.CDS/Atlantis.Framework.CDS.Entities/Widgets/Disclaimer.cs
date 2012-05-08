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

      private string _text;
      public string Text
      {
        get
        {
          if (_text == default(string))
          {
            _text = "Click here for product disclaimers and legal policies.";
          }
          return _text;
        }
        set
        {
          _text = value;
        }
      }

      public int TargetDivWidth { get; set; }
    }
  }
}
