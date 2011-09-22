using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionBidCloseOuts.Interface
{
  public class AuctionBidCloseOutsResponseData : IResponseData
  {
    #region Properties

    string _responseXml = null;
    private AtlantisException _exception = null;

    private bool _isValid = false;
    public bool IsValid
    {
      get { return _isValid; }
    }

    private string _message = string.Empty;
    public string Message
    {
      get { return _message; }
    }

    #endregion

    #region Constructors

    public AuctionBidCloseOutsResponseData(string responseXml)
    {
      _responseXml = responseXml;
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(_responseXml);
      XmlElement xmlElement = xdDoc.DocumentElement;

      if (xmlElement != null)
      {
        string isValid = xmlElement.GetAttribute("IsValid");

        if (!string.IsNullOrEmpty(isValid)
          && isValid.Equals("true", StringComparison.InvariantCultureIgnoreCase))
        {
          _isValid = true;
        }
        else
        {
          _message = xmlElement.GetAttribute("Message");
        }
      }
    }

    public AuctionBidCloseOutsResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public AuctionBidCloseOutsResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "AuctionBidCloseOutsResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #endregion

    #region Public Methods

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion Public Methods
  }
}
