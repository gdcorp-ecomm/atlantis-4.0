using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasketModifyDomainContacts.Interface
{
  public class BasketModifyDomainContactsRequestData : RequestData
  {
    string _basketType = "gdshop";
    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }

    private string _contactXml = string.Empty;
    public  string ContactXml
    {
      get { return _contactXml; }
    }

    private IEnumerable<string> _domainNames;
    public IEnumerable<string> DomainNames
    {
      get { return _domainNames; }
    }

    public BasketModifyDomainContactsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string contactXml, IEnumerable<string> domainNames)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _contactXml = contactXml;
      _domainNames = domainNames;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in BasketModifyDomainContactsRequestData");
    }
  }
}
