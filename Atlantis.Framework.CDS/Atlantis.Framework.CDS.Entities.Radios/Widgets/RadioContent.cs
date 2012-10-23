using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Radios.Widgets
{
  public class RadioContent : IWidgetModel
  {
    public RadioContent()
    {
    }

    public int TabIndex { get; set; }
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    private CurrentRadio _radio;
    public CurrentRadio Radio
    {
      get 
      {
        if (_radio == null)
        {
          _radio = new CurrentRadio();
        }
        return _radio; 
      }
      set { _radio = value; }
    }

    public class CurrentRadio
    {
      public string Text { get; set; }
      public string CiCode { get; set; }
    }
  }
}
