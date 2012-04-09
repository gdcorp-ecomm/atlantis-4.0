using System;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.AuthValidatePassword.Interface
{
  public class AuthValidatePasswordRequestData: RequestData
  {
    public string Password { get; set; }

    public AuthValidatePasswordRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string password)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Password = password;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      string key = string.Concat(ShopperID, "|-", Password);
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
