using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.SalesModals
{
    public class FilteredQuickTour : IWidgetModel
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

      public class QuickTourSlide : FilterBase
      {
        public bool HasTextForCenterImage { get; set; }
        public string HeaderText { get; set; }
        public SimpleFilteredItem[] CenterTextList { get; set; }
        public List<BottomContentItem> BottomContentItems { get; set; }

        public class BottomContentItem : SimpleFilteredItem
        {
          public string Paragraph { get; set; }
          public SimpleFilteredItem[] ListItems { get; set; }
        }

        public List<SimpleFilteredItem> CustomContentItems { get; set; }
      }
      public List<QuickTourSlide> Slides { get; set; }
    }
}
