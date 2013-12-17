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
  public class DomainsRAAQueueVerifyTest
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
      var itemTypes = new List<IItem>(1);

      var itemType = Item.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItems.Create(RegistrationTypes.SHOPPER, itemTypes);

      var verification = Verification.Create(ReasonCodes.VerifiedByFOSEmail, verificationItems);

      IEnumerable<Errors> errorCodes;
      Assert.IsTrue(RAAProvider.TryQueueVerification(verification, out errorCodes));
      Assert.IsFalse(errorCodes.Any());
    }

    [TestMethod]
    public void TestQueueVerifyRegistrantPhoneTest()
    {
      const string domainId = "12776637";
      var itemTypes = new List<IItem>(1);

      var itemType = Item.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItems.Create(RegistrationTypes.REGISTRANT, itemTypes, domainId);

      var verification = Verification.Create(ReasonCodes.VerifiedByFOSEmail, verificationItems);

      IEnumerable<Errors> erroCodes;
      Assert.IsTrue(RAAProvider.TryQueueVerification(verification, out erroCodes));
      Assert.IsFalse(erroCodes.Any());
    }
  }
}
