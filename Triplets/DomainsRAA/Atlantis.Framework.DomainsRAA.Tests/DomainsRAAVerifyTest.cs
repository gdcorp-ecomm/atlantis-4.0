using System.Linq;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAVerify;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Atlantis.Framework.DomainsRAAVerify.Tests
{
  [TestClass]
  public class DomainsRAAVerifyTest
  {
    private const int REQUEST_ID = 765;
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
    public void ShopperPhoneTest()
    {
      var itemTypes = new List<VerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail, string.Empty);

      var request = new DomainsRAAVerifyRequestData(SHOPPER_ID, verificationItems);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAVerifyResponseData;

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ShopperEmailTest()
    {
      const string shopperId = "28018";
      var itemTypes = new List<VerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.EMAIL, "aorellana@godaddy.com");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail, string.Empty);

      var request = new DomainsRAAVerifyRequestData(shopperId, verificationItems);

      var response = (DomainsRAAVerifyResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }

    
    [TestMethod]
    public void RegistrantPhoneTest()
    {
      const string shopperId = "28018";
      const string domainId = "12776637";
      var itemTypes = new List<VerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.REGISTRANT, RequestIp, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail, domainId);

      var request = new DomainsRAAVerifyRequestData(shopperId, verificationItems);

      var response = (DomainsRAAVerifyResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void NoReasonCodeTest()
    {
      var itemTypes = new List<VerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, itemTypes);

      var request = new DomainsRAAVerifyRequestData(SHOPPER_ID, verificationItems);

      var response = (DomainsRAAVerifyResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.HasErrorCodes);
      Assert.IsTrue(response.ErrorCodes.ToArray()[0] == DomainsRAAErrorCodes.Exception);

      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void RegistrantPhoneInvalidDomainIdTest()
    {
      const string shopperId = "28018";
      const string invalidDomainId = "123456789";
      var itemTypes = new List<VerifyRequestItem>(1);

      var itemType = VerifyRequestItem.Create(ItemTypes.PHONE, "+1.3192943900");
      itemTypes.Add(itemType);

      var verificationItems = VerifyRequestItems.Create(RegistrationTypes.REGISTRANT, RequestIp, itemTypes, DomainsRAAReasonCodes.VerifiedByFOSEmail, invalidDomainId);

      var request = new DomainsRAAVerifyRequestData(shopperId, verificationItems);

      var response = (DomainsRAAVerifyResponseData)Engine.Engine.ProcessRequest(request, REQUEST_ID);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
