using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogOfferImpression.Interface
{
  public class LogOfferImpressionRequestData : RequestData
  {
    private string _fbiOfferIdList;
    private short _applicationID; 
    private DateTime _impressionDate;
    private string _visitGuid;
    private TimeSpan _wsRequestTimeout = TimeSpan.FromSeconds(4);

    public LogOfferImpressionRequestData(
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, string fbiOfferIdList, short applicationID, 
      DateTime impressionDate,string visitGuid)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _fbiOfferIdList = fbiOfferIdList;
      _visitGuid = visitGuid;
      _impressionDate = impressionDate;
      _applicationID = applicationID;
      RequestTimeout = _wsRequestTimeout;
    }

    #region Properties
    public string FbiOfferIdList
    {
      get { return _fbiOfferIdList; }
      set { _fbiOfferIdList = value; }
    }

    public string VisitGuid
    {
      get { return _visitGuid; }
      set { _visitGuid = value; }
    }

    public DateTime ImpressionDate
    {
      get { return _impressionDate; }
      set { _impressionDate = value; }
    }

    public short ApplicationID
    {
      get { return _applicationID; }
      set { _applicationID = value; }
    }
    #endregion

    public override string GetCacheMD5()
    {
      throw new Exception("LogOfferImpression is not a cacheable request.");
    }
  }
}
