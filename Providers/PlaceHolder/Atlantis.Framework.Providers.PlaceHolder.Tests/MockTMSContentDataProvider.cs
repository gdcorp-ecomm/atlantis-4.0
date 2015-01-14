using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TMSContentData;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockTMSContentDataProvider : TMSContentDataProvider
  {
    public MockTMSContentDataProvider(IProviderContainer container)
      : base(container) {}
  }
}
