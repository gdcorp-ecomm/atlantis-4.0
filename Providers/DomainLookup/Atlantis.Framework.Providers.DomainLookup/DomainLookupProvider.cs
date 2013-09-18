using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Providers.DomainLookup.Interface;

namespace Atlantis.Framework.Providers.DomainLookup
{
    public class DomainLookupProvider : ProviderBase, IDomainLookupProvider
    {
      private static readonly IDomainLookupData DefaultDomainLookupData = DomainLookupData.DefaultInstance;

      private string _domainName;
      public string DomainName
      {
        get { return _domainName; }
      }

      private IDomainLookupData _parkedDomainInfo = DefaultDomainLookupData;
      public IDomainLookupData ParkedDomainInfo
      {
        get { return _parkedDomainInfo; }
      }

      public DomainLookupProvider(IProviderContainer container) : base(container)
      {
      }

      public IDomainLookupData GetDomainInformation(string domainName)
      {
        IDomainLookupData domainLookupData;

        try
        {
          DomainLookupRequestData requestData = new DomainLookupRequestData(domainName);
          DomainLookupResponseData responseData = (DomainLookupResponseData)DataCache.DataCache.GetProcessRequest(requestData, DomainLookupRequests.DomainLookupRequestType);

          domainLookupData = responseData.domainData;
          _parkedDomainInfo = domainLookupData;
          _domainName = domainName;
        }
        catch (Exception ex)
        {
          domainLookupData = DefaultDomainLookupData;
          Engine.Engine.LogAtlantisException(new AtlantisException("DomainLookupProvider.GetDomainInformation()", 0, ex.Message, "Domain Name: " + domainName));

        }

        return domainLookupData;
      }

      public bool IsDomainExpired()
      {
        return (_parkedDomainInfo.ExpirationDate < DateTime.Now) &&
               (_parkedDomainInfo.ExpirationDate != DateTime.MinValue);
      }
    }
}
