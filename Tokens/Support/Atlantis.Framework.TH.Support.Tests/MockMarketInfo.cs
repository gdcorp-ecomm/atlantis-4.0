using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Atlantis.Framework.TH.Support.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockMarketInfo : IMarket
  {
    public MockMarketInfo(string id, string description, bool isInternalOnly, string msCulture)
    {
      Id = id;
      Description = description;
      IsInternalOnly = isInternalOnly;
      MsCulture = msCulture;
    }

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
  }
}
