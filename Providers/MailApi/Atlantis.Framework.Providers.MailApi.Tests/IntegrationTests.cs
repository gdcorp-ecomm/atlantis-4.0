
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;

using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using Atlantis.Framework.Providers.MailApi.Interface.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Atlantis.Framework.Providers.MailApi.Tests
{
  [TestClass]
  public class IntegrationTests
  {
    private const string ANDROID_APP_KEY = "tv2YfSzBx6zdjHAjIhW9mNe5";
    private const string IOS_APP_KEY = "PAiycbPel3SL1H6tj7UxNwUU";
    private const string MOONFAIRY_EMAIL = "andy@jonathanthemoonfairy.com";
    private const string MOONFAIRY_PW = "abcd123";
    private const string BAD_BAD = "STRONGBAD";

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
    public void LoginFoldersInboxSuccessTest()
    {
      //arrange 
      var providerContainer = InitializeProviderContainer();
      var mailApiProvider = providerContainer.Resolve<IMailApiProvider>();

      //act 
      LoginFullResult result = mailApiProvider.LoginFetchFoldersAndInbox(MOONFAIRY_EMAIL, MOONFAIRY_PW, ANDROID_APP_KEY);

      //assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(result.FolderList);
      Assert.IsNotNull(result.MessageHeaderList);
     // Assert.IsNotNull(result);
    }


    [TestMethod]
    public void LoginFailureTest()
    {
      //arrange 
      var providerContainer = InitializeProviderContainer();
      var mailApiProvider = providerContainer.Resolve<IMailApiProvider>();

      //act 
      try
      {
        LoginResult result = mailApiProvider.Login(MOONFAIRY_EMAIL, BAD_BAD, ANDROID_APP_KEY);
        Assert.Fail("This should've caused an exception");
    }
      catch (MailApiException ex)
    {
        Assert.IsNotNull(ex.EmailAddress);
        Assert.IsNotNull(ex.Data);
      }


    }
  }
}
