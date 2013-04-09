using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInstoreStatement.Interface
{
  public class EcommInstoreStatementResponseData: IResponseData
  {
    public DataSet StatementDataSet { get; private set; }
    private readonly AtlantisException _exception;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EcommInstoreStatementResponseData(DataSet ds)
    {
      StatementDataSet = ds;
    }

    public EcommInstoreStatementResponseData(AtlantisException aex)
    {
      _exception = aex;
    }

    public EcommInstoreStatementResponseData(RequestData request, Exception ex)
    {
      _exception = new AtlantisException(request, "EcommInstoreStatementResponseData", ex.Message, string.Empty);
    }

    #region Public Accessors

    private string _processedToXmlString = string.Empty;
    public string ProcessedToXmlString
    {
      get
      {
        if (string.IsNullOrEmpty(_processedToXmlString))
        {
          var sb = new StringBuilder();
          sb.Append(@"<instorecredits>");

          if (StatementDataSet.Tables.Count > 0)
          {
            DataRowCollection drc = StatementDataSet.Tables[0].Rows;
            if (drc != null && drc.Count > 0)
            {
              var currency = string.Empty;
              var i = 0;

              var deposits = new StringBuilder();
              var withdrawls = new StringBuilder();

              foreach (DataRow dr in drc)
              {
                int rowType;
                int.TryParse(dr["rowType"].ToString(), out rowType);
                var tempCurrency = dr["nativeCurrencyType"].ToString();

                bool currencySwitch;
                if (!string.Equals(currency, tempCurrency))
                {
                  currencySwitch = true;
                  currency = tempCurrency;
                }
                else
                {
                  currencySwitch = false;
                }

                if (currencySwitch)
                {
                  if (i != 0)
                  {
                    sb.Append("<deposits>");
                    sb.Append(deposits.ToString());
                    sb.Append("</deposits>");

                    sb.Append("<withdrawls>");
                    sb.Append(withdrawls.ToString());
                    sb.Append("</withdrawls>");

                    sb.Append("</currency>");

                    deposits.Clear();
                    withdrawls.Clear();
                  }
                  sb.Append("<currency type=\"");
                  sb.Append(currency);
                  sb.Append("\">");
                }

                string date = dr["transactionDate"].ToString();
                string description = dr["description"].ToString();
                string amount = dr["amount"].ToString();
                string expirationDate = dr["expirationDate"] != DBNull.Value ? dr["expirationDate"].ToString() : "N/A";
                switch (rowType)
                {
                  case 1:
                    sb.Append("<beginbalance>");
                    sb.Append(AddBlock(date, description, amount, expirationDate));
                    sb.Append("</beginbalance>");
                    break;
                  case 2:
                    deposits.Append("<deposit>");
                    deposits.Append(AddBlock(date, description, amount, expirationDate));
                    deposits.Append("</deposit>");
                    break;
                  case 3:
                    withdrawls.Append("<withdrawl>");
                    withdrawls.Append(AddBlock(date, description, amount, expirationDate));
                    withdrawls.Append("</withdrawl>");
                    break;
                  case 4:
                    sb.Append("<endbalance>");
                    sb.Append(AddBlock(date, description, amount, expirationDate));
                    sb.Append("</endbalance>");
                    break;
                }

                i++;

                if (i == drc.Count)
                {
                  sb.Append("<deposits>");
                  sb.Append(deposits.ToString());
                  sb.Append("</deposits>");

                  sb.Append("<withdrawls>");
                  sb.Append(withdrawls.ToString());
                  sb.Append("</withdrawls>");

                  sb.Append("</currency>");
                }
              }

            }
          }
          sb.Append("</instorecredits>");

          _processedToXmlString =  sb.ToString();
        }

        return _processedToXmlString;
      }
    }

    public List<InstoreStatementByCurrency> EmptyStatement
    {
      get
      {
        var statementList = new List<InstoreStatementByCurrency>();

        var sb = new StringBuilder();
        sb.Append(@"<instorecredits>");
        sb.Append(BuildInvalidDateRangeXml());
        sb.Append("</instorecredits>");

        var xdoc = XDocument.Parse(sb.ToString());

        if (xdoc != null)
        {
          var currencyList = xdoc.Elements("instorecredits").Elements("currency");
          if (currencyList != null)
          {
            foreach (XElement currencyElement in currencyList)
            {
              string currency = GetAttributeValue(currencyElement, "type");
              var statement = new InstoreStatementByCurrency(currency);

              IEnumerable<XElement> beginingBalance = currencyElement.Elements("beginbalance");
              ProcessSubElements(beginingBalance, ref statement, 1);

              IEnumerable<XElement> endBalance = currencyElement.Elements("endbalance");
              ProcessSubElements(endBalance, ref statement, 4);

              IEnumerable<XElement> deposits = currencyElement.Elements("deposits").Elements("deposit");
              ProcessSubElements(deposits, ref statement, 2);

              IEnumerable<XElement> withdrawls = currencyElement.Elements("withdrawls").Elements("withdrawl");
              ProcessSubElements(withdrawls, ref statement, 3);

              statementList.Add(statement);
            }
          }
        }
        return statementList;
      }
    }

    public List<InstoreStatementByCurrency> StatementByCurrencyList
    {
      get
      {
        var statementList = new List<InstoreStatementByCurrency>();

        var xdoc = XDocument.Parse(ProcessedToXmlString);

        if (xdoc != null)
        {
          var currencyList = xdoc.Elements("instorecredits").Elements("currency");
          if (currencyList != null)
          {
            foreach (XElement currencyElement in currencyList)
            {
              var currency = GetAttributeValue(currencyElement, "type");
              var statement = new InstoreStatementByCurrency(currency);

              IEnumerable<XElement> beginingBalance = currencyElement.Elements("beginbalance");
              ProcessSubElements(beginingBalance, ref statement, 1);

              IEnumerable<XElement> endBalance = currencyElement.Elements("endbalance");
              ProcessSubElements(endBalance, ref statement, 4);

              IEnumerable<XElement> deposits = currencyElement.Elements("deposits").Elements("deposit");
              ProcessSubElements(deposits, ref statement, 2);

              IEnumerable<XElement> withdrawls = currencyElement.Elements("withdrawls").Elements("withdrawl");
              ProcessSubElements(withdrawls, ref statement, 3);

              statementList.Add(statement);
            }
          }
        }

        return statementList;
      }
    }
    #endregion 

    #region Private Methods

    private string BuildInvalidDateRangeXml()
    {
      var currencyType = new XElement("currency", 
        new XAttribute("type", "USD"));

      var description = new XElement("description");
      description.Add(new XCData(""));

      var beginBal = new XElement("beginbalance", 
        new XElement("date", "This date range is outside your in-store credit history."),
        new XElement(description),
        new XElement("amount", 0));

      var endBal = new XElement("endbalance",
        new XElement("date", "This date range is outside your in-store credit history."),
        new XElement(description),
        new XElement("amount", 0));

      var deposits = new XElement("deposits");
      var withdrawal = new XElement("withdrawls");

      currencyType.Add(beginBal);
      currencyType.Add(endBal);
      currencyType.Add(deposits);
      currencyType.Add(withdrawal);

      return currencyType.ToString();    
    }

    private void ProcessSubElements(IEnumerable<XElement> elements, ref InstoreStatementByCurrency statement, int rowtype)
    {
      if (elements != null)
      {
        foreach (XElement elem in elements)
        {
          var date = GetElementValue(elem.Element("date"));
          var description = GetElementValue(elem.Element("description"));
          int amount;
          int.TryParse(GetElementValue(elem.Element("amount")), out amount);
          var expirationDate = GetElementValue(elem.Element("exdate"));
          statement.AddItem(date, description, amount, rowtype, expirationDate);
        }
      }
    }

    private static string GetElementValue(XElement e)
    {
      var value = string.Empty;
      if (e != null)
      {
        value = e.Value;
      }
      return value;
    }

    private static string GetAttributeValue(XElement e, string name)
    {
      var value = string.Empty;
      if (!string.IsNullOrEmpty(name) && e != null && e.HasAttributes && e.Attribute(name) != null)
      {
        value = e.Attribute(name).Value;
      }
      return value;
    }

    private string AddBlock(string date, string description, string amount, string expirationDate)
    {
      var sb = new StringBuilder();

      DateTime dt; 
      var dts = string.Empty;
      if (DateTime.TryParse(date, out dt))
      {
        dts = dt.ToString("d");
      }

      DateTime edt; 
      var edts = string.Empty;
      if (DateTime.TryParse(expirationDate, out edt))
      {
        edts = edt.ToString("d");
      }
      sb.Append("<date>");
      sb.Append(dts);
      sb.Append("</date>");
      sb.Append("<description><![CDATA[");
      sb.Append(description);
      sb.Append("]]></description>");
      sb.Append("<amount>");
      sb.Append(amount);
      sb.Append("</amount>");
      sb.Append("<exdate>");
      sb.Append(edts);
      sb.Append("</exdate>");

      return sb.ToString();
    }

    #endregion

    #region IResponseData Members
    public string ToXML()
    {
      return IsSuccess ? StatementDataSet.GetXml() : string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
