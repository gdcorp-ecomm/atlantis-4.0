using System.Collections.Generic;

namespace Atlantis.Framework.EcommInstoreStatement.Interface
{
  public class InstoreStatementByCurrency
  {
    public string Currency { get; set; }
    public string BeginningBalanceDate { get; set; }
    public int BeginningBalanceAmount { get; set; }
    public string EndingBalanceDate { get; set; }
    public int EndingBalanceAmount { get; set; }
    public List<CreditInfo> Deposits { get; set; }
    public List<CreditInfo> Withdrawls { get; set; }

    public InstoreStatementByCurrency(string currency)
    {
      Currency = currency;
      Deposits = Withdrawls = new List<CreditInfo>();
    }

    public void AddItem(string date, string description, int amount, int rowType)
    {
      switch (rowType)
      {
        case 1:
          BeginningBalanceDate = date;
          BeginningBalanceAmount = amount;
          break;
        case 2:
          CreditInfo d = new CreditInfo(date, description, amount);
          Deposits.Add(d);
          break;
        case 3:
          CreditInfo w = new CreditInfo(date, description, amount);
          Withdrawls.Add(w);
          break;
        case 4:
          EndingBalanceDate = date;
          EndingBalanceAmount = amount;
          break;
      }
    }

    public class CreditInfo
    {
      public string Date { get; set; }
      public string Description { get; set; }
      public int Amount { get; set; }

      public CreditInfo(string date, string description, int amount)
      {
        Date = date;
        Description = description;
        Amount = amount;
      }
    }
  }
}
