using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoGetKeyRequestData : RequestData
  {
    public Token Token { get; private set; }

    public SsoGetKeyRequestData(string rawTokenData, int privateLabelId )
    {
      Token = new Token();
      Token.data = rawTokenData;
      Token.PrivateLabelId = privateLabelId;
    }

    public SsoGetKeyRequestData(Token token)
    {
      Token = token;
    }
    
    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(Token.Header.kid);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

  }
}
