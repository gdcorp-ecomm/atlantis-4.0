using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.Providers.Containers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class BRConfirmationEmail : PurchaseConfirmationEmailBase
  {
    private bool _isDevServer = false;

    public BRConfirmationEmail(OrderData orderData, EmailRequired emailRequired, bool isDevServer, ObjectProviderContainer objectContainer)
      : base(orderData, emailRequired, objectContainer)
    {
      _isDevServer = isDevServer;
    }

    protected override void SetParams()
    {
      SetParam(EmailTokenNames.OrderId, Order.OrderId);
      if (EmailTemplate.Id == EmailTemplateType.OrderConfirmation)
      {
        string xmlParam;
        using (var ms = new MemoryStream())
        {
          Order.OrderXmlDoc.Save(ms);
          xmlParam = String.Concat("<?xml version=\"1.0\" encoding=\"utf-16\"?>", Encoding.Default.GetString(ms.ToArray()));
          ms.Close();
        }

        SetParam(EmailTokenNames.CartXml, xmlParam, AttributeValueWriteMethod.CDataBlock);

        SetParam(EmailTokenNames.ItemsText, GetEulaBlock());
      }
      else
      {
        SetParam(EmailTokenNames.ItemsText, GetItemsText());
      }
      SetParam(EmailTokenNames.ShopperId, ShopperContext.ShopperId);
      SetParam(EmailTokenNames.OrderTotal, TotalPrice);
      SetParam(EmailTokenNames.ServerName, _isDevServer ? "account.dev.bluerazor.com" : "account.bluerazor.com");
      SetParam(EmailTokenNames.LoginName, LoginName);
      SetParam(EmailTokenNames.CC_Address, string.Empty);
      SetParam(EmailTokenNames.OrderTime, OrderTime);
      SetParam(EmailTokenNames.VATId, VATId);
    }

    protected override EmailTemplate EmailTemplate
    {
      get
      {
        EmailTemplate temp;

        if (Order.IsRefund)
        {
          if (IsFraudRefund())
          {
            temp = EmailTemplates[EmailTemplateType.CustomerAccountLocked];
          }
          else
          {
            temp = EmailTemplates[EmailTemplateType.RefundConfirmation];
          }
        }
        else
        {
          if (DoRecurringHostingExists())
          {
            temp = EmailTemplates[EmailTemplateType.RecurringHostingConfirmation];
          }
          else
          {
            if (EmailRequired.ProcessFee && EmailRequired.OtherProductIdsExist)
            {
              temp = EmailTemplates[EmailTemplateType.OrderConfirmation];
            }
            else
            {
              if (EmailRequired.ProcessFee && !EmailRequired.OtherProductIdsExist)
              {
                temp = EmailTemplates[EmailTemplateType.MiscFeesConfirmation];
              }
              else
              {
                temp = EmailTemplates[EmailTemplateType.OrderConfirmation];
              }
            }
          }
        }

        return temp;
      }
    }

    public override List<Atlantis.Framework.MessagingProcess.Interface.MessagingProcessRequestData> GetMessageRequests()
    {
      if (Order.PrivateLabelId == 2)
      {
        return base.GetMessageRequests();
      }
      else
      {
        return null;
      }
    }
  }
}
