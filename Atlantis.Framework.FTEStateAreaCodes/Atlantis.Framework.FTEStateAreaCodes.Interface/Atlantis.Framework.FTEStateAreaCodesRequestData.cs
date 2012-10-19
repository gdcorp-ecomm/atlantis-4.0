using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEStateAreaCodes.Interface
{
  public class FTEStateAreaCodesRequestData : RequestData
  {
    public string GeoCode { get; private set; }

    public FTEStateAreaCodesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string geoCode)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(geoCode))
      {
        throw new ArgumentNullException("The geoCode cannot be null or empty.");
      }

      GeoCode = geoCode;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(GeoCode);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
