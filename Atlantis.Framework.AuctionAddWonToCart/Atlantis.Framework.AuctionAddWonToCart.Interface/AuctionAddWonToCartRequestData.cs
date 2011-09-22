using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionAddWonToCart.Interface {
  public class AuctionAddWonToCartRequestData : RequestData
  {
    private const string PIPE_DELIM = "|";
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(10);
    private string _auctionIds = string.Empty;
    private string _isc = string.Empty;
    private string _itc = string.Empty;

    public AuctionAddWonToCartRequestData(List<string> auctionIds, string isc, string itc, string shopperId, string sourceURL, string orderId, string pathway, int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
      _itc = itc;
      _isc = isc;
      if (auctionIds != null && auctionIds.Count > 0)
      {
        _auctionIds = auctionIds.Count > 1 ? string.Join(PIPE_DELIM, auctionIds.ToArray()) : auctionIds[0];
      }
    }

    /// <summary>
    /// Pipe-Delimited string of auction ids to send to cart
    /// </summary>
    public string AuctionIds
    {
      get {
        return _auctionIds;
      }
      set { _auctionIds = value; }
    }

    public string Isc
    {
      get {
        return _isc;
      }
      set { _isc = value; }
    }

    public string Itc
    {
      get {
        return _itc;
      }
      set { _itc = value; }
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionAddWonToCart is not a cacheable request.");
    }

    #endregion
  }
}
