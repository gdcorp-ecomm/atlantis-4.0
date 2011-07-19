using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogOfferClick.Interface
{
  public class LogOfferClickRequestData : RequestData
  {
    #region Properties
    private string _fbiOfferID;
    public string FbiOfferID
    {
      get { return _fbiOfferID; }
      set { _fbiOfferID = value; }
    }

    private string _visitGuid;
    public string VisitGuid
    {
      get { return _visitGuid; }
      set { _visitGuid = value; }
    }

    private DateTime _clickDate;
    public DateTime ClickDate
    {
      get { return _clickDate; }
      set { _clickDate = value; }
    }

    private short _applicationID;
    public short ApplicationID
    {
      get { return _applicationID; }
      set { _applicationID = value; }
    }

    #endregion
    
    public LogOfferClickRequestData(
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, string fbiOfferID, string visitGuid, DateTime clickDate, short applicationID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _fbiOfferID = fbiOfferID;
      _visitGuid = visitGuid;
      _clickDate = clickDate;
      _applicationID = applicationID;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("LogOfferClick is not a cacheable request.");
    }
  }
}
