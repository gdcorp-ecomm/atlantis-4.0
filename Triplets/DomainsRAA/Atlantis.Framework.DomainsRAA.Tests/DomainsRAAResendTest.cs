using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAResend;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsRAA.Tests
{
  [TestClass]
  public class DomainsRAAResendTest
  {
    private const int REQUEST_ID = 790;
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
    public void ShopperPhoneTestResend()
    {
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);


      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.SHOPPER, itemTypes);

      var verificationItem = VerificationItemElement.Create(SHOPPER_ID, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAAResendRequestData(verificationItem);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAResendResponseData;

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ShopperEmailTestResend()
    {
      const string shopperId = "28018";
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.EMAIL, "aorellana@godaddy.com");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.SHOPPER, itemTypes);

      var verificationItem = VerificationItemElement.Create(shopperId, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAAResendRequestData(verificationItem);

      var response = (DomainsRAAResendResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void RegistrantPhoneTestResend()
    {
      const string shopperId = "28018";
      const string domainId = "12776637";
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.REGISTRANT, itemTypes, domainId);

      var verificationItem = VerificationItemElement.Create(shopperId, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAAResendRequestData(verificationItem);

      var response = (DomainsRAAResendResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void RegistrantPhoneInvalidDomainIdTest()
    {
      const string shopperId = "28018";
      const string invalidDomainId = "123456789";
      var itemTypes = new List<ItemElement>(1);

      var itemType = ItemElement.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerificationItemsElement.Create(RegistrationTypes.REGISTRANT, itemTypes, invalidDomainId);

      var verificationItem = VerificationItemElement.Create(shopperId, RequestIp, verificationItems, DomainsRAAReasonCodes.VerifiedByFOSEmail);

      var request = new DomainsRAAResendRequestData(verificationItem);

      var response = (DomainsRAAResendResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
