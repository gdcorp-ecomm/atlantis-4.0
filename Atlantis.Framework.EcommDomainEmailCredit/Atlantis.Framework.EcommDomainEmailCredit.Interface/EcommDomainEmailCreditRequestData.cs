using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.EcommDomainEmailCredit.Interface
{
  public class EcommDomainEmailCreditRequestData : RequestData
  {
    public EcommDomainEmailCreditRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(5);

    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();

      oMD5.Initialize();

      byte[] stringBytes

      = System.Text.ASCIIEncoding.ASCII.GetBytes(ShopperID + ":" + OrderID);

      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

      string sValue = BitConverter.ToString(md5Bytes, 0);

      return sValue.Replace("-", "");
      
    }
  }
}
