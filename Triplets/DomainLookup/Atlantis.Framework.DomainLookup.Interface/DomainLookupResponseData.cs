using Atlantis.Framework.Interface;
using System;
using System.Data;
using System.Xml.Linq;

using Atlantis.Framework.Providers.DomainLookup.Interface;

namespace Atlantis.Framework.DomainLookup.Interface
{
  public class DomainLookupResponseData : IResponseData
  {
    public static DomainLookupResponseData FromData(AtlantisException atlantisException)
    {
      return new DomainLookupResponseData(atlantisException);
    }

    public static DomainLookupResponseData FromData(DataSet data)
    {
      return new DomainLookupResponseData(data);
    }

    private DomainLookupResponseData(AtlantisException atlantisException)
    {
      _atlantisEx = atlantisException;
    }

    private DomainLookupResponseData(DataSet data)
    {
        parseDomainLookupResponse(data);      
    }

    private void parseDomainLookupResponse(DataSet ds)
    {
        domainData = new DomainLookupResponse(ds) as IDomainLookupResponse;
    }

    public IDomainLookupResponse domainData { get; private set; }

    private readonly AtlantisException _atlantisEx = null;
    public AtlantisException AtlantisEx { get { return this._atlantisEx; } }
    
    public string ToXML()
    {
      //TODO: Serialize the object to XML? -or- come up with XML writer to output XML?
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return this._atlantisEx;
    }
  }
}
