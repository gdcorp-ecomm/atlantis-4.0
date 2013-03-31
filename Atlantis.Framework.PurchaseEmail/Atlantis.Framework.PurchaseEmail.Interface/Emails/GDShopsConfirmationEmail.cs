using System.Collections.Generic;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.Providers.Containers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class GDShopsConfirmationEmail : PurchaseConfirmationEmailBase
  {
    public GDShopsConfirmationEmail(OrderData orderData, EmailRequired emailRequired, ObjectProviderContainer objectContainer)
      : base(orderData, emailRequired,objectContainer)
    {
    }

    protected override void SetParams()
    {
      SetParam(EmailTokenNames.OrderId, Order.OrderId);
      SetParam(EmailTokenNames.VendorInfo, GetItemsText());
      SetParam(EmailTokenNames.ShopperId, ShopperContext.ShopperId);
      SetParam(EmailTokenNames.TotalPrice, string.Empty); //this token is deliberately supplied an empty string as it is redundantely and wrongly used in the template.
      SetParam(EmailTokenNames.LoginName, LoginName);
      SetParam(EmailTokenNames.OrderTime, OrderTime);
    }

    protected override EmailTemplate EmailTemplate
    {
      get { return EmailTemplates[EmailTemplateType.GDShopsPurchaseReceipt]; }
    }

    public override List<MessagingProcessRequestData> GetMessageRequests()
    {
      List<MessagingProcessRequestData> result = null;
      if (Order.PrivateLabelId == 1)
      {
        result = base.GetMessageRequests();
      }
      return result;
    }
  }
}
