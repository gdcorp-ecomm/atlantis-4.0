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

      var itemType = ItemElement.Create(ItemTypes.EMAIL, verifiedEmail);
      
      var request = new DomainsRAAStatusRequestData(RequestIp, new List<ItemElement>() { itemType });

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

      var verfiedItem = ItemElement.Create(ItemTypes.EMAIL, verifiedEmail);
      var notVerifiedItem = ItemElement.Create(ItemTypes.EMAIL, notVerifiedEmail);

      var items = new List<ItemElement>
      {
        verfiedItem, 
        notVerifiedItem
      };

      var request = new DomainsRAAStatusRequestData(RequestIp, items);
      
      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAStatusResponseData;

      Assert.IsTrue(response.HasVerifiedResponseCodes);

      var shopperVerfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail);
      Assert.IsTrue(shopperVerfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      var shopperNotVerfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == notVerifiedEmail);
      Assert.IsTrue(shopperNotVerfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactIsNotVerifiedNotPending);
    }

    [TestMethod]
    public void ShopperIdPhoneEmailsDomainIdVerifiedTest()
    {
      const string verifiedEmail1 = "aorellana@godaddy.com";
      const string verifiedEmail2 = "cbaker@godaddy.com";
      const string verifiedPhone = "18002221234";
      const string veririedDomainId = "12776637";
      const string veririedShopperId = "28018";

      var verfiedEmailItem1 = ItemElement.Create(ItemTypes.EMAIL, verifiedEmail1);
      var verfiedEmailItem2 = ItemElement.Create(ItemTypes.EMAIL, verifiedEmail2);

      var verifiedPhoneItem = ItemElement.Create(ItemTypes.PHONE, verifiedPhone);

      var verifiedDomainIdItem = ItemElement.Create(ItemTypes.DOMAIN_ID, veririedDomainId);

      var verifiedShopperIdItem = ItemElement.Create(ItemTypes.SHOPPER_ID, veririedShopperId);
      

      var itemTypes = new List<ItemElement>
      {
        verfiedEmailItem1, 
        verfiedEmailItem2,
        verifiedPhoneItem,
        verifiedDomainIdItem,
        verifiedShopperIdItem
      };

      var request = new DomainsRAAStatusRequestData(RequestIp, itemTypes);

      var response = Engine.Engine.ProcessRequest(request, REQUEST_ID) as DomainsRAAStatusResponseData;

      Assert.IsTrue(response.HasVerifiedResponseCodes);

      var verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail1);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedEmail2);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactVerified);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == verifiedPhone);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperArtifactIsNotVerifiedNotPending);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == veririedDomainId);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.DomainRecordNotEligibleForRAA);

      verfiedItem = response.VerifiedResponseItems.FirstOrDefault(vi => vi.ItemTypeValue == veririedShopperId);
      Assert.IsTrue(verfiedItem.ItemVerifiedCode == DomainsRAAVerifyCode.ShopperVerified);
    }
  }
}
