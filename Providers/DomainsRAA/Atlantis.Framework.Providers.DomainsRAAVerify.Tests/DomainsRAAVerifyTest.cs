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
  public class DomainsRAAVerifyTest
  {
    private const string SHOPPER_ID = "28018";

    [TestInitialize]
    public void InitializeTests()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      ((MockProviderContainer)ProviderContainer).SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

      ProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ProviderContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
      ProviderContainer.RegisterProvider<IDomainsRAAProvider, DomainsRAAProvider>();

      ProviderContainer.Resolve<IShopperContext>().SetLoggedInShopper(SHOPPER_ID);
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
    public void TestQueueVerifyPhoneTest()
    {
      var itemTypes = new List<IVerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      IEnumerable<DomainsRAAErrorCodes> errorCodes;
      Assert.IsTrue(RAAProvider.TryQueueVerification(verificationItems, out errorCodes));
      Assert.IsFalse(errorCodes.Any());
    }

    [TestMethod]
    public void TestQueueVerifyRegistrantPhoneTest()
    {
      const string domainId = "12776637";
      var itemTypes = new List<IVerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.REGISTRANT, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail, domainId);

      IEnumerable<DomainsRAAErrorCodes> erroCodes;
      Assert.IsTrue(RAAProvider.TryQueueVerification(verificationItems, out erroCodes));
      Assert.IsFalse(erroCodes.Any());
    }

    [TestMethod]
    public void TestQueueVerifyNoReasonCodeTest()
    {
      var itemTypes = new List<IVerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, itemTypes);

      IEnumerable<DomainsRAAErrorCodes> errorCodes;
      Assert.IsFalse(RAAProvider.TryQueueVerification(verificationItems, out errorCodes));
      Assert.IsTrue(errorCodes.Any());

      Assert.IsTrue(errorCodes.ToArray()[0] == DomainsRAAErrorCodes.Exception);
    } 
  }
}
