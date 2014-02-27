using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.MailApi.Tests
{
  [TestClass]
  public class IntegrationTests
  {
    private const string ANDROID_APP_KEY = "tv2YfSzBx6zdjHAjIhW9mNe5";
    private const string IOS_APP_KEY = "PAiycbPel3SL1H6tj7UxNwUU";

    private IProviderContainer InitializeProviderContainer()
    {
      var request = new MockHttpRequest("http://www.godaddy.com/");

      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IMailApiProvider, MailApiProvider>();

      return container;
    }

    [TestMethod]
    public void LoginSuccessTest()
    {
      //arrange 
      var providerContainer = InitializeProviderContainer();
      var mailApiProvider = providerContainer.Resolve<IMailApiProvider>();

      //act 
      CompositeLoginResponse result = mailApiProvider.LoginAndFetchFoldersAndInbox("andy@jonathanthemoonfairy.com", "abcd123", ANDROID_APP_KEY);

      //assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(result.GetMessageListData);
    }

    [TestMethod]
    public void LoginFailureTest()
    {
      //arrange 



      //act 



      //assert


    }
  }
}
