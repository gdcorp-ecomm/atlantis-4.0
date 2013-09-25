using Atlantis.Framework.Interface;
using System;
using System.Security.Cryptography;

namespace Atlantis.Framework.Purchase.Interface
{
  public class PurchaseErrorRequestData : RequestData
  {
    public string ResponseCode { get; private set; }
    public string ReasonCode { get; private set; }
    public string LanguageCode { get; private set; }
    public string RegionCode { get; private set; }

    public PurchaseErrorRequestData(string response_code,string reason_code,string language_code,string region_code)
    {
      ResponseCode = response_code;
      ReasonCode = reason_code;
      LanguageCode = language_code;
      RegionCode = region_code;
    }

    public override string GetCacheMD5()
    {
      string keyValue= string.Concat(ResponseCode,":",ReasonCode,":",LanguageCode,":",RegionCode);
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(keyValue);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);  
    }
  }
}
