using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract(Name="Attribute")]
  public class DataAttribute
  {
    [DataMember]
    public List<string> Values
    {
      get;
      set;
    }

    [DataMember(Name="key")]
    public string Key
    {
      get;
      set;
    }
  }
}
