using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  [TestClass]
  public class HttpProviderContainerTests
  {
    private const string PAGE = "http://www.atlantis.framework.interface.tests-com.ide/default.aspx";

    [TestInitialize]
    public void InitializeHttpContext()
    {
      MockHttpRequest request = new MockHttpRequest(PAGE);
      MockHttpContext.SetFromWorkerRequest(request);
    }

    [TestMethod]
    public void HttpRegisterProvider()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
    }

    [TestMethod]
    public void HttpRegisterProviderThatDoesNotImplementInterface()
    {
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProviderDoesNotImplementInterface>();
        Assert.Fail(string.Format("This should throw an exception since {0} does not implement {1}.",
                                  typeof(NameProviderDoesNotImplementInterface).Name,
                                  typeof(INameProvider).Name));
      }
      catch (ArgumentException argEx)
      {
        if (argEx.Message != string.Format("The type {0} cannot be assigned to type {1}.",
                                           typeof(NameProviderDoesNotImplementInterface).Name,
                                           typeof(INameProvider).Name))
        {
          Assert.Fail(argEx.Message);
        }
        else
        {
          Console.Write(argEx.Message);
        }
      }
    }

    [TestMethod]
    public void GetAndSetData()
    {
      HttpProviderContainer.Instance.SetData<string>("myname", "SuperName");
      string value = HttpProviderContainer.Instance.GetData<string>("myname", "defaultName");
      Assert.AreEqual("SuperName", value);
      HttpContext.Current.Items.Clear();
      value = HttpProviderContainer.Instance.GetData<string>("myname", "defaultName");
      Assert.AreEqual("defaultName", value);
    }

    [TestMethod]
    public void HttpResolveProviderWithZeroParameters()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider = HttpProviderContainer.Instance.Resolve<INameProvider>();

      INameProvider nameProvider2 = HttpProviderContainer.Instance.Resolve<INameProvider>();
      Assert.IsTrue(ReferenceEquals(nameProvider, nameProvider2));
    }

    [TestMethod]
    public void HttpResolveProviderWithZeroParametersCompatibility()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider = Providers.Interface.ProviderContainer.HttpProviderContainer.Instance.Resolve<INameProvider>();
    }

    [TestMethod]
    public void HttpCanResolveTrue()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      Assert.IsTrue(HttpProviderContainer.Instance.CanResolve<INameProvider>());
    }

    [TestMethod]
    public void HttpCanResolveFalse()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      Assert.IsFalse(HttpProviderContainer.Instance.CanResolve<IEmployeeProvider>());
    }

    [TestMethod]
    public void HttpTryResolveSuccess()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider;
      bool success = HttpProviderContainer.Instance.TryResolve(out nameProvider);
      Assert.IsTrue(success);
      Assert.IsNotNull(nameProvider);
    }

    [TestMethod]
    public void HttpTryResolveRepeated()
    {
      HttpContext.Current.Items.Clear();
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider;
      bool success = HttpProviderContainer.Instance.TryResolve(out nameProvider);
      nameProvider.FirstName = "Hello";

      INameProvider sameName;
      success = HttpProviderContainer.Instance.TryResolve(out sameName);
      Assert.IsTrue(success);
      Assert.AreEqual("Hello", sameName.FirstName);
    }

    [TestMethod]
    public void HttpTryResolveNotFound()
    {
      IEmployeeProvider provider;
      bool success = HttpProviderContainer.Instance.TryResolve(out provider);
      Assert.IsFalse(success);
      Assert.IsNull(provider);
    }

    [TestMethod]
    public void HttpResolveProviderNotRegistered()
    {
      try
      {
        IEmployeeProvider employeeProvider = HttpProviderContainer.Instance.Resolve<IEmployeeProvider>();
      }
      catch (Exception ex)
      {
        if (!ex.Message.StartsWith(string.Format("Type {0} is not registered.", typeof(IEmployeeProvider).Name)))
        {
          Assert.Fail(ex.Message);
        }
        else
        {
          Console.Write(ex.Message);
        }
      }
    }

    [TestMethod]
    public void HttpResolveProviderDependsOnAnotherProvider()
    {
      HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IPersonProvider, PersonProvider>();

      IPersonProvider personProvider = HttpProviderContainer.Instance.Resolve<IPersonProvider>();
    }

    [TestMethod]
    public void HttpResolveProviderDependsOnAnotherProviderNotRegistered()
    {
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
        HttpProviderContainer.Instance.RegisterProvider<IManagerProvider, ManagerProvider>();

        // IManagerProvider also needs the IEmployeeProvider registered, but we will not register it on purpose
        IManagerProvider managerProvider = HttpProviderContainer.Instance.Resolve<IManagerProvider>();
        INameProvider nameProvider = managerProvider.NameProvider;
      }
      catch (Exception ex)
      {
        if (!ex.Message.StartsWith(string.Format("Type {0} is not registered.", typeof(IEmployeeProvider).Name)))
        {
          Assert.Fail(ex.Message);
        }
        else
        {
          Console.Write(ex.Message);
        }
      }
    }
  }
}
