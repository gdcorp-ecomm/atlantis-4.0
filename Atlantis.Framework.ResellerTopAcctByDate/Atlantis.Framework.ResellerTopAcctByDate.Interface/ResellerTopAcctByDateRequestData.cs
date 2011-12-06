using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResellerTopAcctByDate.Interface
{
  public class ResellerTopAcctByDateRequestData : RequestData
  {
    private DateTime _startDate;
    private DateTime _endDate;
    private int _numRows;

    public ResellerTopAcctByDateRequestData(
      string shopperID, string sourceURL, string orderID, string pathway, int pageCount,
      DateTime startDate, DateTime endDate, int numRows)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _startDate = startDate;
      _endDate = endDate;
      _numRows = numRows;
      RequestTimeout = new TimeSpan(0, 0, 4);
    }

    public DateTime StartDate
    {
      get { return _startDate; }
      set { _startDate = value; }
    }

    public DateTime EndDate
    {
      get { return _endDate; }
      set { _endDate = value; }
    }

    public int NumRows
    {
      get { return _numRows; }
      set { _numRows = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_startDate + ":" + _endDate + ":" + _numRows);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }



  }
}
