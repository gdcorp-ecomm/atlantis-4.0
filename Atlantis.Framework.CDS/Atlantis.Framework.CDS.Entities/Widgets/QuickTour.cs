using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using System.IO;
using System.Web.UI;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class QuickTour : IWidgetModel
  {
      public int ProductGroup { get; set; }
      public string PageTitle { get; set; }

      private string _bodyImageBasePath;
      public string BodyImageBasePath
      {
        get
        {
          if (_bodyImageBasePath == default(string))
          {
            _bodyImageBasePath = string.Empty;
          }
          return _bodyImageBasePath;
        }
        set
        {
          _bodyImageBasePath = value;
        }
      }

      public class QuickTourSlide
      {
        public bool HasTextForCenterImage { get; set; }
        public string HeaderText { get; set; }
        public string[] CenterTextList { get; set; }
        public List<BottomContentItem> BottomContentItems { get; set; }

        public class BottomContentItem
        {
          public string Paragraph { get; set; }
          public string[] ListItems { get; set; }
          public string Text { get; set; }
        }
      }

      public List<QuickTourSlide> Slides { get; set; }
  }
}
