using System;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegGetDotTypeProductIdList.Interface
{
  public class RegGetDotTypeProductIdListResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _responseXML = string.Empty;

    private bool _isValid = false;
    public bool IsValid
    {
      get { return _isValid; }
    }

    #endregion

    #region Constructors

    public RegGetDotTypeProductIdListResponseData(string responseXML)
    {
      this._responseXML = responseXML;
      this._isValid = IsValidResponse(this._responseXML);
    }

    public RegGetDotTypeProductIdListResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public RegGetDotTypeProductIdListResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "RegGetDotTypeProductIdListResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #endregion Constructors

    #region Public Methods

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return this._responseXML;
    }

    #endregion Public Methods

    #region Private Methods

    private bool IsValidResponse(string responseXml)
    {
      bool isValid = false;
      string currentElementName = null;
      const string responseNodeName = "response";
      const string processingAttrName = "processing";

      if (!string.IsNullOrEmpty(responseXml))
      {
        using (XmlReader reader = XmlReader.Create(new StringReader(responseXml)))
        {
          while (reader.Read())
          {
            switch (reader.NodeType)
            {
              case XmlNodeType.Element:
                currentElementName = reader.Name;
                if (currentElementName.Equals(responseNodeName, StringComparison.InvariantCultureIgnoreCase))
                {
                  if (reader.MoveToAttribute(processingAttrName))
                  {
                    isValid = reader.Value.Equals("success", StringComparison.InvariantCultureIgnoreCase);
                  }
                }
                break;
              default:
                reader.Skip();
                break;
            }
          }
        }
      }

      return isValid;
    }

    #endregion Private Methods
  }
}
