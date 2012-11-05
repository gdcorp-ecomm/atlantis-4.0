using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using Atlantis.Framework.GetShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Preferences;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.ShopperFirstOrderGet.Interface;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal abstract class PurchaseConfirmationEmailBase
  {
    private OrderData _orderData;
    private GetShopperResponseData _shopperData;
    private DepartmentIds _departmentIds;
    private EmailTemplates _emailTemplates;
    private EmailRequired _emailRequired;
    private ICurrencyProvider _currency;
    private ILinkProvider _linkProvider;
    private IProductProvider _products;

    ObjectProviderContainer _objectContainer;

    public PurchaseConfirmationEmailBase(OrderData orderData, EmailRequired emailRequired, ObjectProviderContainer objectContainer)
    {
      _objectContainer = objectContainer;
      _objectContainer.RegisterProvider<ISiteContext, OrderData>(orderData);
      _objectContainer.RegisterProvider<IShopperContext, OrderData>(orderData);
      _objectContainer.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>();
      _objectContainer.RegisterProvider<ICurrencyProvider, CurrencyProvider>();

      _orderData = orderData;
      _shopperData = LoadShopper();
      _departmentIds = new DepartmentIds(_orderData.PrivateLabelId);
      _emailRequired = emailRequired;
      _params = new Dictionary<string, AttributeValue>();
      _emailTemplates = new EmailTemplates();
      _linkProvider = _objectContainer.Resolve<ILinkProvider>();
      _currency = _objectContainer.Resolve<ICurrencyProvider>();
      _products = _objectContainer.Resolve<IProductProvider>();

    }

    private GetShopperResponseData LoadShopper()
    {
      string[] fields = new string[5] { "first_name", "last_name", "lu_emailTypeID", "loginName", "email" };
      GetShopperRequestData request = new GetShopperRequestData(
        _orderData.ShopperId, string.Empty, _orderData.OrderId, string.Empty, 0,
        "PurchaseConfirmationEmailBase", string.Empty, fields);
      GetShopperResponseData result = (GetShopperResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.GetShopper);
      return result;
    }

    public bool ShowVATId
    {
      get
      {
        return _orderData.ShowVATId;
      }
    }

    bool? _isShopperFirstOrder;
    public bool IsShopperFirstOrder
    {
      get
      {
        if (!_isShopperFirstOrder.HasValue)
        {
          _isShopperFirstOrder = false;
          ShopperFirstOrderGetRequestData request = new ShopperFirstOrderGetRequestData(_orderData.ShopperId, string.Empty, _orderData.OrderId, _orderData.Pathway, _orderData.PageCount);
          try
          {
            ShopperFirstOrderGetResponseData response = Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.ShopperFirstOrderGet) as ShopperFirstOrderGetResponseData;
            if (response.IsSuccess && response.IsShopperNew)
            {
              _isShopperFirstOrder = true;
            }
          }
          catch (System.Exception ex)
          {
            AtlantisException aex = new AtlantisException(request, "IsShopperFirstOrder", "Exception caught. ", String.Empty, ex);
            Engine.Engine.LogAtlantisException(aex);
          }
        }
        return _isShopperFirstOrder.Value;
      }
    }

    public bool IsNewShopper
    {
      get
      {
        return _orderData.IsNewShopper;
      }
    }

    Dictionary<string, AttributeValue> _params;

    protected void SetParam(string name, string value)
    {
      _params[name] = new AttributeValue(value);
    }

    protected void SetParam(string name, string value, AttributeValueWriteMethod avWriteMethod)
    {
      _params[name] = new AttributeValue(value, (int)avWriteMethod);
    }

    protected AttributeValue GetParam(string name)
    {
      AttributeValue value = null;
      _params.TryGetValue(name, out value);
      return value;
    }

    protected ISiteContext SiteContext
    {
      get { return _orderData; }
    }
    protected IShopperContext ShopperContext
    {
      get { return _orderData; }
    }

    protected GetShopperResponseData ShopperData
    {
      get { return _shopperData; }
    }

    protected EmailTemplates EmailTemplates
    {
      get { return _emailTemplates; }
    }

    protected EmailRequired EmailRequired
    {
      get { return _emailRequired; }
    }

    protected ICurrencyProvider Currency
    {
      get { return _currency; }
    }

    protected ILinkProvider Links
    {
      get { return _linkProvider; }
    }

    protected IProductProvider Products
    {
      get { return _products; }
    }

    protected virtual string ResourceType
    {
      get { return "CartOrder"; }
    }

    protected virtual string ResourceId
    {
      get { return _orderData.OrderId.ToString(); }
    }

    protected OrderData Order
    {
      get { return _orderData; }
    }

    protected abstract EmailTemplate EmailTemplate { get; }
    protected virtual void SetParams() { }

    protected string ToName
    {
      get
      {
        string result = ShopperData.GetField("first_name") + " " + ShopperData.GetField("last_name");
        return result;
      }
    }

    protected string Notes
    {
      get
      {
        return _orderData.Detail.GetAttribute("notes");
      }
    }

    protected string TotalPrice
    {
      get
      {
        string strTotalPrice = _orderData.Detail.GetAttribute("_total_total");
        int totalPrice;
        if (!int.TryParse(strTotalPrice, out totalPrice))
        {
          totalPrice = -1;
        }

        string totalPriceText = Currency.PriceText(new CurrencyPrice(totalPrice, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional), false, CurrencyNegativeFormat.Parentheses);
        return totalPriceText;
      }
    }

    protected virtual bool IsHTMLEmail
    {
      get
      {
        string strEmailTypeId = ShopperData.GetField("lu_emailTypeID");

        // 1 = plain text, 2 = html
        int emailTypeId = 2;
        if (!string.IsNullOrEmpty(strEmailTypeId))
        {
          if (!int.TryParse(strEmailTypeId, out emailTypeId))
          {
            emailTypeId = 2;
          }
        }
        return (emailTypeId == 2);
      }
    }

    protected string LoginName
    {
      get
      {
        return ShopperData.GetField("loginName");
      }
    }

    protected string OrderTime { get { return DateTime.Now.ToString("F"); } }

    protected string VATId
    {
      get
      {
        string vatId = string.Empty;
        if (ShowVATId)
        {
          if ((_orderData.ContextId == ContextIds.GoDaddy) || (_orderData.ContextId == ContextIds.BlueRazor))
          {
            vatId = DataCache.DataCache.GetAppSetting("VAT_GODADDY");
          }
          else
          {
            vatId = DataCache.DataCache.GetAppSetting("VAT_RESELLER");
          }
        }

        if (!string.IsNullOrEmpty(vatId))
          vatId = "VALUE ADDED TAX ID: " + vatId;

        return vatId;
      }
    }

    protected bool IsFraudRefund()
    {
      return _orderData.IsFraudRefund;
    }

    protected bool DoRecurringHostingExists()
    {
      XmlNodeList recurringHostingNodes = _orderData.OrderXmlDoc.SelectNodes(string.Format("/ORDER/ITEMS/ITEM[@dept_id='{0}']", _departmentIds[DepartmentType.RecurringHostingDeptId]));
      return (recurringHostingNodes.Count > 0);
    }

    public virtual List<MessagingProcessRequestData> GetMessageRequests()
    {
      List<MessagingProcessRequestData> result = new List<MessagingProcessRequestData>();

      if (EmailTemplate == null || EmailTemplate.Name == null || EmailTemplate.Namespace == null)
      {
        throw new ArgumentException("Email template is null in " + GetType().Name + ".");
      }

      MessagingProcessRequestData request = new MessagingProcessRequestData(
        _orderData.ShopperId, string.Empty, _orderData.OrderId, string.Empty, 0,
        _orderData.PrivateLabelId, EmailTemplate.Name, EmailTemplate.Namespace, _orderData.LocalizationCode);

      ResourceItem resourceItem = new ResourceItem(ResourceType, ResourceId);

      SetParams();

      foreach (string key in _params.Keys)
      {
        resourceItem[key] = _params[key];
      }

      request.AddResource(resourceItem);

      ContactPointItem emailContact = new ContactPointItem("ShopperContact", ContactPointTypes.Shopper);
      string shopperEmail = _orderData.ShopperEmail;
      if (!string.IsNullOrEmpty(shopperEmail))
      {
        emailContact.ExcludeContactPointType = true;
        emailContact["lastname"] = _orderData.Detail.GetAttribute("lastname");
        emailContact["firstname"] = _orderData.Detail.GetAttribute("firstname");
        emailContact["email"] = shopperEmail;
        emailContact["sendemail"] = "true";
      }
      else
      {
        emailContact["id"] = _orderData.ShopperId;
      }
      emailContact["EmailType"] = IsHTMLEmail ? "html" : "plaintext";
      resourceItem.ContactPoints.Add(emailContact);

      if (ShopperData != null)
      {
        string emailAddress = ShopperData.GetField("email").ToLower();
        string debugPurchaseConfirmEmails = DataCache.DataCache.GetAppSetting("DEBUG_PURCHASE_CONFIRM_EMAILS");

        if (debugPurchaseConfirmEmails.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          try
          {
            StringCollection requestStrings =
              new StringCollection();
            string requestXml = request.ToXML();
            for (int i = 0; i < 10; i++)
            {
              if (requestXml.Length > 3000)
              {
                requestStrings.Add(requestXml.Substring(0, 3000));
                requestXml = requestXml.Substring(3000);
              }
              else
              {
                requestStrings.Add(requestXml);
                break;
              }
            }

            foreach (string subXml in requestStrings)
            {
              AtlantisException aex = new AtlantisException("GetPurchaseConfirmationEmailRequests", string.Empty, "0", subXml, string.Empty,
                _orderData.ShopperId, _orderData.OrderId, string.Empty, string.Empty, 0);
              Engine.Engine.LogAtlantisException(aex);
            }
          }
          catch (Exception)
          {
            //just logging
          }
        }
      }


      result.Add(request);
      return result;
    }

    private string GetDBPText()
    {
      if (DomainsByProxyInOrder)
      {
        return "<br/><span style=\"font-weight:bold\">Because you just purchased Private Registration, watch for an email from "
            + "<span style=\"text-decoration:underline\">Support@DomainsByProxy.com</span>. "
            + "It contains important information about logging in to your Domains By Proxy account.</span>";
      }
      return String.Empty;
    }

    protected string GetItemsText()
    {
      string hostingConcierge = String.Empty;
      EmailCustomTextGenerator emailCustomTextProvider = new EmailCustomTextGenerator(_orderData, _currency, _departmentIds, _linkProvider, _products);

      StringBuilder itemsTextBuilder = new StringBuilder(1000);
      if (_orderData.ContextId == ContextIds.BlueRazor)
      {
        if (IsHTMLEmail)
          emailCustomTextProvider.BuildItemsText_BR_Html(itemsTextBuilder);
        else
          emailCustomTextProvider.BuildItemsText_BR_PlainText(itemsTextBuilder, false);
      }
      else if (_orderData.ContextId == ContextIds.GoDaddy)
      {
        if (IsHTMLEmail)
        {
          emailCustomTextProvider.BuildItemsText_GD_Html(itemsTextBuilder);
          hostingConcierge = HostingConciergeHtmlGet();
        }
        else
          emailCustomTextProvider.BuildItemsText_GD_PlainText(itemsTextBuilder, false);
      }
      else
      {
        if (IsHTMLEmail)
          emailCustomTextProvider.BuildItemsText_PL_Html(itemsTextBuilder);
        else
          emailCustomTextProvider.BuildItemsText_PL_PlainText(itemsTextBuilder, false);
      }

      if (Currency.SelectedDisplayCurrencyType != "USD")
      {
        if (IsHTMLEmail)
        {
          BuildCurrencyWarning_Html(itemsTextBuilder);
        }
        else
        {
          BuildCurrencyWarning_PlainText(itemsTextBuilder);
        }
      }

      //display canadian business logic if tax applied and billing country = canada
      XmlNodeList caBulkRegistrationList = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRegistration/domain[@tld='CA']");
      XmlNodeList caBulkTransferList = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkTransfer/domain[@tld='CA']");

      if (caBulkRegistrationList.Count > 0 || caBulkTransferList.Count > 0)
      {
        string caName, caNumber;
        if ((_orderData.ContextId == ContextIds.GoDaddy) || (_orderData.ContextId == ContextIds.BlueRazor))
        {
          caName = DataCache.DataCache.GetAppSetting("CA_GST_NAME_GD");
          caNumber = DataCache.DataCache.GetAppSetting("CA_GST_ID_GD");
        }
        else
        {
          caName = DataCache.DataCache.GetAppSetting("CA_GST_NAME_PL");
          caNumber = DataCache.DataCache.GetAppSetting("CA_GST_ID_PL");
        }
        if (IsHTMLEmail)
        {
          BuildCATaxInfo_Html(itemsTextBuilder, caName, caNumber);
        }
        else
        {
          BuildCATaxInfo_PlainText(itemsTextBuilder, caName, caNumber);
        }
      }

      string vatId;
      if ((_orderData.ContextId == ContextIds.GoDaddy) || (_orderData.ContextId == ContextIds.BlueRazor))
        vatId = DataCache.DataCache.GetAppSetting("VAT_GODADDY");
      else
        vatId = DataCache.DataCache.GetAppSetting("VAT_RESELLER");

      if (ShowVATId)
      {
        if (IsHTMLEmail)
        {
          BuildVATInfo_Html(itemsTextBuilder, vatId);
        }
        else
        {
          BuildVATInfo_PlainText(itemsTextBuilder, vatId);
        }
      }

      GetEulaBlock(itemsTextBuilder);

      return itemsTextBuilder.ToString();
    }

    protected void GetEulaBlock(StringBuilder itemsTextBuilder)
    {
      EmailCustomTextGenerator emailCustomTextProvider = new EmailCustomTextGenerator(_orderData, _currency, _departmentIds, _linkProvider, _products);
      string hostingConcierge = _orderData.ContextId == ContextIds.GoDaddy && IsHTMLEmail ?
              HostingConciergeHtmlGet() :
              String.Empty;
      string isc = GetISC();
      if (IsHTMLEmail)
      {
        emailCustomTextProvider.BuildEULAHTML(_orderData.EulaProv, itemsTextBuilder, hostingConcierge, isc);
      }
      else
      {
        emailCustomTextProvider.BuildEULAText(_orderData.EulaProv, itemsTextBuilder, false, isc);
      }

      if (DomainsByProxyInOrder)
      {
        itemsTextBuilder.Append("<br/><span style=\"font-weight:bold\">[%%LCST.REQ.UTOS_DBP_EMAIL%%] <span style=\"text-decoration:underline\">Support@DomainsByProxy.com</span>[%%LCST.REQ.UTOS_DBP_LOGIN%%]</span>");
      }
    }

    protected string GetEulaBlock()
    {
      StringBuilder itemsTextBuilder = new StringBuilder(1000);
      GetEulaBlock(itemsTextBuilder);
      return itemsTextBuilder.ToString();
    }

    private string GetISC()
    {
      string isc = null;
      switch (_orderData.PrivateLabelId)
      {
        case 1:
          isc = "bb462075";
          if (EmailTemplate != null)
          {
            switch (EmailTemplate.Id)
            {
              case EmailTemplateType.RecurringHostingConfirmation:
                isc = "gdbb46";
                break;
              case EmailTemplateType.GDWelcome:
                isc = "bb120720";
                break;
            }
          }
          break;
        case 2:
        case 1387:
          isc = "brbb295";
          break;
        default:
          isc = "wwbb42";
          break;
      }
      return isc;
    }

    private string HostingConciergeHtmlGet()
    {
      string conciergeHtml = String.Empty;
      if (IsHTMLEmail)
      {
        conciergeHtml = HostingConciergeTextGet(true, false);
      }
      return conciergeHtml;
    }

    protected string HostingConciergeTextGet(bool boldPhoneNumbers, bool longText)
    {
      StringBuilder conciergeText = new StringBuilder();
      if (HostingConciergeEnabled)
      {
        string hostPhoneUsa;
        string hostPhoneIntl;
        HostPhoneUsaIntlGet(out hostPhoneUsa, out hostPhoneIntl);
        if (HostingConciergeEnabled && !String.IsNullOrEmpty(hostPhoneIntl) && !String.IsNullOrEmpty(hostPhoneUsa))
        {
          if (_parsedWordPress)
          {
            conciergeText.Append("<b>");
            conciergeText.Append("[%%LCST.REQ.UTOS_WP_HOSTING_SUPPLE_PT1%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_WP_HOSTING_SUPPLE_PT2%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_WP_HOSTING_SUPPLE_PT3%%]<br />");
            conciergeText.Append("</b>");
          }
          else if (_parsedCloud)
          {
            conciergeText.Append("<b>");
            conciergeText.Append("[%%LCST.REQ.UTOS_CLD_INFRA_SUPPLE_PT1%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_CLD_INFRA_SUPPLE_PT2%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_CLD_INFRA_SUPPLE_PT3%%]<br />");
            conciergeText.Append("</b>");
          }
          else if (_parsedHosting)
          {
            conciergeText.Append("<b>");
            conciergeText.Append("[%%LCST.REQ.UTOS_HOSTING_SUPPLE_PT1%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_HOSTING_SUPPLE_PT2%%]<br /><br />");
            conciergeText.Append("[%%LCST.REQ.UTOS_HOSTING_SUPPLE_PT3%%]<br />");
            conciergeText.Append("</b>");
          }

        }
      }
      return conciergeText.ToString();
    }

    protected bool DomainsByProxyInOrder
    {
      get
      {
        ParseOrderItems();
        return _parsedDomainsByProxy;
      }
    }

    private bool _parsedHosting = false;
    private bool _parsedDomainsByProxy = false;
    private bool _parsedItems = false;

    private bool _parsedCloud = false;
    private bool _parsedWordPress = false;

    private void ParseOrderItems()
    {
      if (!_parsedItems)
      {
        _parsedItems = true;
        _parsedHosting = false;
        _parsedDomainsByProxy = false;
        XmlNodeList itemNodes = Order.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
        if (itemNodes != null)
        {
          foreach (XmlElement itemElement in itemNodes)
          {
            string pf_id = itemElement.GetAttribute("pf_id");
            int pfIdInt;
            if (Int32.TryParse(pf_id, out pfIdInt))
            {
              if (ProductIds.isHostingProduct(pfIdInt))
              {
                _parsedHosting = true;
              }
              if (ProductIds.isDomainsByProxy(pfIdInt))
              {
                _parsedDomainsByProxy = true;
              }
              if (ProductIds.isWordPressHosting(pfIdInt))
              {
                _parsedWordPress = true;
              }
              if (ProductIds.isCloudServer(pfIdInt))
              {
                _parsedCloud = true;
              }
            }
          }
        }
      }
    }

    protected bool HostingConciergeEnabled
    {
      get
      {
        bool hostingInOrder = false;
        ParseOrderItems();
        if (_parsedHosting || _parsedWordPress || _parsedCloud)
        {
          hostingInOrder = true;
        }
        return hostingInOrder;
      }
    }

    protected static void HostPhoneUsaIntlGet(out string hostPhoneUsa, out string hostPhoneIntl)
    {
      hostPhoneIntl = String.Empty;
      hostPhoneUsa = String.Empty;
      try
      {
        string response = DataCache.DataCache.GetCacheData("<GetHostingConcierge/>");
        if (!String.IsNullOrEmpty(response))
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(response);
          XmlNodeList items = xmlDocument.SelectNodes("/data/item");

          if (items != null)
          {
            foreach (XmlElement item in items)
            {
              if (item.Attributes["constant_desc"] != null && item.Attributes["value"] != null)
              {
                if (item.Attributes["constant_desc"].Value == "PLVC_HOSTPHONEUSA")
                {
                  hostPhoneUsa = item.Attributes["value"].Value;
                }
                if (item.Attributes["constant_desc"].Value == "PLVC_HOSTPHONEINTL")
                {
                  hostPhoneIntl = item.Attributes["value"].Value;
                }
              }
            }
          }
        }
      }
      catch (Exception)
      {
        hostPhoneIntl = String.Empty;
        hostPhoneUsa = String.Empty;
      }
    }




    private void BuildCurrencyWarning_PlainText(StringBuilder itemsTextBuilder)
    {
      itemsTextBuilder.AppendLine(Environment.NewLine + "[%%LCST.REQ.UTOS_TRANSPRICE%%]");
    }
    private void BuildCurrencyWarning_Html(StringBuilder itemsTextBuilder)
    {
      itemsTextBuilder.AppendLine("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td colspan='3' align='center' class='bodyText'><hr></td></tr>");
      itemsTextBuilder.AppendLine("<tr><td class='subText' style='white-space:normal'>[%%LCST.REQ.UTOS_TRANSPRICE%%]</td></tr></table>");
    }

    private void BuildCATaxInfo_PlainText(StringBuilder itemsTextBuilder, string caName, string caNumber)
    {
      itemsTextBuilder.AppendLine(Environment.NewLine + "[%%LCST.REQ.UTOS_CAN_GST%%]:");
      itemsTextBuilder.AppendLine("Business Name: " + caName);
      itemsTextBuilder.AppendLine("Business Number: " + caNumber);
    }
    private void BuildCATaxInfo_Html(StringBuilder itemsTextBuilder, string caName, string caNumber)
    {
      itemsTextBuilder.AppendLine("<br/><table cellspacing='1' cellpadding='0' border='0' class='bodyText'>");
      itemsTextBuilder.AppendLine("<tr><td colspan='3' style='line-height:5px'>&nbsp;</td></tr>");
      itemsTextBuilder.AppendLine("<tr><td colspan='3' class='bodyText'><b>[%%LCST.REQ.UTOS_CAN_GST%%]:</b></td></tr>");
      itemsTextBuilder.AppendLine("<tr><td style='line-height:5px'>&nbsp;</td></tr>");
      itemsTextBuilder.AppendLine("<tr><td class='bodyText' style='padding-left:3px'>[%%LCST.REQ.UTOS_BUS_NAME%%]: </td>");
      itemsTextBuilder.AppendLine("<td class='bodyText'>" + caName + "</td>");
      itemsTextBuilder.AppendLine("</tr>");
      itemsTextBuilder.AppendLine("<tr><td class='bodyText' style='padding-left:3px'>[%%LCST.REQ.UTOS_BUS_NUMBER%%]: </td>");
      itemsTextBuilder.AppendLine("<td class='bodyText'>" + caNumber + "</td>");
      itemsTextBuilder.AppendLine("</tr>");
      itemsTextBuilder.AppendLine("<tr><td style='line-height:5px'>&nbsp;</td></tr></table>");
    }

    private void BuildVATInfo_PlainText(StringBuilder itemsTextBuilder, string vatId)
    {
      itemsTextBuilder.AppendLine();
      itemsTextBuilder.AppendLine(" [%%LCST.REQ.UTOS_VAT%%]: " + vatId);
    }
    private void BuildVATInfo_Html(StringBuilder itemsTextBuilder, string vatId)
    {
      itemsTextBuilder.AppendLine("<br/><table cellspacing='1' cellpadding='0' border='0' class='bodyText'>");
      itemsTextBuilder.AppendLine("<tr><td class='bodyText' style='padding-left:3px'> [%%LCST.REQ.UTOS_VAT%%]: </td>");
      itemsTextBuilder.AppendLine("<td class='bodyText'>" + vatId + "</td>");
      itemsTextBuilder.AppendLine("</tr>");
      itemsTextBuilder.AppendLine("<tr><td style='line-height:5px'>&nbsp;</td></tr></table>");
    }

    protected string GetCrossSellItems()
    {
      string iscCode = GetISC();

      var emailCustomTextProvider = new EmailCustomTextGenerator(_orderData, _currency, _departmentIds, _linkProvider, _products);
      CrossSellConfig crossSellConfig = new CrossSellConfig(_orderData, _departmentIds, _products);

      StringBuilder itemsTextBuilder = new StringBuilder(1000);

      if (IsHTMLEmail)
        emailCustomTextProvider.BuildCrossSellHTML(crossSellConfig.CrossSellProductList, itemsTextBuilder, crossSellConfig.CiCode, iscCode);
      else
        emailCustomTextProvider.BuildCrossSellText(crossSellConfig.CrossSellProductList, itemsTextBuilder, crossSellConfig.CiCode, false, iscCode);

      return itemsTextBuilder.ToString();
    }

    #region Video.Me

    Dictionary<string, int> _shopperProductData;
    protected string GetVideoLink(string htmlOrText)
    {
      StringBuilder pmBuilder = new StringBuilder(1000);
      if (!HasVideoMe())
      {
        if (htmlOrText == "html")
        {
          pmBuilder.Append("<div>");
          pmBuilder.Append("<img src='" + Links.ImageRoot + "cart/img_video_me.gif' width='88' height='37' align='left' hspace='4'/>");
          pmBuilder.Append("[%%LCST.REQ.UTOS_PS%%] <a href='http://www.video.me/Default.aspx'>[%%LCST.REQ.UTOS_LOGIN%%]</a> [%%LCST.REQ.UTOS_GD_USERNAME%%]");
          pmBuilder.Append("</div>");
        }
        else
        {
          pmBuilder.Append("[%%LCST.REQ.UTOS_PS%%] [%%LCST.REQ.UTOS_LOGIN%%] [%%LCST.REQ.UTOS_GD_USERNAME%%] : http://www.video.me/Default.aspx");
        }
      }
      return pmBuilder.ToString();
    }

    private bool HasVideoMe()
    {
      bool hasVideoMe = false;
      if (_shopperProductData == null)
      {
        EmailCustomTextGenerator emailCustomTextProvider = new EmailCustomTextGenerator(_orderData, _currency, _departmentIds, _linkProvider, _products);
        CrossSellConfig crossSellConfig = new CrossSellConfig(_orderData, _departmentIds, _products);
        Dictionary<string, int> shopperProductData = crossSellConfig.GetShopperProductDictionary(_orderData.ShopperId, _orderData.PrivateLabelId.ToString(), true);
        _shopperProductData = shopperProductData;
      }
      //Check mirage data
      if (_shopperProductData.ContainsKey(ProductIds.VideoMeEcon1mo.ToString()) ||
      _shopperProductData.ContainsKey(ProductIds.VideoMeEcon1yr.ToString()) ||
      _shopperProductData.ContainsKey(ProductIds.VideoMeEcon2yr.ToString()) ||
      _shopperProductData.ContainsKey(ProductIds.VideoMeEcon3yr.ToString()))
      {
        hasVideoMe = true;
      }
      if (!hasVideoMe)
      {
        //check current order
        XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes(string.Format("/ORDER/ITEMS/ITEM[@pf_id='{0}' or @pf_id='{1}' or @pf_id='{2}' or @pf_id='{3}']",
        ProductIds.VideoMeEcon1mo.ToString(),
        ProductIds.VideoMeEcon1yr.ToString(),
        ProductIds.VideoMeEcon2yr.ToString(),
        ProductIds.VideoMeEcon3yr.ToString()));
        if (itemNodes.Count > 0)
        {
          hasVideoMe = true;
        }
      }
      return hasVideoMe;
    }


    #endregion

  }
}
