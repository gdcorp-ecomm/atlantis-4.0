using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Disclaimer : IWidgetModel
  {
    public List<string> Disclaimers { get; set; }
    public bool HasModal { get; set; }

    private ModalData _currentModal;
    public ModalData CurrentModal
    {
      get 
      {
        if (_currentModal == null)
        {
          _currentModal = new ModalData();
        }
        return _currentModal; 
      }
      set { _currentModal = value; }
    }

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
