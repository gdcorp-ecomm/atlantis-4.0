using System;

namespace Atlantis.Framework.EcommLOCAccountDetails.Interface
{
  public class LineOfCreditTransaction
  {
    public string TransactionType { get; set; }
    public DateTime DisplayDate { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
  }
}
