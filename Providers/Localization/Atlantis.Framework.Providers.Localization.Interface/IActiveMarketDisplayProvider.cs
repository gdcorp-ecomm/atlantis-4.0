using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface IActiveMarketDisplayProvider
  {
    IList<IActiveMarketDisplay> GetActiveMarketDisplay();
  }
}
