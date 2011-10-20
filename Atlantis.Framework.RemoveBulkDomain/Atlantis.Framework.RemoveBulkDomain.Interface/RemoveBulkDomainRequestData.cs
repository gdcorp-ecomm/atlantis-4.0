using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RemoveBulkdomain.Interface
{
  public class RemoveBulkDomainRequestData : RequestData
  {
    HashSet<string> _secondLevelDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    HashSet<string> _domains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    int _rowID = -1;//Invalid rowID

    public RemoveBulkDomainRequestData(string shopperID,
                             string sourceURL,
                             string orderID,
                             string pathway,
                             int pageCount,
                             IEnumerable<string> domainNames,
                             int rowID)
                            : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _rowID = rowID;
      AddDomainName(domainNames);
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    public RemoveBulkDomainRequestData(string shopperID,
                             string sourceURL,
                             string orderID,
                             string pathway,
                             int pageCount,
                             string domainName,
                             int rowID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _rowID = rowID;
      AddDomainName(domainName);
    }

    public RemoveBulkDomainRequestData(string shopperID,
                             string sourceURL,
                             string orderID,
                             string pathway,
                             int pageCount,
                             string sld,
                             string tld,
                             int rowID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _rowID = rowID;
      _domains.Add(sld + "." + tld);
    }

    public RemoveBulkDomainRequestData(string shopperID,
                             string sourceURL,
                             string orderID,
                             string pathway,
                             int pageCount,
                             HashSet<string> fullDomainNames,
                             int rowID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _rowID = rowID;
      foreach (string domain in fullDomainNames)
      {
        _domains.Add(domain);
      }
    }

    public void AddDomainName(string domainName)
    {
      if (domainName.Contains("."))
      {
        _domains.Add(domainName);
      }
      else if (!string.IsNullOrEmpty(domainName))
      {
        _secondLevelDomains.Add(domainName);
      }
    }

    public void AddDomainName(string sld, string tld)
    {
      _domains.Add(sld + "." + tld);
    }

    public void AddDomainName(IEnumerable<string> domainNames)
    {
      foreach (string currentDomain in domainNames)
      {
        AddDomainName(currentDomain);
      }
    }

    public string DomainList()
    {
      if (_secondLevelDomains.Count != 0)
      {
        string[] domainNames = new string[_secondLevelDomains.Count];
        _secondLevelDomains.CopyTo(domainNames);
        return String.Join(",", domainNames);
      }
      else
      {
        return string.Empty;
      }
    }

    public string FullDomainList()
    {
      if (_domains.Count != 0)
      {
        string[] domainNames = new string[_domains.Count];
        _domains.CopyTo(domainNames);
        return String.Join(",", domainNames);
      }
      else
      {
        return string.Empty;
      }
    }

    public int RowID
    {
      get { return _rowID; }
      set { _rowID = value; }
    }
    
    public override string GetCacheMD5()
    {
      throw new Exception("RemoveBulkDomain is not a cacheable request.");
    }
  }
}
