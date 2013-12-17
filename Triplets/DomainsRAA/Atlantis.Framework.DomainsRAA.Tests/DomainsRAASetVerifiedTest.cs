using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAASetVerified;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsRAA.Tests
{
  [TestClass]
  public class DomainsRAASetVerifiedTest
  {
    private const int REQUEST_ID = 783;
    private const string SHOPPER_ID = "28018";

    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      ((MockProviderContainer)ProviderContainer).SetData(MockSiteContextSettings.IsRequestInternal, true);
      ((MockProviderContainer)ProviderContainer).SetData("MockSiteContextSettings.ShopperId", SHOPPER_ID);

      ProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ProviderContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
      ProviderContainer.RegisterProvider<IProxyContext, WebProxyContext>();
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

    private string RequestIp
    {
      get
      {
        return HttpContext.Current.Request.UserHostAddress;
      }
    }

    [TestMethod]
    public void VerifyTokenTest()
    {
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.TOKEN, "cd7163e1f6c332ddd10cda01aa1ee6cb29e7dc45cf14bdb0b189c38cfdfd32422daed747"); // From RAA service document
      itemTypes.Add(itemType);

      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.NONE, new List<ItemElement> {itemType}, string.Empty, "1.1.1.1");

      var verificationItem = VerificationItemElement.Create(SHOPPER_ID, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAASetVerifiedRequestData(verificationItem);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAASetVerifiedResponseData;

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void VerifyItemTypeTokenTest()
    {
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.DOMAIN_ID, "cd7163e1f6c332ddd10cda01aa1ee6cb29e7dc45cf14bdb0b189c38cfdfd32422daed747"); // From RAA service document
      itemTypes.Add(itemType);

      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.NONE, new List<ItemElement> { itemType }, string.Empty, "1.1.1.1");

      var verificationItem = VerificationItemElement.Create(SHOPPER_ID, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAASetVerifiedRequestData(verificationItem);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAASetVerifiedResponseData;

      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.ErrorCodes.ToArray()[0] == DomainsRAAErrorCodes.InvalidOrMissingItemType);
    }

  }
}
