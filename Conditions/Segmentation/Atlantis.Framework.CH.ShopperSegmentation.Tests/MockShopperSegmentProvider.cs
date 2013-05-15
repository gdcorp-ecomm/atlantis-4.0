using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using System.Reflection;
using System.Linq;
using Atlantis.Framework.Providers.Segmentation.Interface;

namespace Atlantis.Framework.CH.ShopperSegment.Tests
{
  public class MockShopperSegmentProvider : ProviderBase, ISegmentationProvider
  {
    public MockShopperSegmentProvider(IProviderContainer container)
      : base(container)
    {

    }

    public System.Collections.Generic.IEnumerable<int> GetShopperSegmentIds()
    {
      return Enumerable.Range(1, 5);
    }

    public int GetShopperSegmentId()
    {
      return 6;
    }
  }
}

