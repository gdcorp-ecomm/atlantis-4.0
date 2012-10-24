using System;
using Atlantis.Framework.CDS.Entities.Widgets;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSetUpAndSettings;

namespace Atlantis.Framework.CDS.Tests.EntitiesTests
{
  [TestClass]
  public class SalesHeader4Test
  {
    /// <summary>
    ///A test for BannerInfo Constructor.
    ///</summary>
    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void ConstructorTest()
    {
      SalesHeader4 target = new SalesHeader4();
      Assert.IsNotNull(target, "SalesHeader4 is null.");
      Assert.IsInstanceOfType(target, typeof(SalesHeader4), String.Format("target is wrong type.  Should be type of {0}", typeof(SalesHeader4).Name));

    }

    [TestCategory("CDS"), Priority(0), TestAssertions(1), TestMethod]
    public void MarketingButtonConstructorTest()
    {
      MarketingButton target = new MarketingButton();
      Assert.IsNotNull(target, "target is null");
      Assert.IsInstanceOfType(target, typeof(MarketingButton), String.Format("target is wrong type.  Should be type of {0}", typeof(MarketingButton).Name));
      
    }
  }
}
