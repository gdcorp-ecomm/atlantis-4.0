using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Testing.MockLocalization
{
  public class MockCountrySiteInfo : ICountrySite
  {
    public MockCountrySiteInfo(string id, string description, int priceGroupId, bool isInternalOnly,
                       string defaultCurrencyType, string defaultMarketId)
    {
      Id = id;
      Description = description;
      PriceGroupId = priceGroupId;
      IsInternalOnly = isInternalOnly;
      DefaultCurrencyType = defaultCurrencyType;
      DefaultMarketId = defaultMarketId;
    }

    #region ICountrySite Members

    public string Id { get; private set; }

    public string Description { get; private set; }

    public int PriceGroupId { get; private set; }

    public bool IsInternalOnly { get; private set; }

    public string DefaultCurrencyType { get; private set; }

    public string DefaultMarketId { get; private set; }

    #endregion
  }
}
