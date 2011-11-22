using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.OrionAddAttribute.Interface.Types
{
  public class OrionAttribute
  {
    public string Name { get; private set; }
    /// <summary>
    /// List of key (Name) and value (Value) pairs representing Orion AttributeElements
    /// </summary>
    public List<KeyValuePair<string, string>> Elements { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">AttributeName</param>
    /// <param name="elements">List of key (Name) and value (Value) pairs representing Orion AttributeElements</param>
    public OrionAttribute(string name, List<KeyValuePair<string, string>> elements)
    {
      Name = name;
      Elements = elements;
    }
  }
}
