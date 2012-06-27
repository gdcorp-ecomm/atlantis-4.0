using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCurrenciesForPaymentType.Interface
{
  public class EcommCurrenciesForPaymentTypeRequestData : RequestData
  {
    private string _basketType = "gdshop";
    private string _paymentType = "credit_card";
    private string _paymentSubType = string.Empty;

    public EcommCurrenciesForPaymentTypeRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {

    }

    ///<summary>
    ///Defaulted to "gdshop"
    ///</summary>
    public string BasketType
    {
      get
      {
        return _basketType;
      }
      set
      {
        _basketType = value;
      }
    }

    ///<summary>
    ///Defaulted to "credit_card". 
    ///</summary>
    ///<remarks>
    ///Proc condition IF "credit_card" ELSE ... 
    ///</remarks>
    public string PaymentType
    {
      get
      {
        return _paymentType;
      }
      set
      {
        _paymentType = value;
      }
    }

    ///<summary>
    ///credit card type (visa, mastercard... if paymentType defaulted to credit_card. 
    ///</summary>
    ///<remarks>
    ///"cc_type" value for credit card profiles, "account_type" value for ach profiles, "type" value for all others
    ///</remarks>    
    public string PaymentSubType
    {
      get
      {
        return _paymentSubType;
      }
      set
      {
        _paymentSubType = value;
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EcommCurrenciesForPaymentType is not a cacheable request.");
    }

  }
}
