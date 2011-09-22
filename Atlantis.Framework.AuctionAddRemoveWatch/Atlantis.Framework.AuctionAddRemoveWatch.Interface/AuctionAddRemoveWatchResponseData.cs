using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionAddRemoveWatch.Interface
{
  public class AuctionAddRemoveWatchResponseData: IResponseData
  {
    #region Members

    private AtlantisException _atlEx;
    private string _auctionAddRemoveWatchResponseXml;
    
    #endregion


    #region Properties

    public bool IsSuccess { get; private set; }

    public bool IsValidAddRemoveWatchRequest { get; private set;}

    public string ShopperId { get; private set; }

    public string AuctionId { get; private set; }

    public string ErrorNumber { get; private set; }

    public string RawErrorString { get; private set; }

    public string FriendlyErrorString { get; private set; }

    #endregion


    #region Constructors

    public AuctionAddRemoveWatchResponseData(string responseXml)
    {
      if (!string.IsNullOrEmpty(responseXml))
      {
        _auctionAddRemoveWatchResponseXml = responseXml;
        if (ParseAuctionAddRemoveWatchResponse(responseXml))
        {
          IsSuccess = true;
        }
      }
    }

    public AuctionAddRemoveWatchResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
        "AuctionAddRemoveWatchResponseData",
        ex.Message,
        oRequestData.ToXML());  
    }

    #endregion


    #region Methods
    
    private bool ParseAuctionAddRemoveWatchResponse(string responseXml)
    {
      bool success = false;

      XDocument xDoc = XDocument.Parse(responseXml);

      if (xDoc.Root != null)
      {
        XElement auctionWatchResponseElement = xDoc.Element("AddRemoveWatchRsp");

        if (auctionWatchResponseElement != null)
        {
          IsValidAddRemoveWatchRequest = (auctionWatchResponseElement.Attribute("Valid") != null ? string.Compare(auctionWatchResponseElement.Attribute("Valid").Value, "true", true) == 0 : false);
          ShopperId = (auctionWatchResponseElement.Attribute("ShopperId") != null ? auctionWatchResponseElement.Attribute("ShopperId").Value.Trim() : string.Empty);
          AuctionId = (auctionWatchResponseElement.Attribute("AuctionId") != null ? auctionWatchResponseElement.Attribute("AuctionId").Value.Trim() : string.Empty);
          success = true;

          if (!IsValidAddRemoveWatchRequest)
          {
            XElement auctionWatchResponseErrorElement = auctionWatchResponseElement.Element("ErrorData");

            if (auctionWatchResponseErrorElement != null)
            {
              ErrorNumber = (auctionWatchResponseErrorElement.Attribute("errornumber") != null ? auctionWatchResponseErrorElement.Attribute("errornumber").Value.Trim() : string.Empty);
              RawErrorString = (auctionWatchResponseErrorElement.Element("RawErrorString") != null ? auctionWatchResponseErrorElement.Element("RawErrorString").Value.Trim() : string.Empty);
              FriendlyErrorString = (auctionWatchResponseErrorElement.Element("FriendlyErrorString") != null ? auctionWatchResponseErrorElement.Element("FriendlyErrorString").Value.Trim() : string.Empty);
            }
          }
        }
      }

      return success;
    }

    #endregion


    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionAddRemoveWatchResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
