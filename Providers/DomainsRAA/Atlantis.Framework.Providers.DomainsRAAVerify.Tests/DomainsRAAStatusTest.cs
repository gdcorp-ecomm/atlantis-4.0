using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainsRAA;
using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;
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

      var itemType = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail);
      var requestItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, new List<IVerifyRequestItem> { itemType });

      IDomainsRAAStatus raaStatus;
      Assert.IsTrue(RAAProvider.TryGetStatus(requestItems, out raaStatus));
      Assert.IsTrue(raaStatus.HasVerifiedResponseItems);
      Assert.IsTrue(raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail).ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);
      Assert.IsFalse(raaStatus.HasErrorCodes);
    }

    [TestMethod]
    public void ShopperEmailsTest()
    {
      const string verifiedEmail = "aorellana@godaddy.com";
      const string notVerifiedEmail = "notverfiedemail@godaddy.com";

      var verfiedRequestItem = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail);
      var notVerifiedRequestItem = VerifyRequestItem.Create(ItemTypes.EMAIL, notVerifiedEmail);

      var items = new List<IVerifyRequestItem>
      {
        verfiedRequestItem, 
        notVerifiedRequestItem
      };

      var requestItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, items);

      IDomainsRAAStatus raaStatus;
      Assert.IsTrue(RAAProvider.TryGetStatus(requestItems, out raaStatus));
      Assert.IsTrue(raaStatus.HasVerifiedResponseItems);

      var verifiedItem = raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail);
      Assert.IsTrue(verifiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);
      Assert.IsTrue(verifiedItem.ItemTypeValue == verifiedEmail);

      var notVerifiedItem = raaStatus.VerifiedItems.FirstOrDefault(vi => vi.ItemTypeValue == notVerifiedEmail);
      Assert.IsTrue(notVerifiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactIsNotVerified);
      
      
      Assert.IsFalse(raaStatus.HasErrorCodes);
    }
  }
}
