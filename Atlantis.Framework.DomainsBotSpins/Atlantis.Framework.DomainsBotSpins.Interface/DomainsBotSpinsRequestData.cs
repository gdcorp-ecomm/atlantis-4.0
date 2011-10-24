using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotSpins.Interface
{
  public class DomainsBotSpinsRequestData : RequestData
  {
    #region Properties 

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

    private int _maxResults = 10;
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

    #endregion

    /// <summary>
    /// DomainsBotSpins Request Object
    /// </summary>
    /// <param name="searchKey">key to search on</param>    
    /// <param name="tlds">List of string tlds</param>
    /// <param name="maxResults">max results to return</param>
    /// <param name="removeKeys"></param>
    /// <param name="filters"></param>
    /// <param name="targets">Comma delimited list of targets with weight percentages "synonyms=0.4,related=0.2,dash=0.4,typo=0.2"</param>
    /// <param name="supportedLanguages">"EN, DE, ES, IT, PT, FR" or leave blank to have service determine appropriate language</param>
    /// <param name="requestTimeout">timespan request timeout</param>
    public DomainsBotSpinsRequestData(string searchKey
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
                                      , string sPathway
                                      , int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      SearchKey = searchKey;
      TLDs = tlds;
      MaxResults = maxResults;
      RemoveKeys = removeKeys;
      Filters = filters;
      Targets = targets;
      SupportedLang = supportedLanguages;
      RequestTimeout = TimeSpan.FromMilliseconds(3000); 
    }

    /// <summary>
    /// DomainsBotSpins Request Object
    /// </summary>
    /// <param name="searchKey">key to search on</param>    
    /// <param name="tlds">List of string tlds</param>
    /// <param name="maxResults">max results to return</param>
    /// <param name="filters"></param>
    /// <param name="targets">Comma delimited list of targets with weight percentages "synonyms=0.4,related=0.2,dash=0.4,typo=0.2"</param>
    public DomainsBotSpinsRequestData(string searchKey
                                      , List<string> tlds
                                      , int maxResults
                                      , string filters
                                      , string targets
                                      , string sShopperID
                                      , string sSourceURL
                                      , string sOrderID
                                      , string sPathway
                                      , int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(3000); 
      SearchKey = searchKey;
      TLDs = tlds;
      MaxResults = maxResults;
      Filters = filters;
      Targets = targets;
    }


    

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DomainsBot is not a cacheable request.");
    }

    #endregion

  }
}
