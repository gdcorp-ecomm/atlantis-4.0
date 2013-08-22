using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atlantis.Framework.Interface;
using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Providers.DomainLookup.Interface;

namespace Atlantis.Framework.Providers.DomainLookup
{
    public class DomainLookupProvider : ProviderBase, IDomainLookupProvider
    {
        #region Properties

        private Lazy<IDomainLookupResponse> domainInformation;

        #endregion

        public DomainLookupProvider(IProviderContainer container)
            : base(container)
        {
        }

        public IDomainLookupResponse GetDomainInformation(string domainname)
        {
            if (ReferenceEquals(null, domainInformation))
            {
                domainInformation = new Lazy<IDomainLookupResponse>(() =>
                {
                    IDomainLookupResponse returnValue = null;

                    DomainLookupRequestData domainLookupRequestData = new DomainLookupRequestData(domainname);
                    DomainLookupResponseData domainLookupResponse = (DomainLookupResponseData)Engine.Engine.ProcessRequest(domainLookupRequestData, DomainLookupRequests.DomainLookupRequestType);

                    if (domainLookupResponse.AtlantisEx == null)
                    {
                        returnValue = domainLookupResponse.domainData;
                    }

                    return returnValue;
                });
            }

            return domainInformation.Value;
        }
    }
}
