using System;
using Atlantis.Framework.Interface;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract]
  public class OfferResponse
  {
    #region Public Properties

    [DataMember]
    public Data Data
    {
      get;
      set;
    }

    [DataMember]
    public string Html
    {
      get;
      set;
    }

    [DataMember]
    public string OfferServiceVersion
    {
      get;
      set;
    }

    #endregion

  }
}
