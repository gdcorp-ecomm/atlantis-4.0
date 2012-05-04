using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.UserAgentEx.Interface
{
  public class UserAgentExRequestData : RequestData
  {
    public int ExpressionType { get; private set; }

    public UserAgentExRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int expressionType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ExpressionType = expressionType;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ExpressionType.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
