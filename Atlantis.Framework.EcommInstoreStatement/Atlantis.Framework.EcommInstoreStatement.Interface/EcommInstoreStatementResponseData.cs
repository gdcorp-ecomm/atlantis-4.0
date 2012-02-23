using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
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

    public XmlDocument InstoreProcessedXML
    {
      get
      {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(ProcessedToXMLString()); 
        return xml;
      }
    }

    private string ProcessedToXMLString()
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
          }

          if (i > 0) { sb.Append("</currency>"); }
        }
      }
      sb.Append("</instorecredits>");
      return sb.ToString();
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
