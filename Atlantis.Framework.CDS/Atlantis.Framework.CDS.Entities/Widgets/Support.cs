﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Support Footer")]
  public class Support : FooterBase, IWidgetModel
  {
    private string _title = "Support";
    public string Title
    {
      get { return _title; }
      set { _title = value; }
    }
    public string CustomSupportNumber { get; set; }

    private string _communityGroup = "product group of your choice";
    public string CommunityGroup
    {
      get { return _communityGroup; }
      set { _communityGroup = value; }
    }

    private bool _hideResponseTime;
    public bool HideResponseTime
    {
      get { return _hideResponseTime; }
      set { _hideResponseTime = value; }
    }

    private bool _showAdditionalSupportText;
    public bool ShowAdditionalSupportText
    {
      get { return _showAdditionalSupportText; }
      set { _showAdditionalSupportText = value; }
    }

    private string _additionalSupportText = string.Empty;
    public string AdditionalSupportText
    {
      get { return _additionalSupportText; }
      set { _additionalSupportText = value; }
    }

    public List<Section> Sections { get; set; }

    public class Section : ElementBase
    {
      public Section(string title)
      {
        Title = title;
      }

      public List<Column> Columns { get; set; }

      private string _title = string.Empty;
      public string Title
      {
        get { return _title; }
        set { _title = value; }
      }

      public class Column
      {
        public string ColumnTitle { get; set; }
        public List<ListItem> ListItems { get; set; }
      }

      public class ListItem : ElementBase
      {
        public ListItem(string text)
        {
          Text = text;
        }
      }
    }
  }
}
