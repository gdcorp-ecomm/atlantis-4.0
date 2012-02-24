using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInstoreStatement.Interface
{
  public class EcommInstoreStatementResponseData: IResponseData
  {
    private bool _success;
    private AtlantisException _exception;
    private DataSet _results;

    public EcommInstoreStatementResponseData(DataSet ds)
    {
      _results = ds;
      _success = true;
    }

    public EcommInstoreStatementResponseData(AtlantisException aex)
    {
      _success = false;
      _exception = aex;
    }

    public EcommInstoreStatementResponseData(RequestData request, Exception ex)
    {
      _success = false;
      _exception = new AtlantisException(request, "EcommInstoreStatementResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public DataSet StatementDataSet
    {
      get
      {
        return _results;
      }
    }

    private string _processedToXMLString = string.Empty;
    public string ProcessedToXMLString
    {
      get
      {
        if (string.IsNullOrEmpty(_processedToXMLString))
        {
          StringBuilder sb = new StringBuilder();
          sb.Append(@"<instorecredits>");

          if (_results.Tables.Count > 0)
          {
            DataRowCollection drc = _results.Tables[0].Rows;
            if (drc != null && drc.Count > 0)
            {
              string currency = string.Empty;
              string tempCurrency = string.Empty;
              bool currencySwitch = false;
              int i = 0;
              int rowType = 0;
              string date = string.Empty;
              string description = string.Empty;
              string amount = string.Empty;

              StringBuilder deposits = new StringBuilder();
              StringBuilder withdrawls = new StringBuilder();

              foreach (DataRow dr in drc)
              {
                int.TryParse(dr["rowType"].ToString(), out rowType);
                tempCurrency = dr["nativeCurrencyType"].ToString();

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

                date = dr["transactionDate"].ToString();
                description = dr["description"].ToString();
                amount = dr["amount"].ToString();
                switch (rowType)
                {
                  case 1:
                    sb.Append("<beginbalance>");
                    sb.Append(AddBlock(date, description, amount));
                    sb.Append("</beginbalance>");
                    break;
                  case 2:
                    deposits.Append("<deposit>");
                    deposits.Append(AddBlock(date, description, amount));
                    deposits.Append("</deposit>");
                    break;
                  case 3:
                    withdrawls.Append("<withdrawl>");
                    withdrawls.Append(AddBlock(date, description, amount));
                    withdrawls.Append("</withdrawl>");
                    break;
                  case 4:
                    sb.Append("<endbalance>");
                    sb.Append(AddBlock(date, description, amount));
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

          _processedToXMLString =  sb.ToString();
        }

        return _processedToXMLString;
      }
    }

    public List<InstoreStatementByCurrency> StatementByCurrencyList
    {
      get
      {
        List<InstoreStatementByCurrency> statementList = new List<InstoreStatementByCurrency>();

        XDocument xdoc = XDocument.Parse(ProcessedToXMLString);

        if (xdoc != null)
        {
          var currencyList = xdoc.Elements("instorecredits").Elements("currency");
          if (currencyList != null)
          {
            
            string currency = string.Empty;

            foreach (XElement currencyElement in currencyList)
            {
              currency = GetAttributeValue(currencyElement, "type");
              InstoreStatementByCurrency statement = new InstoreStatementByCurrency(currency);

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

    #region Private Methods

    private void ProcessSubElements(IEnumerable<XElement> elements, ref InstoreStatementByCurrency statement, int rowtype)
    {
      string date = string.Empty;
      string description = string.Empty;
      int amount = 0;

      if (elements != null)
      {
        foreach (XElement elem in elements)
        {
          date = GetElementValue(elem.Element("date"));
          description = GetElementValue(elem.Element("description"));
          int.TryParse(GetElementValue(elem.Element("amount")), out amount);
          statement.AddItem(date, description, amount, rowtype);
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

    private string AddBlock(string date, string description, string amount)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("<date>");

      DateTime dt; string dts = string.Empty;
      if (DateTime.TryParse(date, out dt))
      {
        dts = dt.ToString("d");
      }
      sb.Append(dts);
      sb.Append("</date>");
      sb.Append("<description><![CDATA[");
      sb.Append(description);
      sb.Append("]]></description>");
      sb.Append("<amount>");
      sb.Append(amount);
      sb.Append("</amount>");

      return sb.ToString();
    }

    #endregion


    #region IResponseData Members
    public string ToXML()
    {
      var xml = string.Empty;

      if (_success)
      {
        var serializer = new XmlSerializer(typeof(DataSet));
        var writer = new StringWriter();
        serializer.Serialize(writer, this._results);
        xml = writer.ToString();
      }

      return xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }

}
