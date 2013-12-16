using System.Collections.Generic;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface
{
  public interface IDomainsRAAProvider
  {
    bool TryQueueVerification(IVerifyRequestItems verficationItems, out IEnumerable<DomainsRAAErrorCodes> errorCodes);

    bool TryGetStatus(IVerifyRequestItems requestItems, out IDomainsRAAStatus raaStatus);
  }


}
