using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class MyaAccordionMetaDataRequestData : RequestData
  {

    private static int _minimumAccordionMetaDataCount = 30;
    static public int MinimumAccordionMetaDataCount
    {
      get { return _minimumAccordionMetaDataCount; }
      set { _minimumAccordionMetaDataCount = value; }
    }

    public MyaAccordionMetaDataRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Empty);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
