using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Personalization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
  public class PersonalizationMessageTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();
    private IPersonalizationProvider _personalizationProvider;

    private void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IPersonalizationProvider, PersonalizationProvider>();
      _container.RegisterProvider<IShopperContext, MockShopperContext>();

      _container.Resolve<IShopperContext>().SetNewShopper("12345");

      _personalizationProvider = _container.Resolve<IPersonalizationProvider>();
    }

    [TestMethod]
    public void GetTargetedMessagesObject()
    {
      Initialize();
      var targetMessage = _personalizationProvider.GetTargetedMessages();
      
      XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages));

      using (var stringWriter = new StringWriter())
      {
        serializer.Serialize(stringWriter, targetMessage);
        Debug.Write(stringWriter.ToString());
      }

      Assert.IsTrue(targetMessage.ResultCode == 0);
    }
  }
}
