using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionRecommendations.Interface
{
  public class AuctionRecommendationsRequestData : RequestData
  {
    private string _requestXML;
    private string _userID;
    private string _userPwd;
    private string _app;

    public AuctionRecommendationsRequestData(string shopperId, string shopperKey, string sourceUrl, string orderId, string pathway, int pageCount, string app, int timeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestXML = "<auctionrecommendations shopperid='" + shopperId + "' searchbyshopperkey='" + shopperKey + "' />";
      _app = app;
      RequestTimeout = new TimeSpan(0, 0, 0, 0, timeout);
    }


    public AuctionRecommendationsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
    AuctionDomainToCheck domainToCheck, string userID, string userPwd, string app, int timeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      string tlds = domainToCheck.TldIds == string.Empty ? "1,2" : domainToCheck.TldIds;
      if (!string.IsNullOrEmpty(domainToCheck.MinAuctionEndTime))
        _requestXML = "<auctionrecommendations tldid='" + tlds + "' minauctionendtime='" + domainToCheck.MinAuctionEndTime + "'><wordlist><word name='" + domainToCheck.DomainName + "' /></wordlist></auctionrecommendations>";
      else
        _requestXML = "<auctionrecommendations tldid='" + tlds + "'><wordlist><word name='" + domainToCheck.DomainName + "' /></wordlist></auctionrecommendations>";

      _userID = userID;
      _userPwd = userPwd;
      _app = app;
      RequestTimeout = new TimeSpan(0, 0, 0, 0, timeout);
    }

    public string RequestXML
    {
      get { return _requestXML; }
    }
    public string UserID
    {
      get { return _userID; }
    }
    public string UserPwd
    {
      get { return _userPwd; }
    }
    public string App
    {
      get { return _app; }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionRecommendations is not a cacheable request.");
    }
  }
}
