using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainBundleId.Interface
{
  public class DomainBundleIdRequestData : RequestData
  {
    /// <summary>
    /// DCC domain Id or the "Domain" ResoureceId from the billing table (do not use the resourceId from the DCC web service.)
    /// </summary>
    public int DomainId { get; private set; }
    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 2);
    
    /// <summary>
    /// Use to get the bundle id (resourceid) of the domain bundle)
    /// </summary>
    /// <param name="shopperId"></param>
    /// <param name="sourceUrl"></param>
    /// <param name="orderId"></param>
    /// <param name="pathway"></param>
    /// <param name="pageCount"></param>
    /// <param name="domainId">DCC domain Id or the "Domain" ResoureceId from the billing table (do not use the resourceId attribute value from the DCC web service.)</param>
    public DomainBundleIdRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int domainId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainId = domainId;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = ASCIIEncoding.ASCII.GetBytes(ShopperID + DomainId);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }


  }
}
