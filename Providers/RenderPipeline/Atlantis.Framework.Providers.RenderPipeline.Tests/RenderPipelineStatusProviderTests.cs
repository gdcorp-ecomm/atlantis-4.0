using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class RenderPipelineStatusProviderTests
  {
    public static IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      return providerContainer;
    }

    [TestMethod]
    public void RenderPipelineStatus_Create()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Collection<AtlantisException> exceptions = new Collection<AtlantisException>
      {
        new AtlantisException("RenderPipelineStatus_Create", 5, "An exception occurred", "Data about the exception")
      };

      Collection<KeyValuePair<string, string>> data = new Collection<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("myKey", "myValue")
      };

      RenderPipelineStatus status = (RenderPipelineStatus) statusProvider.CreateNewStatus(RenderPipelineResult.Success, "foo", exceptions, data);

      Assert.IsTrue(status.Status == RenderPipelineResult.Success);
      Assert.IsTrue(status.Source == "foo");
      Assert.IsTrue(status.Exceptions.Count() == 1);
      Assert.IsTrue(status.Data.Count() == 1);
    }

    [TestMethod]
    public void RenderPipelineStatus_AddException()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      RenderPipelineStatus status = (RenderPipelineStatus)statusProvider.CreateNewStatus(RenderPipelineResult.Success, "foo");
      status.AddException(new AtlantisException("SourceFunction", 1, "ErrorDescription", "Data"));

      Assert.IsTrue(status.Status == RenderPipelineResult.Success);
      Assert.IsTrue(status.Source == "foo");
      Assert.IsTrue(status.Exceptions.Count() == 1);
      Assert.IsTrue(!status.Data.Any());
    }

    [TestMethod]
    public void RenderPipelineStatus_AddData()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      RenderPipelineStatus status = (RenderPipelineStatus)statusProvider.CreateNewStatus(RenderPipelineResult.Success, "foo");
      status.AddData("SomeKey", "SomeValue");

      Assert.IsTrue(status.Status == RenderPipelineResult.Success);
      Assert.IsTrue(status.Source == "foo");
      Assert.IsTrue(!status.Exceptions.Any());
      Assert.IsTrue(status.Data.Count() == 1);
    }

    [TestMethod]
    public void RenderPipelineStatusProvider_Create()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.Success);
      Assert.IsTrue(!statusProvider.GetStatuses().Any());
    }

    [TestMethod]
    public void RenderPipelineStatusProvider_CreateSuccessStatus()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Collection<KeyValuePair<string, string>> data = new Collection<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("myKey", "myValue")
      };

      statusProvider.Add(statusProvider.CreateNewStatus(RenderPipelineResult.Success, "foo", null, data));

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.Success);
      Assert.IsTrue(statusProvider.GetStatuses().Count() == 1);
    }

    [TestMethod]
    public void RenderPipelineStatusProvider_CreateSuccessWithErrorsStatus()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Collection<AtlantisException> exceptions = new Collection<AtlantisException>
      {
        new AtlantisException("RenderPipelineStatusProvider_CreateSuccessWithErrorsStatus", 5, "An minor exception occurred", "Data about the exception")
      };

      statusProvider.Add(statusProvider.CreateNewStatus(RenderPipelineResult.SuccessWithErrors, "foo", exceptions));

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.SuccessWithErrors);
      Assert.IsTrue(statusProvider.GetStatuses().Count() == 1);
    }

    [TestMethod]
    public void RenderPipelineStatusProvider_CreateErrorsStatus()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Collection<AtlantisException> exceptions = new Collection<AtlantisException>
      {
        new AtlantisException("RenderPipelineStatusProvider_CreateSuccessWithErrorsStatus", 5, "An major exception occurred", "Data about the exception")
      };

      statusProvider.Add(statusProvider.CreateNewStatus(RenderPipelineResult.Error, "foo", exceptions));

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.Error);
      Assert.IsTrue(statusProvider.GetStatuses().Count() == 1);
    }

    [TestMethod]
    public void RenderPipelineStatusProvider_Reset()
    {
      RenderPipelineStatusProvider statusProvider = new RenderPipelineStatusProvider(InitializeProviderContainer());

      Collection<AtlantisException> exceptions = new Collection<AtlantisException>
      {
        new AtlantisException("RenderPipelineStatusProvider_CreateSuccessWithErrorsStatus", 5, "An minor exception occurred", "Data about the exception")
      };

      Collection<KeyValuePair<string, string>> data = new Collection<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("myKey", "myValue")
      };

      statusProvider.Add(statusProvider.CreateNewStatus(RenderPipelineResult.SuccessWithErrors, "foo", exceptions, data));

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.SuccessWithErrors);
      Assert.IsTrue(statusProvider.GetStatuses().Count() == 1);

      statusProvider.Reset();

      Assert.IsTrue(statusProvider.Status == RenderPipelineResult.Success);
      Assert.IsTrue(!statusProvider.GetStatuses().Any());
    }
  }
}
