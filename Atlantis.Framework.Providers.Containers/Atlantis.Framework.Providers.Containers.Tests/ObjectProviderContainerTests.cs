using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  /// <summary>
  /// Summary description for ObjectProviderContainerTests
  /// </summary>
  [TestClass]
  public class ObjectProviderContainerTests
  {
    private IProviderContainer ObjectContainer { get; set; }

    [TestInitialize]
    public void InitializeObjectContainer()
    {
      ObjectContainer = new ObjectProviderContainer();
    }

    [TestMethod]
    public void ObjectRegisterProvider()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
    }

    [TestMethod]
    public void ObjectRegisterProviderThatDoesNotImplementInterface()
    {
      try
      {
        ObjectContainer.RegisterProvider<INameProvider, NameProviderDoesNotImplementInterface>();
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
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      ObjectContainer.SetData<string>("myname", "SuperName");
      string value = ObjectContainer.GetData<string>("myname", "defaultName");
      Assert.AreEqual("SuperName", value);
    }

    [TestMethod]
    public void ObjectResolveProviderWithZeroParameters()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider = ObjectContainer.Resolve<INameProvider>();

      INameProvider nameProvider2 = ObjectContainer.Resolve<INameProvider>();
      Assert.AreEqual(nameProvider, nameProvider2);
    }

    [TestMethod]
    public void CanResolveTrue()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      Assert.IsTrue(ObjectContainer.CanResolve<INameProvider>());
    }

    [TestMethod]
    public void CanResolveFalse()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      Assert.IsFalse(ObjectContainer.CanResolve<IEmployeeProvider>());
    }

    [TestMethod]
    public void TryResolveSuccess()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider;
      bool success = ObjectContainer.TryResolve(out nameProvider);
      Assert.IsTrue(success);
      Assert.IsNotNull(nameProvider);
    }

    [TestMethod]
    public void TryResolveNotFound()
    {
      IEmployeeProvider provider;
      bool success = ObjectContainer.TryResolve(out provider);
      Assert.IsFalse(success);
      Assert.IsNull(provider);
    }

    [TestMethod]
    public void TryResolveRepeated()
    {
      ObjectProviderContainer newContainer = new ObjectProviderContainer();
      newContainer.RegisterProvider<INameProvider, NameProvider>();
      INameProvider nameProvider;
      bool success = newContainer.TryResolve(out nameProvider);
      nameProvider.FirstName = "Hello";

      INameProvider sameName;
      success = newContainer.TryResolve(out sameName);
      Assert.IsTrue(success);
      Assert.AreEqual("Hello", sameName.FirstName);
    }

    [TestMethod]
    public void ObjectResolveProviderNotRegistered()
    {
      try
      {
        IEmployeeProvider employeeProvider = ObjectContainer.Resolve<IEmployeeProvider>();
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
    public void ObjectResolveProviderDependsOnAnotherProvider()
    {
      ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
      ObjectContainer.RegisterProvider<IPersonProvider, PersonProvider>();

      IPersonProvider personProvider = ObjectContainer.Resolve<IPersonProvider>();
    }

    [TestMethod]
    public void ObjectResolveProviderDependsOnAnotherProviderNotRegistered()
    {
      try
      {
        ObjectContainer.RegisterProvider<INameProvider, NameProvider>();
        ObjectContainer.RegisterProvider<IManagerProvider, ManagerProvider>();

        // IManagerProvider also needs the IEmployeeProvider registered, but we will not register it on purpose
        IManagerProvider managerProvider = ObjectContainer.Resolve<IManagerProvider>();
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
