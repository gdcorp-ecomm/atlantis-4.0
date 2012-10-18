using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPSingleVideoPod : IWidgetModel
  {
    public IList<HPSingleVideoPodItem> SingleVideoPodItemList { get; set; }
    public HPSingleVideoPod()
    {
      SingleVideoPodItemList = new List<HPSingleVideoPodItem>();
    }
  }

  public class HPSingleVideoPodItem : IWidgetModel
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
