using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class TabsContainer : IWidgetModel
  {
    private const int _DEFAULT_WIDTH = 918;

    private int _width;
    public int Width
    {
      get
      {
        if (_width == default(int))
        {
          _width = _DEFAULT_WIDTH;
        }
        return _width;
      }
      set
      {
        _width = value;
      }
    }

    public List<Widget<object>> Widgets { get; set; }

    public bool HideBorder { get; set; }
  }
}
