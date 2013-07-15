
namespace Atlantis.Framework.EcommGiftCardStatement.Interface
{
  public class GiftCardTransaction
  {
    public int TransactionType { get; set; }
    public string DisplayDate { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }

    public GiftCardTransaction(int transactionType, string displayDate, string description, string amount)
    {
      TransactionType = transactionType;
      DisplayDate = displayDate;
      Description = description;
      Amount = amount;
    }

  }
}
