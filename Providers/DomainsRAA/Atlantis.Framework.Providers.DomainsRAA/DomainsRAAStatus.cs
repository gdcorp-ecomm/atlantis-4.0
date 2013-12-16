using System.Collections.Generic;
using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;

namespace Atlantis.Framework.Providers.DomainsRAA
{
  public class DomainsRAAStatus : IDomainsRAAStatus
  {
    public bool HasVerifiedResponseItems { get; set; }
    public bool HasErrorCodes { get; set; }

    public IEnumerable<IVerifiedResponseItem> VerifiedItems { get; private set; }

    public IEnumerable<DomainsRAAErrorCodes> ErrorCodes { get; private set; }

    internal static DomainsRAAStatus Create(IList<IVerifiedResponseItem> verifiedResponseItems, IList<DomainsRAAErrorCodes> errorCodes)
    {
      var status = new DomainsRAAStatus
      {
        ErrorCodes = errorCodes,
        HasErrorCodes = errorCodes != null && errorCodes.Count > 0, 

        VerifiedItems = verifiedResponseItems, 
        HasVerifiedResponseItems = verifiedResponseItems != null && verifiedResponseItems.Count > 0
      };

      return status;
    }
  }
}
