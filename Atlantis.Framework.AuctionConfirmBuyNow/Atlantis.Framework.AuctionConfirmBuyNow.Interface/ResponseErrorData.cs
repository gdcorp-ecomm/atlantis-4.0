using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface
{
  public class ResponseErrorData
  {
    public int ErrorNumber { get; set; }
    public string ErrorMessage { get; set; }

    public ResponseErrorData()
    {
    }

    public ResponseErrorData(int errorNumber, string errorMessage)
    {
      ErrorNumber = errorNumber;
      ErrorMessage = errorMessage;
    }
  }
}
