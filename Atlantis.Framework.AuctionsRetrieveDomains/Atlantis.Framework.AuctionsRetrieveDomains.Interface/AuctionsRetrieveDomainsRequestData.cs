using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsRetrieveDomains.Interface
{
  public class AuctionsRetrieveDomainsRequestData : RequestData
  {
    private int _auctionCount;

    public AuctionsRetrieveDomainsRequestData( 
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, int auctionCount)
      : base (shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _auctionCount = auctionCount;
      RequestTimeout = new TimeSpan(0, 0, 4);
    }

    public int AuctionCount
    {
      get { return _auctionCount; }
      set { _auctionCount = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_auctionCount.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
