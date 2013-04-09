using System.Collections.Generic;

namespace Atlantis.Framework.EcommInstoreStatement.Interface
{
  public class InstoreStatementByCurrency
  {
    public string Currency { get; set; }
    public string BeginningBalanceDate { get; private set; }
    public int BeginningBalanceAmount { get; private set; }
    public string EndingBalanceDate { get; private set; }
    public int EndingBalanceAmount { get; private set; }
    public List<CreditInfo> Deposits { get; private set; }
    public List<CreditInfo> Withdrawls { get; private set; }

    public InstoreStatementByCurrency(string currency)
    {
      Currency = currency;
      Deposits = new List<CreditInfo>();
      Withdrawls = new List<CreditInfo>();
    }

    public void AddItem(string date, string description, int amount, int rowType, string expirationDate)
    {
      switch (rowType)
      {
        case 1:
          BeginningBalanceDate = date;
          BeginningBalanceAmount = amount;
          break;
        case 2:
          var d = new CreditInfo(date, description, amount, expirationDate);
          Deposits.Add(d);
          break;
        case 3:
          var w = new CreditInfo(date, description, amount, expirationDate);
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
      public string Date { get; private set; }
      public string Description { get; private set; }
      public int Amount { get; private set; }
      public string ExpirationDate { get; private set; }

      public CreditInfo(string date, string description, int amount, string expirationDate)
      {
        Date = date;
        Description = description;
        Amount = amount;
        ExpirationDate = expirationDate;
      }
    }
  }
}
