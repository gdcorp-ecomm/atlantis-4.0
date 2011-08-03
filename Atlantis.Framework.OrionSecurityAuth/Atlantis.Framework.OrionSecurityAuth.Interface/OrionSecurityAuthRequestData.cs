using System;
using System.Security.Cryptography;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionSecurityAuth.Interface
{
  public class OrionSecurityAuthRequestData : RequestData
  {
    private int _orionSecurityAuthRequestType = 128;

    public int OrionSecurityAuthRequestType
    {
      get { return _orionSecurityAuthRequestType; }
      set { _orionSecurityAuthRequestType = value; }
    }

    public string AppName { get; private set; }

    public OrionSecurityAuthRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string appName)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      AppName = appName;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes("ORION_SECURITY_AUTH_TOKEN");
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
