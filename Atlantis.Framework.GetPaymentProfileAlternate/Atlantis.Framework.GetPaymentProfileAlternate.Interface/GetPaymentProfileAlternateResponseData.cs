using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfileAlternate.Interface
{
  public class GetPaymentProfileAlternateResponseData : IResponseData
  {
    #region Constructors

    public GetPaymentProfileAlternateResponseData(int paymentProfileId, bool bIsSuccess)
    {
      IsSuccess = bIsSuccess;
      PaymentProfileId = paymentProfileId;
    }

    #endregion 

    public bool IsSuccess
    {
      get;
      private set;
    }

    public int PaymentProfileId
    {
      get;
      private set;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Concat("<profile id=>", PaymentProfileId, "</profile>");
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
