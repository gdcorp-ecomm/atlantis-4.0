using System;
using System.Collections.Generic;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.NameMatch.Interface
{
  public class NameMatchRequestData : RequestData
  {
    #region Properties

    private bool _addDashes;
    public bool AddDashes
    {
      get
      {
        return _addDashes;
      }
      set
      {
        _addDashes = value;
      }
    }

    private bool _addRelated;
    public bool AddRelated
    {
      get
      {
        return _addRelated;
      }
      set
      {
        _addRelated = value;
      }
    }

    private bool _availableDomainsOnly;
    public bool AvailableDomainsOnly
    {
      get
      {
        return _availableDomainsOnly;
      }
      set
      {
        _availableDomainsOnly = value;
      }
    }

    private string _customerIp;
    public string CustomerIp
    {
      get
      {
        return _customerIp;
      }
      set
      {
        _customerIp = value;
      }
    }

    private string _filters = string.Empty;
    public string Filters
    {
      get
      {
        return _filters;
      }
      set
      {
        _filters = value;
      }
    }

    private int _maxResults = 10; //limit
    public int MaxResults
    {
      get
      {
        return _maxResults;
      }
      set
      {
        _maxResults = value;
      }
    }

    private string _privateLabelId;
    public string PrivateLabelId
    {
      get
      {
        return _privateLabelId;
      }
      set
      {
        _privateLabelId = value;
      }
    }

    private string _prefixes = string.Empty;
    public string Prefixes
    {
      get
      {
        return _prefixes;
      }
      set
      {
        _prefixes = value;
      }
    }

    private bool _removeKeys;
    public bool RemoveKeys
    {

      get
      {
        return _removeKeys;
      }
      set
      {
        _removeKeys = value;
      }
    }

    public string RequestingServer
    {
      get
      {
        return Dns.GetHostName();
      }
    }

    private string _searchKey = string.Empty;
    public string SearchKey
    {
      get
      {
        return _searchKey;
      }
      set
      {
        _searchKey = value;
      }
    }

    private string _shopperAuth = string.Empty; //auth | partialauth | unknown
    public string ShopperAuth
    {
      get
      {
        return _shopperAuth;
      }
      set
      {
        _shopperAuth = value;
      }
    }

    private string _sourceCode; //DPP_blah blha.. make it up
    public string SourceCode
    {
      get
      {
        return _sourceCode;
      }
      set
      {
        _sourceCode = value;
      }
    }

    private string _suffixes = string.Empty;
    public string Suffixes
    {
      get
      {
        return _suffixes;
      }
      set
      {
        _suffixes = value;
      }
    }

    private string _supportedLanguages = null;
    public string SupportedLang
    {
      get
      {
        return _supportedLanguages;
      }
      set
      {
        _supportedLanguages = value;
      }
    }

    private string _targets = string.Empty;
    public string Targets
    {
      get
      {
        return _targets;
      }
      set
      {
        _targets = value;
      }
    }

    private List<string> _tlds = new List<string>();
    public List<string> TLDs
    {
      get
      {
        return _tlds;
      }
      set
      {
        _tlds = value;
      }
    }

    private string _version = "0.9";
    public string ServiceVersion
    {
      get
      {
        return _version;
      }
      set
      {
        _version = value;
      }
    }

    #endregion

    #region Request Object constructors

    /// <summary>
    /// NameMatch Request Object
    /// </summary>
    /// <param name="searchKey">key to search on</param>    
    /// <param name="tlds">List of string tlds</param>
    /// <param name="maxResults">max results to return</param>
    /// <param name="removeKeys"></param>
    /// <param name="filters"></param>
    /// <param name="targets">Comma delimited list of targets with weight percentages "synonyms=0.4,related=0.2,dash=0.4,typo=0.2"</param>
    /// <param name="supportedLanguages">"EN, DE, ES, IT, PT, FR" or leave blank to have service determine appropriate language</param>
    /// <param name="requestTimeout">timespan request timeout</param>
    public NameMatchRequestData(string searchKey
                                      , List<string> tlds
                                      , int maxResults
                                      , bool removeKeys
                                      , string filters
                                      , string targets
                                      , string supportedLanguages
                                      , TimeSpan requestTimeout
                                      , string sShopperID
                                      , string sSourceURL
                                      , string sOrderID
                                      , string sPathway //visitng id
                                      , int pageCount
                                      , bool addDashes
                                      , bool addRelated
                                      , string prefixes
                                      , string suffixes
                                      , string shopperAuth
                                      , bool availableDomainsOnly
                                      , string customerIp
                                      , string privateLableId
                                      , string sourceCode
                                      , string version)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, pageCount)
    {
      SearchKey = searchKey;
      TLDs = tlds;
      MaxResults = maxResults;
      RemoveKeys = removeKeys;
      Filters = filters;
      Targets = targets;
      SupportedLang = supportedLanguages;
      RequestTimeout = requestTimeout;
      AddDashes = addDashes;
      AddRelated = AddRelated;
      Prefixes = prefixes;
      Suffixes = suffixes;
      ShopperAuth = shopperAuth;
      AvailableDomainsOnly = availableDomainsOnly;
      ServiceVersion = version;
      CustomerIp = customerIp;
      PrivateLabelId = privateLableId;
      SourceCode = sourceCode;
    }

    /// <summary>
    /// NameMatch Request Object
    /// </summary>
    /// <param name="searchKey">key to search on</param>    
    /// <param name="tlds">List of string tlds</param>
    /// <param name="maxResults">max results to return</param>
    /// <param name="filters"></param>
    /// <param name="targets">Comma delimited list of targets with weight percentages "synonyms=0.4,related=0.2,dash=0.4,typo=0.2"</param>
    public NameMatchRequestData(string searchKey
                                      , List<string> tlds
                                      , int maxResults
                                      , string filters
                                      , string targets
                                      , string sShopperID
                                      , string sSourceURL
                                      , string sOrderID
                                      , string sPathway
                                      , int iPageCount
                                      , bool addDashes
                                      , bool addRelated
                                      , string prefixes
                                      , string suffixes
                                      , string shopperAuth
                                      , bool availableDomainsOnly
                                      , string customerIp
                                      , string privateLableId
                                      , string sourceCode
                                      , string version)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      SearchKey = searchKey;
      TLDs = tlds;
      MaxResults = maxResults;
      Filters = filters;
      Targets = targets;
      AddDashes = addDashes;
      AddRelated = AddRelated;
      Prefixes = prefixes;
      Suffixes = suffixes;
      ShopperAuth = shopperAuth;
      AvailableDomainsOnly = availableDomainsOnly;
      ServiceVersion = version;
      CustomerIp = customerIp;
      PrivateLabelId = privateLableId;
      SourceCode = sourceCode;
      RequestTimeout = TimeSpan.FromMilliseconds(5000);
    }


    #endregion

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("NameMatch is not a cacheable request.");
    }

    #endregion
  }
}
