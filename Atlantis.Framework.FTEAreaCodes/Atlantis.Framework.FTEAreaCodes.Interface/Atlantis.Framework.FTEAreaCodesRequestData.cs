using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEAreaCodes.Interface
{
  public class FTEAreaCodesRequestData : RequestData
  {
    public string CcCode { get; private set; }

    public FTEAreaCodesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string ccCode)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(ccCode))
      {
        throw new ArgumentNullException("The ccCode cannot be null or empty.");
      }

      CcCode = ccCode;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CcCode);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}