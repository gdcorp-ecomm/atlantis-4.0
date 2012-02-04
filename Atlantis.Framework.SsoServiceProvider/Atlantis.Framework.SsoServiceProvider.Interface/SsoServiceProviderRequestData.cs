using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProvider.Interface
{
  public class SsoServiceProviderRequestData : RequestData
  {
    public SsoServiceProviderRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string serviceProviderKey)
    : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ServiceProviderKey = serviceProviderKey;
    }

    public string ServiceProviderKey { get; private set; }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ServiceProviderKey);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
