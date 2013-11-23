using System;
using System.Web.UI;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.DotTypeRegistration;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Web.DotTypeRegistrationValidationHandler.TestWeb
{
  public partial class _Default : Page
  {
    private IDotTypeRegistrationProvider _provider;
    private IDotTypeRegistrationProvider DotTypeRegistrationProvider
    {
      get { return _provider ?? (_provider = HttpProviderContainer.Instance.Resolve<IDotTypeRegistrationProvider>()); }
    }

    protected IDotTypeFormFieldsByDomain DotTypeFormFieldsByDomain;

    protected void Page_Load(object sender, EventArgs e)
    {
      string[] domains = { "domain1.n.borg", "claim1.example" };

      IDotTypeFormLookup lookup = DotTypeFormLookup.Create("dpp", "j.borg", "MOBILE", "GA");
      DotTypeRegistrationProvider.GetDotTypeFormSchemas(lookup, domains, out DotTypeFormFieldsByDomain);
    }
  }
}