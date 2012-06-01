﻿using System;
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
    }

    [DisplayName("IPad Video Url")]
    public string IPadVideoUrl { get; set; }
    public bool HasQuickTour { get; set; }
    public bool HasVideo { get; set; }
    public bool HasPopup { get; set; }
    [DisplayName("Quick Tour")]
    public ModalData CurrentQuickTour { get; set; }
    [DisplayName("Product Video")]
    public VideoData CurrentVideo { get; set; }
    [DisplayName("Popup")]
    public PopupData CurrentPopup { get; set; }
    public int[] ProductGroupFilters { get; set; }

    public class ModalData
    {
      public string Url { get; set; }
      public int TargetDivWidth { get; set; }

      private string _associatedMktgBtn;
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
            MarketingButtons buttonType = (MarketingButtons)Enum.Parse(typeof(MarketingButtons), value);
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
      public string WindowName { get; set; }
      public string WindowParams { get; set; }
    }
  }
}
