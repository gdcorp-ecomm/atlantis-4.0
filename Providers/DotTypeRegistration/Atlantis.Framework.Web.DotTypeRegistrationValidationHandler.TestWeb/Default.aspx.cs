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
      //LoadEligibilityFormSchemas();
      LoadClaimDataFormSchemas();
    }

    private void LoadEligibilityFormSchemas()
    {
      string[] domains = {"domain1.j.borg"};

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "j.borg", "MOBILE", "GA");
      DotTypeRegistrationProvider.GetDotTypeFormSchemas(lookup, domains, out DotTypeFormFieldsByDomain);
    }

    private void LoadClaimDataFormSchemas()
    {
      string[] domains = { "validateandt9st.lrclaim" };

      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "MOBILE", "LR");
      DotTypeRegistrationProvider.GetDotTypeFormSchemas(lookup, domains, out DotTypeFormFieldsByDomain);
    }
  }
}