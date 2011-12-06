using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotDatabase.Interface
{
  public class DomainsBotDatabaseRequestData : RequestData
  {
    #region Properties

    private string _databaseToUse = String.Empty;
    public string DatabaseToUse
    {
      get { return _databaseToUse; }
      set { _databaseToUse = value; }
    }

    private string _domainNameToSearch = String.Empty;
    public string DomainNameToSearch
    {
      get { return _domainNameToSearch; }
      set { _domainNameToSearch = value; }
    }

    private List<string> _dotTypesToSearch = new List<string>();
    public List<string> DotTypesToSearch
    {
      get { return _dotTypesToSearch; }
      set { _dotTypesToSearch = value; }
    }

    private bool _removeKeys = false;
    public bool RemoveKeys
    {
      get { return _removeKeys; }
      set { _removeKeys = value; }
    }

    private int _maxResults = 10;
    public int MaxResults
    {
      get { return _maxResults; }
      set { _maxResults = value; }
    }
    #endregion

    #region Constructors

    public DomainsBotDatabaseRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
    }

    public DomainsBotDatabaseRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         string domainNameToSearch,
                                         IEnumerable<string> dotTypesToSearch,
                                         int maxResults)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      this._domainNameToSearch = domainNameToSearch;
      AddDotTypes(dotTypesToSearch);
      this._maxResults = maxResults;
    }

    public DomainsBotDatabaseRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         string domainNameToSearch,
                                         string dotTypeToSearch,
                                         int maxResults)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      this._domainNameToSearch = domainNameToSearch;
      AddDotType(dotTypeToSearch);
      this._maxResults = maxResults;
    }

    #endregion

    #region Public Methods

    public void AddDotType(string dotType)
    {
      if (!String.IsNullOrEmpty(dotType))
      {
        this._dotTypesToSearch.Add(dotType.Trim().ToLowerInvariant());
      }
    }

    public void AddDotTypes(IEnumerable<string> dotTypes)
    {
      if (dotTypes != null)
      {
        foreach (string dotType in dotTypes)
        {
          AddDotType(dotType);
        }
      }
    }

    public string GetDotTypesString()
    {
      string result = String.Empty;
      int itemsCount = this._dotTypesToSearch.Count;
      if (itemsCount > 0)
      {
        int lastItem = itemsCount > 0 ? itemsCount - 1 : 0;
        for (int i = 0; i < itemsCount; i++)
        {
          result += this._dotTypesToSearch[i];
          if (i < lastItem)
          {
            result += ",";
          }
        }
      }
      return result;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ShopperID", this.ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", this.SourceURL);
      xtwRequest.WriteAttributeString("OrderID", this.OrderID);
      xtwRequest.WriteAttributeString("Pathway", this.Pathway);
      xtwRequest.WriteAttributeString("PageCount", this.PageCount.ToString());
      xtwRequest.WriteAttributeString("databaseToUse", this._databaseToUse);
      xtwRequest.WriteAttributeString("domainNameToSearch", this._domainNameToSearch);
      xtwRequest.WriteAttributeString("dotTypesToSearch", GetDotTypesString());
      xtwRequest.WriteAttributeString("removeKeys", this._removeKeys.ToString());
      xtwRequest.WriteAttributeString("maxResults", this._maxResults.ToString());
      xtwRequest.WriteAttributeString("timeout", RequestTimeout.TotalMilliseconds.ToString());
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();

    }

    #endregion

    #region Private Methods
    #endregion
  }
}
