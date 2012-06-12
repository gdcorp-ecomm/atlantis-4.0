using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using System.ComponentModel;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public enum MarketingButtons
  {
    None,
    Primary,
    Secondary
  }

  public class SalesScript : IWidgetModel
  {
    public SalesScript()
    {
      this.CurrentPopup = new PopupData();
      this.CurrentQuickTour = new ModalData();
      this.CurrentVideo = new VideoData();
    }

    [DisplayName("IPad Video Url")]
    public string IPadVideoUrl { get; set; }
    [DisplayName("Has Quick Tour")]
    public bool HasQuickTour { get; set; }
    [DisplayName("Has Video")]
    public bool HasVideo { get; set; }
    [DisplayName("Has Popup")]
    public bool HasPopup { get; set; }
    public ModalData CurrentQuickTour { get; set; }
    public VideoData CurrentVideo { get; set; }
    public PopupData CurrentPopup { get; set; }

    public class ModalData
    {
      public string Url { get; set; }
      [DisplayName("Target Width")]
      public int TargetDivWidth { get; set; }

      private string _associatedMktgBtn;
      [DisplayName("Associated Marketing Button")]
      public string AssociatedMktgBtn
      {
        get
        {
          return _associatedMktgBtn;
        }
        set
        {
          try
          {
            if (value != null)
            {
              MarketingButtons buttonType = (MarketingButtons)Enum.Parse(typeof(MarketingButtons), value);
            }
          }
          catch (Exception ex)
          {
            throw ex;
          }
          _associatedMktgBtn = value;
        }
      }
    }

    public class VideoData : ModalData
    {
    }

    public class PopupData : ModalData
    {
      [DisplayName("Window Name")]
      public string WindowName { get; set; }
      [DisplayName("Window Params")]
      public string WindowParams { get; set; }
    }
  }
}
