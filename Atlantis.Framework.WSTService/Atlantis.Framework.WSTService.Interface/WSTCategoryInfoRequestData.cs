using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTCategoryInfoRequestData : RequestData
  {
    public WSTCategoryInfoRequestData(string shopperId,
                                      string sourceURL,
                                      string orderId,
                                      string pathway,
                                      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      md5.Initialize();
      byte[] stringBytes = ASCIIEncoding.ASCII.GetBytes("WSTCategoryInfoRequestData");
      byte[] md5Bytes = md5.ComputeHash(stringBytes);
      string value = BitConverter.ToString(md5Bytes, 0);
      return value.Replace("-", string.Empty);
    }
  }
}
