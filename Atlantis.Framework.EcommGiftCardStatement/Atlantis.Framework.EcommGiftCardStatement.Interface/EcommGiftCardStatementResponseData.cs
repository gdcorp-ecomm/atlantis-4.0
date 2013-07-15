using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardStatement.Interface
{
  public class EcommGiftCardStatementResponseData : IResponseData
  {
    private AtlantisException _exception;
    public bool IsSuccess { get; set; }
    public List<GiftCardTransaction> GiftCardTransactionList = new List<GiftCardTransaction>(2); 
    
    public EcommGiftCardStatementResponseData(List<GiftCardTransaction> giftCardTransaction)
    {
      IsSuccess = true;
      GiftCardTransactionList = giftCardTransaction;
    }

    public EcommGiftCardStatementResponseData(AtlantisException aex)
    {
      IsSuccess = false;
      _exception = aex;
    }

    public EcommGiftCardStatementResponseData(RequestData request, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(request, "EcommGiftCardStatementResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members
    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion

  }
}
