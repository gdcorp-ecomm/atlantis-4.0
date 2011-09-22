using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface 
{
  public class AuctionConfirmBuyNowResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionConfirmBuyNowResponseXml = string.Empty;
    
    public bool IsSuccess { get; private set; }

    public bool IsConfirmBuyNowValid { get; private set; }

    public string ErrorMessage { get; private set; }

    public AuctionConfirmBuyNowResponseData(string auctionXml)
    {
      if (!string.IsNullOrEmpty(auctionXml))
      {
        _auctionConfirmBuyNowResponseXml = auctionXml;
        IsSuccess = true;
        _atlEx = null;

        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(auctionXml);

        var item = xDoc.SelectSingleNode("/ConfirmBuyNowWithSystemIdRsp");
        if (item != null)
        {
          XmlAttributeCollection xAtr = item.Attributes;
          if (xAtr != null)
          {
            foreach (XmlAttribute xAtrNode in xAtr)
            {
              if (!string.IsNullOrEmpty(xAtrNode.Value))
              {
                switch (xAtrNode.Name)
                {
                  case "Valid":
                    IsConfirmBuyNowValid = string.Compare(xAtrNode.Value, "true", true) == 0;
                    break;
                  case "Error":
                    ErrorMessage = xAtrNode.Value;
                    break;
                }
              }
            }
          }
        }
      }
    }

    public AuctionConfirmBuyNowResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
        "AuctionConfirmBuyNowResponseData",
        ex.Message,
        oRequestData.ToXML());
    }

    
    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionConfirmBuyNowResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
