using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResellerCalculator.Interface
{
  public class ResellerCalculatorRequestData : RequestData
  {
    private string _xmlDoc;
    private TimeSpan _requestTimeout;

    public ResellerCalculatorRequestData(
      string shopperID, string sourceURL, string orderID, string pathway, int pageCount,
      string xmlDoc)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _xmlDoc = xmlDoc;
      _requestTimeout = TimeSpan.FromSeconds(4);
    }

    public string XmlDoc
    {
      get { return _xmlDoc; }
      set { _xmlDoc = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_xmlDoc);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
