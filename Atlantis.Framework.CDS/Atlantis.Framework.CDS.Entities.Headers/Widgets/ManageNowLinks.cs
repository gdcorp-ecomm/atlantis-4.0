using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.Headers.Widgets
{
  /// <summary>
  /// This is a new widget and is not meant to be backwards compatible with Atlantis.Framework.CDS.Entities.Widgets.ManageNow 
  /// </summary>
  public class ManageNowLinks : IWidgetModel
  {
    private string _linkSeparator;
    public string LinkSeparator
    {
      get
      {
        if (string.IsNullOrEmpty(_linkSeparator))
        {
          _linkSeparator = "&nbsp;&nbsp;|&nbsp;&nbsp;";
        }
        return _linkSeparator; 
      }
      set
      { 
        _linkSeparator = value;
      }
    }

    private string _description;
    public string Description
    {
      get
      {
        if (_description == default(string))
        {
          _description = "Already own this product?";
        }
        return _description;
      }
      set
      {
        _description = value;
      }
    }

    private List<Link> _links;
    public List<Link> Links
    {
      get
      {
        if (_links == null)
        {
          _links = new List<Link>();
        }
        return _links;
      }
      set
      {
        _links = value;
      }
    }

    private bool? _addManagerQueryStringParamsIfManager;
    public bool? AddManagerQueryStringParamsIfManager
    {
      get
      {
        if (!_addManagerQueryStringParamsIfManager.HasValue)
        {
          _addManagerQueryStringParamsIfManager = true;
        }
        return _addManagerQueryStringParamsIfManager.Value;
      }
      set
      {
        _addManagerQueryStringParamsIfManager = value;
      }
    }

    public class Link : SimpleFilteredItem
    {
      public string LinkToken { get; set; }
    }
  }
}
