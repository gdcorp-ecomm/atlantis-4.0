using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface IActiveMarketDisplayProvider
  {
    /// <summary>
    /// Get list of IActiveMarketDisplay objects.
    /// </summary>
    /// <param name="includeInternalOnly">Include information for non-public mappings, markets, and countrysites</param>
    /// <returns>Returns information for all mapped markets for all countrysites</returns>
    IList<IActiveMarketDisplay> GetActiveMarketDisplay(bool includeInternalOnly);
  }
}
