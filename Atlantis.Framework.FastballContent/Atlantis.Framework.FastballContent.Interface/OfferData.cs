using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract]
  public class OfferData
  {
    #region Public Properties

    [DataMember]
    public string CandidateAttributeXml
    {
      get;
      set;
    }

    [DataMember]
    public int ResultCode
    {
      get;
      set;
    }

    [DataMember]
    public List<SelectedOffer> SelectedOffers
    {
      get;
      set;
    }

    #endregion

  }
}
