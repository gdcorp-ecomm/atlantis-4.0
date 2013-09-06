using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Interface
{
  public class MyaAvailableProductNamespacesRequestData : RequestData
  {
    public string Culture { get; private set; }
    
    public MyaAvailableProductNamespacesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string culture) :
      base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Culture = culture;
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
