using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.StratosphereGetMapUrl.Interface
{
  public class StratosphereGetMapUrlRequestData : RequestData
  {
    #region Properties

    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 10);
    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }

    private int _stratosphereGetMapUrlRequestType = 236;
    public int StratosphereGetMapUrlRequestType
    {
      get { return _stratosphereGetMapUrlRequestType; }
      set { _stratosphereGetMapUrlRequestType = value; }
    }

    public string MapType { get; private set; }
    public string LookupValue { get; private set; }
    public X509Certificate2 Certificate { get; private set; }

    #endregion
    /// <summary>
    /// This is an Internal Request used by Framework StratosphereGetMap.  Call that instead.
    /// </summary>
    public StratosphereGetMapUrlRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string mapType
      , string lookupValue
      , X509Certificate2 cert)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      MapType = mapType;
      if (string.Compare(MapType, "shopper", true) == 0)
      {
        LookupValue = shopperId;
      }
      else
      {
        LookupValue = lookupValue;
      }
      Certificate = cert;
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
