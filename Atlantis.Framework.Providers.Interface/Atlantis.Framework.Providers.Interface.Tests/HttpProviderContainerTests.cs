using System;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Interface.Tests
{
  [TestClass]
  public class HttpProviderContainerTests
  {
    private const string APP_VIR_DIR = "/";
    private const string APP_PHYSICAL_DIR = @"c:\inetpub\wwwroot";
    private const string PAGE = "http://www.atlantis.framework.interface.tests-com.ide/default.aspx";

    public HttpProviderContainerTests()
    {
      SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(APP_VIR_DIR, APP_PHYSICAL_DIR, PAGE, string.Empty, null, "www.atlantistest-com.ide");
      HttpContext.Current = new HttpContext(workerRequest);
    }

    [TestMethod]
    public void RegisterProvider()
    {
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void RegisterProviderThatDoesNotImplementInterface()
    {
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProviderDoesNotImplementInterface>();
        Assert.Fail(string.Format("This should throw an exception since {0} does not implement {1}.", 
                                  typeof(NameProviderDoesNotImplementInterface).Name, 
                                  typeof(INameProvider).Name));
      }
      catch(ArgumentException argEx)
      {
        if (argEx.Message != string.Format("The type {0} cannot be assigned to type {1}.",
                                           typeof(NameProviderDoesNotImplementInterface).Name,
                                           typeof (INameProvider).Name))
        {
          Assert.Fail(argEx.Message);
        }
        else
        {
          Console.Write(argEx.Message);
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void ResolveProviderWithZeroParameters()
    { 
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
        INameProvider nameProvider = HttpProviderContainer.Instance.Resolve<INameProvider>();
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void ResolveProviderNotRegistered()
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
    public void ResolveProviderDependsOnAnotherProvider()
    {
      try
      {
        HttpProviderContainer.Instance.RegisterProvider<INameProvider, NameProvider>();
        HttpProviderContainer.Instance.RegisterProvider<IPersonProvider, PersonProvider>();

        IPersonProvider personProvider = HttpProviderContainer.Instance.Resolve<IPersonProvider>();
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void ResolveProviderDependsOnAnotherProviderNotRegistered()
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
