using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface
{
  public class AuctionConfirmBuyNowBulkResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionConfirmBuyNowBulkResponseXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public Dictionary<string, bool> ConfirmBuyNowValid { get; private set; }

    public Dictionary<string, ResponseErrorData> ConfirmBuyNowError { get; private set; }

    public string ErrorMessage { get; private set; }

    public int? ErrorNumber { get; private set; }

    public AuctionConfirmBuyNowBulkResponseData(string auctionXml)
    {
      if (!string.IsNullOrEmpty(auctionXml))
      {
        _auctionConfirmBuyNowBulkResponseXml = auctionXml;
        ConfirmBuyNowValid = new Dictionary<string, bool>();
        ConfirmBuyNowError = new Dictionary<string, ResponseErrorData>();
        IsSuccess = true;
        _atlEx = null;
        PopulateFromXML(auctionXml);
      }
    }

    private void PopulateFromXML(string resultXml)
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(resultXml);

      XmlElement xnBulkBuyNowResponse = (XmlElement)xDoc.SelectSingleNode("/BulkBuyNowResponse");
      if (xnBulkBuyNowResponse != null)
      {
        XmlAttributeCollection xAtr = xnBulkBuyNowResponse.Attributes;
        if (xAtr != null)
        {
          foreach (XmlAttribute xAtrNode in xAtr)
          {
            if (!string.IsNullOrEmpty(xAtrNode.Value))
            {
              switch (xAtrNode.Name.ToLower())
              {
                case "valid":
                  IsSuccess = string.Compare(xAtrNode.Value, "true", true) == 0;
                  break;
                case "error":
                  ErrorMessage = xAtrNode.Value;
                  break;
              }
            }
          }

          if (!IsSuccess)
          {
            XmlElement xnErrorData = (XmlElement)xnBulkBuyNowResponse.SelectSingleNode("ErrorData");
            if (xnErrorData != null && xnErrorData.HasAttribute("errornumber"))
            {
              int errorNumber = 0;
              int.TryParse(xnErrorData.Attributes["errornumber"].Value, out errorNumber);
              ErrorNumber = errorNumber;
            }
          }
        }
      }

      XmlNodeList xnConfirmBuyNowWithSystemIdRsp = xDoc.SelectNodes("/BulkBuyNowResponse/ConfirmBuyNowWithSystemIdRsp");
      if (xnConfirmBuyNowWithSystemIdRsp != null)
      {
        foreach (XmlElement rsp in xnConfirmBuyNowWithSystemIdRsp)
        {
          bool isValid = false;
          string message = string.Empty;
          string identifier = string.Empty;
          int errorNumber = 0;

          XmlAttributeCollection attrs = rsp.Attributes;
          if (attrs != null)
          {
            foreach (XmlAttribute xAtrNode in attrs)
            {
              if (!string.IsNullOrEmpty(xAtrNode.Value))
              {
                switch (xAtrNode.Name.ToLower())
                {
                  case "valid":
                    isValid = string.Compare(xAtrNode.Value, "true", true) == 0;
                    break;
                  case "error":
                    message = xAtrNode.Value;
                    break;
                  case "auctionid":
                    if (string.IsNullOrEmpty(identifier))
                    {
                      identifier = xAtrNode.Value;
                    }
                    break;
                  case "domainname":
                    identifier = xAtrNode.Value;
                    break;
                }
              }
            }

            if (!isValid)
            {
              XmlElement xnErrorData = (XmlElement)rsp.SelectSingleNode("ErrorData");
              if (xnErrorData != null && xnErrorData.HasAttribute("errornumber"))
              {
                int.TryParse(xnErrorData.Attributes["errornumber"].Value, out errorNumber);
              }
            }
          }

          if (!string.IsNullOrEmpty(identifier) && !ConfirmBuyNowValid.ContainsKey(identifier))
          {
            ConfirmBuyNowValid.Add(identifier, isValid);
            ConfirmBuyNowError.Add(identifier, new ResponseErrorData(errorNumber, message));
          }
        }
      }
    }

    public AuctionConfirmBuyNowBulkResponseData(RequestData requestData, Exception ex)
    {
      _atlEx = new AtlantisException(requestData, "AuctionConfirmBuyNowBulkResponseData", ex.Message, requestData.ToXML());
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionConfirmBuyNowBulkResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}