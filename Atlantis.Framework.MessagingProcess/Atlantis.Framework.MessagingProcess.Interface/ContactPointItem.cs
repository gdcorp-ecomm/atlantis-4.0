using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MessagingProcess.Interface
{
  public class ContactPointItem : Dictionary<string, string>
  {
    private string _name;
    private string _type;
    private bool _excludeContactPointType = false;
    private Dictionary<string, string> _resourceItems = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    public ContactPointItem(string name, string type) :
      base(StringComparer.InvariantCultureIgnoreCase)
    {
      _name = name;
      _type = type;
    }

    public ContactPointItem(string name, string type, IDictionary<string, string> attributeItems) :
      base(attributeItems, StringComparer.InvariantCultureIgnoreCase)
    {
      _name = name;
      _type = type;
    }

    public string Name
    {
      get { return _name; }
    }

    public string Type
    {
      get { return _type; }
    }

    public bool ExcludeContactPointType
    {
      get { return _excludeContactPointType; }
      set { _excludeContactPointType = value; }
    }


  }
}
