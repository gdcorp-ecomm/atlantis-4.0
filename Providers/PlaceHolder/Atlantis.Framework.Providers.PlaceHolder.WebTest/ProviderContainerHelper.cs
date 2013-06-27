using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public static class ProviderContainerHelper
  {
    private static IProviderContainer _instance;
    public static IProviderContainer Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new MockProviderContainer();
          _instance.RegisterProvider<ISiteContext, MockSiteContext>();
          _instance.RegisterProvider<IShopperContext, MockShopperContext>();
          _instance.RegisterProvider<IManagerContext, MockManagerContext>();
          _instance.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
          _instance.RegisterProvider<ICDSContentProvider, MockCDSProvider>();
        }
        return _instance;
      }
    }
  }

  public class MockCDSProvider : ProviderBase, ICDSContentProvider
  {
      private class CDSContext : IRenderContent
      {
          string _app { get; set; }
          string _path { get; set; }

          public CDSContext(string app, string path)
          {
              _app = app;
              _path = path;
          }

          public string Content
          {
              get { return string.Format("This is for a call to the CDS with the location of {0}/{1}", _app, _path); }
          }
      }

      public MockCDSProvider(IProviderContainer container)
          : base(container)
      {
      }

      public IWhitelistResult CheckWhiteList(string appName, string relativePath)
      {
          throw new System.NotImplementedException();
      }

      public IRedirectResult CheckRedirectRules(string appName, string relativePath)
      {
          throw new System.NotImplementedException();
      }

      public Render.Pipeline.Interface.IRenderContent GetContent(string appName, string relativePath)
      {
          return new CDSContext(appName, relativePath);
      }
  }
}