using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

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

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    public bool HideBorder { get; set; }
  }
}
