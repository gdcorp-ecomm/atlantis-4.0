using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using System.IO;
using System.Web.UI;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class QuickTour : IWidgetModel
  {
      public int CurrentProduct { get; set; }
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

        public string GetBottomContentText()
        {
          string result = string.Empty;
          if (this.BottomContentItems != null && this.BottomContentItems.Count > 0)
          {
            StringBuilder sb = new StringBuilder();
            foreach (BottomContentItem item in this.BottomContentItems)
            {
              sb.Append(item.Text);
            }
            result = sb.ToString();
          }
          return result;
        }

        public class BottomContentItem
        {
          public virtual string Text { get; set; }
        }

        public class BottomContentParagraph : BottomContentItem
        {
          public string Paragraph { get; set; }

          public override string Text
          {
            get
            {
              return string.Format("<p>{0}</p>", this.Paragraph);
            }
          }
        }

        public class BottomContentList : BottomContentItem
        {
          public string[] ListItems { get; set; }

          public override string Text
          {
            get
            {
              return this.GetMarkupForCurrentList();
            }
          }

          private string GetMarkupForCurrentList()
          {
            string result = string.Empty;
            if (this.ListItems != null && this.ListItems.Length > 0)
            {
              string className = "listdisc";
              StringWriter sw = new StringWriter();
              using (HtmlTextWriter writer = new HtmlTextWriter(sw))
              {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, className);
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (string item in this.ListItems)
                {
                  writer.RenderBeginTag(HtmlTextWriterTag.Li);
                  writer.Write(item);
                  writer.RenderEndTag();
                }
                writer.RenderEndTag();
              }
              result = sw.ToString();
            }
            return result;
          }
        }
      }

      public List<QuickTourSlide> Slides { get; set; }

      // Custom converter for bottom content to correctly deserialize to correct derived class if applicable
      public class BottomContentItemConverter : JsonCreationConverter<QuickTour.QuickTourSlide.BottomContentItem>
      {
        protected override QuickTour.QuickTourSlide.BottomContentItem Create(Type objectType, JObject jObject)
        {
          if (FieldExists("Paragraph", jObject))
          {
            return new QuickTour.QuickTourSlide.BottomContentParagraph();
          }
          else if (FieldExists("ListItems", jObject))
          {
            return new QuickTour.QuickTourSlide.BottomContentList();
          }
          else
          {
            return new QuickTour.QuickTourSlide.BottomContentItem();
          }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
          return jObject[fieldName] != null;
        }
      }
  }
}
