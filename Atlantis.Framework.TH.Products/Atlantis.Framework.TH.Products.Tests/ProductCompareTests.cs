using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.Interface.Preferences;

namespace Atlantis.Framework.TH.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ProductOffer.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  class ProductCompareTests
  {
  }
}
