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
  public class DomainsRAASetVerifiedTest
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
    public void SetVerifiedTest()
    {
      var itemTypes = new List<IItem>(1);

      var itemType = Item.Create(ItemTypes.TOKEN, "cd7163e1f6c332ddd10cda01aa1ee6cb29e7dc45cf14bdb0b189c38cfdfd32422daed747");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItems.Create(RegistrationTypes.SHOPPER, itemTypes, string.Empty, "1.1.1.1");

      var verification = Verification.Create(ReasonCodes.VerifiedByFOSEmail, verificationItems);

      IEnumerable<Errors> errorCodes;
      Assert.IsTrue(RAAProvider.TrySetVerifiedToken(verification, out errorCodes));
      Assert.IsFalse(errorCodes.Any());
    }

    [TestMethod]
    public void BadItemTypeTest()
    {
      var itemTypes = new List<IItem>(1);

      var itemType = Item.Create(ItemTypes.PHONE, "cd7163e1f6c332ddd10cda01aa1ee6cb29e7dc45cf14bdb0b189c38cfdfd32422daed747");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItems.Create(RegistrationTypes.SHOPPER, itemTypes, string.Empty, "1.1.1.1");

      var verification = Verification.Create(ReasonCodes.VerifiedByFOSEmail, verificationItems);

      IEnumerable<Errors> errorCodes;
      Assert.IsFalse(RAAProvider.TrySetVerifiedToken(verification, out errorCodes));
      Assert.IsTrue(errorCodes.Any());

      Assert.IsTrue(errorCodes.ToArray()[0] == Errors.InvalidOrMissingItemType);
    } 
  }
}
