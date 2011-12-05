using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{

  [DataContract]
  public class DataItem
  {
    #region Public Properties

    [DataMember]
    public List<DataAttribute> Attributes
    {
      get;
      set;
    }

    [DataMember(Name = "ID")]
    public string Id
    {
      get;
      set;
    }

    [DataMember(Name = "type")]
    public string MessageType
    {
      get;
      set;
    }

    #endregion

  }
}
