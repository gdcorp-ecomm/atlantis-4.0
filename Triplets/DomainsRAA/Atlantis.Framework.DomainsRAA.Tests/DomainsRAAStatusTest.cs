using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsRAA.Tests
{
  [TestClass]
  public class DomainsRAAStatusTests
  {
    private const int REQUEST_ID = 767;
    private const string SHOPPER_ID = "28018";

    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);
      
      ((MockProviderContainer) ProviderContainer).SetData(MockSiteContextSettings.IsRequestInternal, true);

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
    public void ShopperEmailVerifiedTest()
    {
      const string verifiedEmail = "aorellana@godaddy.com";

      var itemType = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail);
      var itemTypes = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, new List<VerifyRequestItem> { itemType });

      var request = new DomainsRAAStatusRequestData(itemTypes);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAStatusResponseData;

      Assert.IsTrue(response.HasVerifiedResponseCodes);
      var shopperVerfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail);
      Assert.IsTrue(shopperVerfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);
    }

    [TestMethod]
    public void ShopperEmailsTest()
    {
      const string verifiedEmail = "aorellana@godaddy.com";
      const string notVerifiedEmail = "notverfiedemail@godaddy.com";

      var verfiedItem = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail);
      var notVerifiedItem = VerifyRequestItem.Create(ItemTypes.EMAIL, notVerifiedEmail);

      var items = new List<VerifyRequestItem>
      {
        verfiedItem, 
        notVerifiedItem
      };

      var itemTypes = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, items);

      var request = new DomainsRAAStatusRequestData(itemTypes);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAStatusResponseData;

      Assert.IsTrue(response.HasVerifiedResponseCodes);

      var shopperVerfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail);
      Assert.IsTrue(shopperVerfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      var shopperNotVerfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == notVerifiedEmail);
      Assert.IsTrue(shopperNotVerfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactIsNotVerified);
    }

    [TestMethod]
    public void ShopperIdPhoneEmailsDomainIdVerifiedTest()
    {
      const string verifiedEmail1 = "aorellana@godaddy.com";
      const string verifiedEmail2 = "cbaker@godaddy.com";
      const string verifiedPhone = "18002221234";
      const string veririedDomainId = "12776637";
      const string veririedShopperId = "28018";

      var verfiedEmailItem1 = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail1);
      var verfiedEmailItem2 = VerifyRequestItem.Create(ItemTypes.EMAIL, verifiedEmail2);

      var verifiedPhoneItem = VerifyRequestItem.Create(ItemTypes.PHONE, verifiedPhone);

      var verifiedDomainIdItem = VerifyRequestItem.Create(ItemTypes.DOMAIN_ID, veririedDomainId);

      var verifiedShopperIdItem = VerifyRequestItem.Create(ItemTypes.SHOPPER_ID, veririedShopperId);
      

      var itemTypes = new List<VerifyRequestItem>
      {
        verfiedEmailItem1, 
        verfiedEmailItem2,
        verifiedPhoneItem,
        verifiedDomainIdItem,
        verifiedShopperIdItem
      };

      var requestItems = VerifyRequestItems.Create(RegistrationTypes.SHOPPER, RequestIp, itemTypes);

      var request = new DomainsRAAStatusRequestData(requestItems);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAStatusResponseData;

      Assert.IsTrue(response.HasVerifiedResponseCodes);

      var verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail1);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail2);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedPhone);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactIsNotVerified);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == veririedDomainId);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.DomainRecordNotEligibleForRAA);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == veririedShopperId);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperVerified);
    }
  }
}
