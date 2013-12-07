using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Support.Tests
{
  public class MockMarketInfo : IMarket
  {
    public MockMarketInfo(string id, string description, bool isInternalOnly, string msCulture)
    {
      Id = id;
      Description = description;
      IsInternalOnly = isInternalOnly;
      MsCulture = msCulture;
    }

    #region IMarket Members

    public string Description
    {
      get;
      private set;
    }

    public string Id
    {
      get;
      private set;
    }

    public bool IsInternalOnly
    {
      get;
      private set;
    }

    public string MsCulture
    {
      get;
      private set;
    }

    #endregion
  }
}
