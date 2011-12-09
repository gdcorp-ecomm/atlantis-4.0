using System.Collections.Generic;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EmailTemplates : Dictionary<EmailTemplateType, EmailTemplate>
  {
    const string ORDER_PROCESSING = "OrderProcessing";

    public EmailTemplates() : base(10)
    {
      this.Add(EmailTemplateType.MEPurchaseReceipt, new EmailTemplate(EmailTemplateType.MEPurchaseReceipt, "MEPurchaseReceipt", "TDNAM"));
      this.Add(EmailTemplateType.InvalidWhoIsNoticeAPICustomer, new EmailTemplate(EmailTemplateType.InvalidWhoIsNoticeAPICustomer, "InvalidWhoIsNoticeAPICustomer", "Legal"));
      this.Add(EmailTemplateType.GDShopsPurchaseReceipt, new EmailTemplate(EmailTemplateType.GDShopsPurchaseReceipt, "GDShopsPurchaseReceipt", "GDShops"));
      this.Add(EmailTemplateType.CustomerAccountLocked, new EmailTemplate(EmailTemplateType.CustomerAccountLocked, "CustomerAccountLocked", "Fraud"));
      this.Add(EmailTemplateType.AdminFeesConfirmation, new EmailTemplate(EmailTemplateType.AdminFeesConfirmation, "AdminFeesConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.GDWelcome, new EmailTemplate(EmailTemplateType.GDWelcome, "GDWelcomeAlt", ORDER_PROCESSING));
      this.Add(EmailTemplateType.RecurringHostingConfirmation, new EmailTemplate(EmailTemplateType.RecurringHostingConfirmation, "RecurringHostingConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.OrderConfirmation, new EmailTemplate(EmailTemplateType.OrderConfirmation, "OrderConfirmationAlt", ORDER_PROCESSING));
      this.Add(EmailTemplateType.MiscFeesConfirmation, new EmailTemplate(EmailTemplateType.MiscFeesConfirmation, "FeeProcessed", ORDER_PROCESSING));
      this.Add(EmailTemplateType.ArizonaHumaneSociety, new EmailTemplate(EmailTemplateType.ArizonaHumaneSociety, "ArizonaHumaneSociety", ORDER_PROCESSING));
      this.Add(EmailTemplateType.RefundConfirmation, new EmailTemplate(EmailTemplateType.RefundConfirmation, "RefundConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.RefundConfirmation_ME, new EmailTemplate(EmailTemplateType.RefundConfirmation_ME, "MERefund", ORDER_PROCESSING));
      this.Add(EmailTemplateType.RefundConfirmation_DBP, new EmailTemplate(EmailTemplateType.RefundConfirmation_DBP, "DBPRefund", ORDER_PROCESSING));
      this.Add(EmailTemplateType.OrderConfirmation_WWD, new EmailTemplate(EmailTemplateType.OrderConfirmation_WWD, "WWDOrderConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.OrderConfirmation_SuperReseller, new EmailTemplate(EmailTemplateType.OrderConfirmation_SuperReseller, "SRInstantResellerOrderConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.OrderConfirmation_DBP, new EmailTemplate(EmailTemplateType.OrderConfirmation_DBP, "DBPOrderConfirmation", ORDER_PROCESSING));
      this.Add(EmailTemplateType.OrderConfirmation_ProductOnlyReseller, new EmailTemplate(EmailTemplateType.OrderConfirmation_ProductOnlyReseller, "ProductOnlyResellerOrderConfirmation", ORDER_PROCESSING));
	    this.Add(EmailTemplateType.OrderConfirmation_WelcomeTellAFriend, new EmailTemplate(EmailTemplateType.OrderConfirmation_WelcomeTellAFriend, "WelcomeTellAFriend", ORDER_PROCESSING));
    }
  }
}