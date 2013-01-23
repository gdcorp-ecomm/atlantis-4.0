using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets.Dynamic
{
  public class DynamicMeta : ContentWidget
  {
    private string _canonical;
    private string _description;
    private string _favoriteIcon;
    private string _keywords;

    public DynamicMeta()
    {
      AllowRobots = true;
    }

    public bool AllowRobots { get; set; }

    public string Canonical
    {
      get
      {
        if (string.IsNullOrEmpty(_canonical))
        {
          var child = Children.FirstOrDefault(c => Tag.Link == c.Tag && c.HasAttribute("rel", "canonical"));
          if (!ReferenceEquals(null, child))
          {
            _canonical = child.Attributes["href"];
          }
        }
        return _canonical;
      }
      set
      {
        _canonical = value;
        AddOrUpdateChild(Tag.Link, "rel", "canonical", "href", _canonical);
      }
    }

    public string Description
    {
      get
      {
        if (string.IsNullOrEmpty(_description))
        {
          var child = Children.FirstOrDefault(c => Tag.Meta == c.Tag && c.HasAttribute("name", "description"));
          if (!ReferenceEquals(null, child))
          {
            _description = child.Attributes["content"];
          }
        }
        return _description;
      }
      set
      {
        _description = value;
        var child = AddOrUpdateChild(Tag.Meta, "name", "description", "content", _description);
        child.Index = 1;
      }
    }

    public string FavoriteIcon
    {
      get
      {
        if (string.IsNullOrEmpty(_favoriteIcon))
        {
          var child = Children.FirstOrDefault(c => Tag.Meta == c.Tag && c.HasAttribute("rel", "shortcut icon"));
          if (!ReferenceEquals(null, child))
          {
            _favoriteIcon = child.Attributes["href"];
          }
        }
        return _favoriteIcon;
      }
      set
      {
        _favoriteIcon = value;
        AddOrUpdateChild(Tag.Meta, "rel", "shortcut icon", "href", _favoriteIcon);
      }
    }

    public string Keywords
    {
      get
      {
        if (string.IsNullOrEmpty(_keywords))
        {
          var child = Children.FirstOrDefault(c => Tag.Meta == c.Tag && c.HasAttribute("name", "keywords"));
          if (!ReferenceEquals(null, child))
          {
            _description = child.Attributes["content"];
          }
        }
        return _keywords;
      }
      set
      {
        _keywords = value;
        var child = AddOrUpdateChild(Tag.Meta, "name", "keywords", "content", _keywords);
        child.Index = 2;
      }
    }

    public string Title { get; set; }
  }
}

