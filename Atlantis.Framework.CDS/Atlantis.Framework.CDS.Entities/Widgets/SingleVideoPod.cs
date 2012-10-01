using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SingleVideoPod : IWidgetModel
  {
    public IList<SingleVideoPodItem> SingleVideoPodItemList { get; set; }
    public SingleVideoPod()
    {
      SingleVideoPodItemList = new List<SingleVideoPodItem>();
    }
  }

  public class SingleVideoPodItem : IWidgetModel
  {
    public string VideoTitle { get; set; }
    public string BackgroundImage { get; set; }
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public string VideoMediaId { get; set; }
    public string VideoLanguage { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
