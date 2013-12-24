using System.Collections.Generic;
using Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface
{
  public interface IDomainsRAAProvider
  {
    bool TryQueueVerification(IVerification verfication, out IEnumerable<Errors> errorCodes);

    bool TryResend(IVerification verfication, out IEnumerable<Errors> errorCodes);

    bool TryGetStatus(IList<IItem> requestItems, out IDomainsRAAStatus raaStatus);

    bool TrySetVerifiedToken(IVerification verification, out IEnumerable<Errors> errorCodes);
  }
}
