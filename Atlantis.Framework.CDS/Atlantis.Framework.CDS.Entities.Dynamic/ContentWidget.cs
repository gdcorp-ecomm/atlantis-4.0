using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets.Dynamic
{
  public class ContentWidget : SimpleFilteredItem, IWidgetModel
  {
    private Lazy<Dictionary<string, string>> _attributes = new Lazy<Dictionary<string, string>>();
    private Lazy<ObservableCollection<ContentWidget>> _children = new Lazy<ObservableCollection<ContentWidget>>();
    private Lazy<List<Widget<IWidgetModel>>> _widgets = new Lazy<List<Widget<IWidgetModel>>>();

    public ContentWidget()
    {
      Attributes = new Dictionary<string, string>();
      Children = new Collection<ContentWidget>();
      Widgets = new List<Widget<IWidgetModel>>();
    }

    public Dictionary<string, string> Attributes
    {
      get
      {
        return _attributes.Value;
      }
      set
      {
        if (!ReferenceEquals(null, value) && !ReferenceEquals(value, _attributes.Value))
        {
          _attributes = new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase));
        }

      }
    }

    public Collection<ContentWidget> Children
    {
      get
      {
        return _children.Value;
      }
      set
      {
        if (!ReferenceEquals(null, value) && !ReferenceEquals(value, _children.Value))
        {
          var previousValue = _children.Value as ObservableCollection<ContentWidget>;
          if (!ReferenceEquals(null, previousValue))
          {
            previousValue.CollectionChanged -= OnChildrenChanged;
          }
          var wrapped = new ObservableCollection<ContentWidget>(value);
          wrapped.CollectionChanged += OnChildrenChanged;
          _children = new Lazy<ObservableCollection<ContentWidget>>(() => wrapped);
        }
      }
    }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets
    {
      get
      {
        return _widgets.Value;
      }
      set
      {
        _widgets = new Lazy<List<Widget<IWidgetModel>>>(() => new List<Widget<IWidgetModel>>(value));
      }
    }

    public int Index { get; set; }

    [Required]
    public Tag Tag
    {
      get;
      set;
    }

    public bool HasAttribute(KeyValuePair<string, string> attribute)
    {
      bool returnValue = false;

      if (!ReferenceEquals(null, attribute))
      {
        returnValue = HasAttribute(attribute.Key, attribute.Value);
      }

      return returnValue;
    }

    public bool HasAttribute(string key, string value)
    {
      bool returnValue = false;

      if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
      {
        returnValue = !ReferenceEquals(null, this.Attributes) &&
            Attributes.ContainsKey(key) &&
            value.Equals(Attributes[key], StringComparison.OrdinalIgnoreCase);
      }

      return returnValue;
    }

    protected ContentWidget AddOrUpdateChild(Tag tagName, string findKey, string findValue, string newKey, string newValue)
    {
      KeyValuePair<string, string> findAttribute = new KeyValuePair<string, string>(findKey, findValue);
      KeyValuePair<string, string> newAttribute = new KeyValuePair<string, string>(newKey, newValue);
      return AddOrUpdateChild(tagName, findAttribute, newAttribute);
    }

    protected ContentWidget AddOrUpdateChild(Tag tagName, KeyValuePair<string, string> findAttribute, KeyValuePair<string, string> newAttribute)
    {
      ContentWidget returnValue = new ContentWidget() { Tag = tagName };

      returnValue = Children.FirstOrDefault(c => tagName == c.Tag && c.HasAttribute(findAttribute));
      if (ReferenceEquals(null, returnValue))
      {
        returnValue = new ContentWidget() { Tag = tagName };
        returnValue.Attributes.Add(findAttribute.Key, findAttribute.Value);
        Children.Add(returnValue);
      }
      returnValue.Attributes[newAttribute.Key] = newAttribute.Value;

      return returnValue;
    }

    protected void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (!ReferenceEquals(null, e))
      {
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
          case NotifyCollectionChangedAction.Move:
          case NotifyCollectionChangedAction.Remove:
            // TODO: Re-Index the collection
            break;
          case NotifyCollectionChangedAction.Replace:
            break;
          case NotifyCollectionChangedAction.Reset:
            break;
          default:
            break;
        }
      }

      if (!ReferenceEquals(null, e.NewItems) && 0 < e.NewItems.Count)
      {
        foreach (var item in e.NewItems.Cast<ContentWidget>())
        {
          item.Index = this.Children.Count;
        }
      }
    }
  }
}
