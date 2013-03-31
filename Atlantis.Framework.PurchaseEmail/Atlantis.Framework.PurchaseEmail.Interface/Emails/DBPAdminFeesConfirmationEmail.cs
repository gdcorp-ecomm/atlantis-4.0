using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class DBPAdminFeesConfirmationEmail : AdminFeeConfirmationEmail
  {
    public DBPAdminFeesConfirmationEmail(OrderData orderData, EmailRequired emailRequired, ObjectProviderContainer objectContainer)
      : base(orderData, emailRequired, objectContainer)
    {
    }

    protected override string DomainAgreement
    {
      get
      {
        return Links.GetUrl(LinkTypes.DomainsByProxy, "policy/ShowDoc.aspx", QueryParamMode.ExplicitParameters, "pageid", "domain_nameproxy"); ;
      }
    }

    protected override string AgreementText
    {
      get
      {
        return "Domains By Proxy's Domain Name Proxy Agreement";
      }
    }

    protected override string ProcessingText
    {
      get
      {
        return "DBP's processing of this inquiry";
      }
    }
  }
}
