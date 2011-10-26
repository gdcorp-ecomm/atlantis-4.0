using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.StratosphereGetMap.Interface
{
  public class StratosphereGetMapRequestData : RequestData
  {
    #region Properties

    public string MapType { get; private set; }
    public string LookupValue { get; private set; }
    public int NumberOfDomains { get; private set; }
    public string requestUrl { get; private set; }
    public int? UpdatedStratosphereGetMapUrlRequest { get; set; }
    
    #endregion

    public StratosphereGetMapRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string mapType
      , string lookupValue
      , int? numberOfDomains)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      MapType = mapType;
      LookupValue = lookupValue;
      NumberOfDomains = -1;
      if (numberOfDomains.HasValue)
      {
        NumberOfDomains = numberOfDomains.Value;
      }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}-{2}",
        ShopperID, MapType, LookupValue));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
