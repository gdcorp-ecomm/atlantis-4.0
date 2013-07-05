using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  public class PlaceHolderProviderTests
  {
    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
          _providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
        }

        return _providerContainer;
      }
    }

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void NullPlaceHolder()
    {
      string content = @"<div>
                          [@P[doesNotExist:<Data location=""sdfdfsafsf""><Parameters><Parameter key=""Hello"" value=""World"" /></Parameters></Data>]@P]
                         </div>";


      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();
      string finalContent = placeHolderProvider.ReplacePlaceHolders(content);

      WriteOutput(finalContent);
      Assert.IsFalse(finalContent.Contains("[@P[") || finalContent.Contains("]@P]"));
    }

    [TestMethod]
    public void NoParametersPlaceHolder()
    {
      string content = @"<div>
                          [@P[userControl:<Data location=""sdfdfsafsf""></Data>]@P]
                         </div>";


      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();
      
      placeHolderProvider.ReplacePlaceHolders(content);
    }

    [TestMethod]
    public void EncodedPlaceHolder()
    {
      string content = @"<div>
                          [@P[userControl:<Data location=\""sdfdfsafsf\""><Parameters><Parameter key=\""Hello\"" value=\""World\"" /></Parameters></Data>]@P]
                         </div>";


      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();
      IPlaceHolderEncoding testPlaceHolderEncoding = new TestPlaceHolderEncoding();

      string finalContent = placeHolderProvider.ReplacePlaceHolders(content, testPlaceHolderEncoding);

      WriteOutput(finalContent);
      Assert.IsFalse(finalContent.Contains("[@P[") || finalContent.Contains("]@P]"));
    }

    [TestMethod]
    public void NoPlaceHolders()
    {
      string content = @"<div>
                          Hello World
                         </div>";


      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();
      string finalContent = placeHolderProvider.ReplacePlaceHolders(content);

      WriteOutput(finalContent);
      Assert.IsTrue(finalContent.Contains("Hello World"));
    }

    [TestMethod]
    public void NullContent()
    {
      string content = null;

      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();
      string finalContent = placeHolderProvider.ReplacePlaceHolders(content);

      WriteOutput(finalContent);
      Assert.IsTrue(finalContent.Equals(string.Empty));
    }
  }
}
