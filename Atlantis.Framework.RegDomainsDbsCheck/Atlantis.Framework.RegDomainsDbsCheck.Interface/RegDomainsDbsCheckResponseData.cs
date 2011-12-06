using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegDomainsDbsCheck.Interface
{
  public class RegDomainsDbsCheckResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _responseXML = string.Empty;

    public bool IsValid
    {
      get { return _domainsWithDbsAttributes.Count > 0; }
    }

    private Dictionary<string, DbsAttributes> _domainsWithDbsAttributes = new Dictionary<string, DbsAttributes>();
    public Dictionary<string, DbsAttributes> DomainsWithDbsAttributes
    {
      get { return _domainsWithDbsAttributes; }
    }

    #endregion Properties

    #region Constructors

    public RegDomainsDbsCheckResponseData(string responseXML)
    {
      ParseResponseXml(responseXML);
    }

    public RegDomainsDbsCheckResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public RegDomainsDbsCheckResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "RegDomainsDbsCheckResponseData",
        exception.Message, requestData.ToXML());
    }

    #endregion Constructors

    #region Public Methods

    public string ToXML()
    {
      return this._responseXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion Public Methods

    #region Private Methods

    private void ParseResponseXml(string responseXml)
    {
      string currentElementName = null;
      const string nodeName = "domain";
      const string isDbsCapableAttrib = "isDbsCapable";
      const string auctionActiveAttrib = "auctionActive";
      const string auctionLinkAttrib = "auctionLink";

      if (!string.IsNullOrEmpty(responseXml))
      {
        this._responseXML = responseXml;

        using (XmlReader reader = XmlReader.Create(new StringReader(responseXml)))
        {
          while (reader.Read())
          {
            switch (reader.NodeType)
            {
              case XmlNodeType.Element:
                currentElementName = reader.Name;

                if (currentElementName.Equals(nodeName, StringComparison.InvariantCultureIgnoreCase))
                {
                  bool isDbsCapable = false;
                  bool auctionActive = false;
                  string auctionLink = string.Empty;

                  if (reader.MoveToAttribute(isDbsCapableAttrib))
                  {
                    isDbsCapable = reader.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                  }

                  if (reader.MoveToAttribute(auctionActiveAttrib))
                  {
                    auctionActive = reader.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                  }

                  if (reader.MoveToAttribute(auctionLinkAttrib))
                  {
                    auctionLink = reader.Value;
                  }

                  reader.MoveToContent();
                  this.DomainsWithDbsAttributes[reader.ReadString()]
                    = new DbsAttributes(isDbsCapable, auctionActive, auctionLink);
                }
                break;
              default:
                reader.Skip();
                break;
            }
          }
        }
      }
    }

    #endregion Private Methods
  }
}
