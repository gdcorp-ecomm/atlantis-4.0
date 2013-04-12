using System;
using System.Web.UI;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Conditions.Monitor.WebTest
{
  public partial class Default : Page
  {
    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get
      {
        if (_objectProviderContainer == null)
        {
          _objectProviderContainer = new ObjectProviderContainer();
          _objectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _objectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _objectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        }

        return _objectProviderContainer;
      }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      ConditionHandlerManager.EvaluateCondition("dataCenter", new[] {"AP"}, ObjectProviderContainer);
      ConditionHandlerManager.EvaluateCondition("countrySiteContext", new[] { "IN" }, ObjectProviderContainer);
    }
  }
}