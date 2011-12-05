using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract]
  public class MessageData
  {
    [DataMember]
    public List<DataItem> DataItems
    {
      get;
      set;
    }
  }
}
