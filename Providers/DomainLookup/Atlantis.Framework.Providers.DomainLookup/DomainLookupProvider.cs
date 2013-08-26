using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Providers.DomainLookup.Interface;

namespace Atlantis.Framework.Providers.DomainLookup
{
    public class DomainLookupProvider : ProviderBase, IDomainLookupProvider
    {
      private static readonly IDomainLookupData _defaultDomainLookupData = DomainLookupData.DefaultInstance; 

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

        }
        catch (Exception ex)
        {
          domainLookupData = _defaultDomainLookupData;
          Engine.Engine.LogAtlantisException(new AtlantisException("DomainLookupProvider.GetDomainInformation()", 0, ex.Message, "Domain Name: " + domainName));

        }

        return domainLookupData;
      }
    }
}
