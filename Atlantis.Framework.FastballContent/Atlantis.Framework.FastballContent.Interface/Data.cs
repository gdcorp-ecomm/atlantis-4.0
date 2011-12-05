using System;
using Atlantis.Framework.Interface;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballContent.Interface
{

  [DataContract]
  public class Data
  {
    #region Public Properties

    [DataMember(Name = "AppID")]
    public int ApplicationId
    {
      get;
      set;
    }

    [DataMember]
    public string OapiPlacement
    {
      get;
      set;
    }

    [DataMember]
    public OfferData OfferData
    {
      get;
      set;
    }

    [DataMember]
    public string Placement
    {
      get;
      set;
    }

    [DataMember(Name = "PrivateLabelID")]
    public int PrivateLabelId
    {
      get;
      set;
    }

    [DataMember(Name = "UrlLibrary")]
    public List<ServerInfo> Servers
    {
      get;
      set;
    }

    [DataMember(Name = "ShopperID")]
    public string ShopperId
    {
      get;
      set;
    }

    #endregion

  }
}
