using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSCreateTdnamAuction.Interface
{
  public class DBSCreateTdnamAuctionRequestData : RequestData
  {
    private string _sellerShopperId;
    private int _resourceId;
    private int _sellerOfferPriceUSD;
    private string _requestXml;
    
    public DBSCreateTdnamAuctionRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, 
    string sellerShopperId, int resourceId, int sellerOfferPriceUSD)
      : base (shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _sellerShopperId = sellerShopperId;
      _resourceId = resourceId;
      _sellerOfferPriceUSD = sellerOfferPriceUSD;      
      _requestXml = RequestXml;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public string SellerShopperId
    {
      get { return _sellerShopperId; }
      set { _sellerShopperId = value; }
    }
    
    public int ResourceId
    {
      get { return _resourceId; }
      set { _resourceId = value; }
    }

    public int SellerOfferPriceUSD
    {
      get { return _sellerOfferPriceUSD; }
      set { _sellerOfferPriceUSD = value; }
    }

    public string RequestXml
    {
      get 
      {
        return string.Format("<auction><shopperId>{0}</shopperId><resourceId>{1}</resourceId><startingPrice>{2}</startingPrice></auction>", SellerShopperId.ToString(), ResourceId.ToString(), SellerOfferPriceUSD.ToString());
      }
    }
    
    public override string GetCacheMD5()
    {
      throw new Exception("DBSCreateTdnamAuction is not a cacheable request.");
    }

  }
}
