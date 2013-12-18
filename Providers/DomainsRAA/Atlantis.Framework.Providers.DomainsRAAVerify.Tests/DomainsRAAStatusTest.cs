using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainsRAA;
using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DomainsRAAVerify.Tests
{
  [TestClass]
  public class DomainsRAAStatusTest
  {
    private const string SHOPPER_ID = "28018";

    [TestInitialize]
    public void InitializeTests()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      ((MockProviderContainer)ProviderContainer).SetData(MockSiteContextSettings.IsRequestInternal, true);
      ((MockProviderContainer)ProviderContainer).SetData("MockSiteContextSettings.ShopperId", SHOPPER_ID);

      ProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ProviderContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
      ProviderContainer.RegisterProvider<IDomainsRAAProvider, DomainsRAAProvider>();
    }

    private IProviderContainer _providerContainer;

    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
        }

        return _providerContainer;
      }
    }

    private IDomainsRAAProvider _raaProvider;
    private IDomainsRAAProvider RAAProvider
    {
      get
      {
        if (_raaProvider == null)
        {
          _raaProvider = ProviderContainer.Resolve<IDomainsRAAProvider>();
        }

        return _raaProvider;
      }
    }

    [TestMethod]
    public void ShopperEmailVerifiedTest()
    {
      const string verifiedEmail = "aorellana@godaddy.com";

      var itemType = Item.Create(ItemTypes.EMAIL, verifiedEmail);
      var requestItems = new List<IItem> {itemType};

      IDomainsRAAStatus raaStatus;
      Assert.IsTrue(RAAProvider.TryGetStatus(requestItems, out raaStatus));
      Assert.IsTrue(raaStatus.HasVerifiedResponseItems);
      Assert.IsTrue(raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail).ItemVerifiedCode == DomainsRAAVerifyCode.Verified);
      Assert.IsFalse(raaStatus.HasErrorCodes);
    }

    [TestMethod]
    public void ShopperEmailsTest()
    {
      const string verifiedEmail = "aorellana@godaddy.com";
      const string notVerifiedEmail = "notverfiedemail@godaddy.com";

      var verfiedRequestItem = Item.Create(ItemTypes.EMAIL, verifiedEmail);
      var notVerifiedRequestItem = Item.Create(ItemTypes.EMAIL, notVerifiedEmail);

      var items = new List<IItem>
      {
        verfiedRequestItem, 
        notVerifiedRequestItem
      };
      
      IDomainsRAAStatus raaStatus;
      Assert.IsTrue(RAAProvider.TryGetStatus(items, out raaStatus));
      Assert.IsTrue(raaStatus.HasVerifiedResponseItems);

      var verifiedItem = raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail);
      Assert.IsTrue(verifiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.Verified);
      Assert.IsTrue(verifiedItem.ItemTypeValue == verifiedEmail);

      var notVerifiedItem = raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == notVerifiedEmail);
      Assert.IsTrue(notVerifiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.NotVerified);
      
      
      Assert.IsFalse(raaStatus.HasErrorCodes);
    }
  }
}
