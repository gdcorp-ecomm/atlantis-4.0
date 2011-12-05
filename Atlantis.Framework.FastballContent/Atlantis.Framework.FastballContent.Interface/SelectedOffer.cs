using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract]
  public class SelectedOffer
  {
    #region Public Properties

    [DataMember(Name = "discountType")]
    public string DiscountType
    {
      get;
      set;
    }

    [DataMember(Name = "EndDate")]
    public string EndDate
    {
      get;
      set;
    }

    [DataMember(Name = "fastballDiscount")]
    public string FastballDiscount
    {
      get;
      set;
    }

    [DataMember(Name = "fbiOfferID")]
    public string FastballIntegrationOfferId
    {
      get;
      set;
    }

    [DataMember(Name = "fastballOrderDiscount")]
    public string FastballOrderDiscount
    {
      get;
      set;
    }

    [DataMember]
    public MessageData MessageData
    {
      get;
      set;
    }

    [DataMember(Name = "productGroupCode")]
    public string ProductGroupCode
    {
      get;
      set;
    }

    [DataMember(Name = "promoID")]
    public string PromoId
    {
      get;
      set;
    }

    [DataMember(Name = "TargetDate")]
    public string TargetDate
    {
      get;
      set;
    }

    #endregion

  }
}
