using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotDatabase.Interface
{
  public class DomainsBotDatabaseResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;

    private Dictionary<string, DatabaseDomainAttributes> _domainsWithAttributes = new Dictionary<string, DatabaseDomainAttributes>();
    public Dictionary<string, DatabaseDomainAttributes> DomainsWithAttributes
    {
      get
      {
        return _domainsWithAttributes;
      }
    }

    private int _availableResultsCount = 0;
    public int AvailableResultsCount
    {
      get { return _availableResultsCount; }
    }

    public bool NoErrors
    {
      get { return _exception == null; }
    }

    #endregion

    #region Constructors

    public DomainsBotDatabaseResponseData(int availableResultsCount)
    {
      this._availableResultsCount = availableResultsCount;
    }

    public DomainsBotDatabaseResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public DomainsBotDatabaseResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "DomainsBotDatabaseResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #endregion

    #region Public Methods

    public void AddDomain(string domain, DatabaseDomainAttributes databaseDomainAttributes)
    {
      if (this._domainsWithAttributes.ContainsKey(domain))
      {
        this._domainsWithAttributes[domain] = databaseDomainAttributes;
      }
      else
      {
        this._domainsWithAttributes.Add(domain, databaseDomainAttributes);
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("response");
      foreach (KeyValuePair<string, DatabaseDomainAttributes> domain in this._domainsWithAttributes)
      {
        xtwRequest.WriteStartElement("premiumDomain");
        xtwRequest.WriteAttributeString("price", domain.Value.Price.ToString());
        xtwRequest.WriteAttributeString("commission", domain.Value.Commission.ToString());
        xtwRequest.WriteAttributeString("auctionendtime", domain.Value.AuctionEndTime.ToString());
        xtwRequest.WriteValue(domain.Key);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion
  }
}
