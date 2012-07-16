using System;
using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.EcommLOCAccountDetails.Interface
{
  public class LineOfCreditAccountDetail
  {
    public int AccountId { get; set; }
    public string MaskedAccountNumber { get; set; }
    public int CreditLimit { get; set; }
    public int CreditAvailable { get; set; }
    public string CurrencyType { get; set; }
    public string AccountType { get; set; }
    public string AccountStatus { get; set; }
    public List<LineOfCreditTransaction> Transactions { get; set; }

    public LineOfCreditAccountDetail()
    {
      Transactions = new List<LineOfCreditTransaction>();
    }

    public LineOfCreditAccountDetail(string responseXml)
    {
      Transactions = new List<LineOfCreditTransaction>();
      GetDetails(responseXml);
    }

    private void GetDetails(string responseXml)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(responseXml);

      XmlNode detailNode = doc.SelectSingleNode("AccountDetails");
      if (detailNode != null)
      {
        int detailId = 0;
        int.TryParse(detailNode.Attributes["accountID"].Value, out detailId);

        AccountId = detailId;
        MaskedAccountNumber = detailNode.Attributes["maskedAccountNumber"].Value;

        int creditLimit = 0;
        if (int.TryParse(detailNode.Attributes["creditLimit"].Value, out creditLimit))
        {
          CreditLimit = creditLimit;
        }
        else
        {
          CreditLimit = 0;
        }

        int creditAvailable = 0;
        if (int.TryParse(detailNode.Attributes["creditAvailable"].Value, out creditAvailable))
        {
          CreditAvailable = creditAvailable;
        }
        else
        {
          CreditAvailable = 0;
        }

        CurrencyType = detailNode.Attributes["currencyType"].Value;
        AccountType = UcaseFirst(detailNode.Attributes["accountType"].Value);
        AccountStatus = UcaseFirst(detailNode.Attributes["accountStatus"].Value);


        XmlNodeList trasactions = detailNode.SelectNodes("./Detail");
        if (trasactions != null && trasactions.Count > 0)
        {
          foreach (XmlNode transactionNode in trasactions)
          {
            LineOfCreditTransaction tran = new LineOfCreditTransaction();
            string rowType = transactionNode.Attributes["rowType"].Value;
            switch (rowType)
            {
              case "1":
                tran.TransactionType = "Beginning Balance";
                break;
              case "2":
                tran.TransactionType = "";
                break;
              case "3":
                tran.TransactionType = "";
                break;
              case "4":
                tran.TransactionType = "Ending Balance";
                break;

            }

            DateTime displayDate;
            if (DateTime.TryParse(transactionNode.Attributes["displayDate"].Value, out displayDate))
            {
              tran.DisplayDate = displayDate;
            }

            tran.Description = transactionNode.Attributes["description"].Value;

            int amount = 0;
            if (int.TryParse(transactionNode.Attributes["amount"].Value, out amount))
            {
              tran.Amount = amount;
            }
            else
            {
              tran.Amount = 0;
            }


            Transactions.Add(tran);
          }
        }
      }
    }

    private string UcaseFirst(string str)
    {
      string result = string.Empty;

      if (!string.IsNullOrEmpty(str) && str.Length > 1)
      {
        result = str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1).ToLower();
      }
      return result;
    }   

  }
}
