using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInstoreBalance.Interface
{
  public class EcommInstoreBalanceResponseData : IResponseData
  {
    public const string _DEPOSIT_DATE = "oldestDepositDate";
    public const string _ACCOUNT_BAL = "account_balance";
    public const string _CUR_TYPE = "nativeCurrencyType";

    #region Properties
    private AtlantisException _exception;

    private DataTable _results;
    public DataTable Balances
    {
      get
      {
        return _results;
      }
    }

    private bool _success;
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    private bool? _hasBalance;
    public bool HasBalance
    {
      get
      {
        if (_hasBalance == null)
        {
          _hasBalance = false;
          foreach (DataRow row in _results.Rows)
          {
            Double balance;
            if (Double.TryParse(row[_ACCOUNT_BAL].ToString(), out balance))
            {
              if (balance > 0)
              {
                _hasBalance = true;
                break;
              }
            }
          }
        }

        return Convert.ToBoolean(_hasBalance);
      }
    }
  
    #endregion

    public EcommInstoreBalanceResponseData(DataTable results)
    {
      _results = results;
      _success = true;
    }

    public EcommInstoreBalanceResponseData(AtlantisException aex)
    {
      _success = false;
      _exception = aex;
    }

    public EcommInstoreBalanceResponseData(RequestData request, Exception ex)
    {
      _success = false;
      _exception = new AtlantisException(request, "EcommInstoreBalanceResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members
    public string ToXML()
    {
      var xml = string.Empty;

      if (_success)
      {
        var serializer = new XmlSerializer(typeof(DataTable));
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
