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

namespace Atlantis.Framework.CH.Segmentation.Tests
{
  public class MockShopperSegmentProvider : ProviderBase, IShopperSegmentationProvider
  {
    public MockShopperSegmentProvider(IProviderContainer container)
      : base(container)
    {

    }

    public string GetShopperSegmentId()
    {
      return Container.GetData(MockShopperSegmentProviderSettings.ShopperSegment, "nacent");
    }
  }

  public class MockShopperSegmentProviderSettings
  {
    public const string ShopperSegment = "MockManagerContextSettings.ShopperSegment";
  }
}

