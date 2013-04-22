using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockProviderBase : ProviderBase
  {
    public MockProviderBase(IProviderContainer container) 
      :base(container)
    {
    }

    protected object GetMockSetting(string name)
    {
      object result = null;

      MockProviderContainer mockContainer = Container as MockProviderContainer;
      if (mockContainer != null)
      {
        result = mockContainer.GetMockSetting(name);
      }
      else if (HttpContext.Current != null)
      {
        result = HttpContext.Current.Items[name];
      }

      return result;
    }

  }
}
