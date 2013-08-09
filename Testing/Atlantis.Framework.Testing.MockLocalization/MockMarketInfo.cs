using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Testing.MockLocalization
{
  public class MockMarketInfo : IMarket
  {
    public MockMarketInfo(string id, string description, string msCulture, bool internalOnly)
    {
      Id = id;
      Description = description;
      MsCulture = msCulture;
      IsInternalOnly = internalOnly;
    }

    #region IMarket Members

    public string Id { get; private set; }

    public string Description { get; private set; }

    public string MsCulture { get; private set; }

    public bool IsInternalOnly { get; private set; }

    #endregion
  }
}
