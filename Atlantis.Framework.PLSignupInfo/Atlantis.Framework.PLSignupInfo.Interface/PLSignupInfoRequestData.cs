using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PLSignupInfo.Interface
{
  public class PLSignupInfoRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }

    public PLSignupInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(PrivateLabelId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
