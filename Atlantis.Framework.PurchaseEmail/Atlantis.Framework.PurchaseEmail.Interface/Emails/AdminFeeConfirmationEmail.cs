using System.Xml;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class AdminFeeConfirmationEmail : PurchaseConfirmationEmailBase
  {
    public AdminFeeConfirmationEmail(OrderData orderData, EmailRequired emailRequired, ObjectProviderContainer objectContainer)
      : base(orderData, emailRequired,objectContainer)
    {
    }

    protected override void SetParams()
    {
      SetParam(EmailTokenNames.ShopperName, ToName);
      SetParam(EmailTokenNames.Domain, Domain);
      SetParam(EmailTokenNames.Notes, Notes);
      SetParam(EmailTokenNames.TotalPrice, TotalPrice);
      SetParam(EmailTokenNames.DomainAgreement, DomainAgreement);
      SetParam(EmailTokenNames.AgreementText, AgreementText);
      SetParam(EmailTokenNames.ProcessingText, ProcessingText);
    }

    protected override bool IsHTMLEmail
    {
      get
      {
        return false; //this email is always sent in the plain text format
      }
    }

    protected virtual string AgreementText
    {
      get
      {
        return "our registration agreement";
      }
    }

    protected virtual string ProcessingText
    {
      get
      {
        return "our processing of this inquiry";
      }
    }

    protected virtual string Domain
    {
      get
      {
        string domain = string.Empty;
        XmlNodeList itemNodes = Order.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
        foreach (XmlElement itemElement in itemNodes)
        {
          string tempDomain = itemElement.GetAttribute("domain");
          if (!string.IsNullOrEmpty(tempDomain))
          {
            domain = tempDomain;
            break;
          }
        }
        return domain;
      }
    }

    protected virtual string DomainAgreement
    {
      get
      {
        string domainAgreement;
        switch (SiteContext.PrivateLabelId)
        {
          case 1:
            domainAgreement = Links.GetUrl(LinkTypes.SiteRoot, "gdshop/legal_agreements/domain_registration_GD.asp", QueryParamMode.CommonParameters, true, "prog_id", SiteContext.ProgId);
            break;
          case 2:
            domainAgreement = Links.GetUrl(LinkTypes.SiteRoot, "gdshop/legal_agreements/domain_registration_BR.asp", QueryParamMode.CommonParameters, true, "prog_id", SiteContext.ProgId);
            break;
          default:
            domainAgreement = Links.GetUrl(LinkTypes.SiteRoot, "gdshop/legal_agreements/domain_registration_WWD.asp", QueryParamMode.CommonParameters, true, "prog_id", SiteContext.ProgId);
            break;
        }
        return domainAgreement;
      }
    }

    protected override EmailTemplate EmailTemplate
    {
      get
      {
        EmailTemplate temp;
        if (SiteContext.PrivateLabelId == 1)
        {
          temp = (EmailRequired.ApiAdminFee) ? EmailTemplates[EmailTemplateType.InvalidWhoIsNoticeAPICustomer] : EmailTemplates[EmailTemplateType.AdminFeesConfirmation];
        }
        else
        {
          temp = EmailTemplates[EmailTemplateType.AdminFeesConfirmation];
        }
        return temp;
      }
    }

  }
}
