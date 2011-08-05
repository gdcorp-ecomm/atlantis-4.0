using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MessagingProcess.Interface
{
  public class ResourceItem : Dictionary<string, AttributeValue>
  {
    string _type = string.Empty;
    string _id = string.Empty;
    List<ContactPointItem> _contactPointItems = new List<ContactPointItem>();

    public ResourceItem(string type, string id, bool bUseCDataBlock)
      : base(StringComparer.InvariantCultureIgnoreCase)
    {
      _type = type;
      _id = id;
    }

    public ResourceItem(string type, string id) 
      : base(StringComparer.InvariantCultureIgnoreCase)
    {
      _type = type;
      _id = id;
    }

    public ResourceItem(string type, string id, IDictionary<string, AttributeValue> attributes)
      : base(attributes, StringComparer.InvariantCultureIgnoreCase)
    {
      _type = type;
      _id = id;
    }

    public string Type
    {
      get { return _type; }
    }

    public string Id
    {
      get { return _id; }
    }

    public List<ContactPointItem> ContactPoints
    {
      get { return _contactPointItems; }
    }

    public void Add(string key, string value)
    {
      base.Add(key, new AttributeValue(value));
    }

  }
}
