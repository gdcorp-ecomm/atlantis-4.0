using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.PurchaseEmail.Interface.Emails.Eula;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EmailCustomTextGenerator
  {
    OrderData _orderData;
    ICurrencyProvider _currency;
    ILinkProvider _links;
    IProductProvider _products;

    public EmailCustomTextGenerator(OrderData orderData, ICurrencyProvider currency, DepartmentIds departmentIds, ILinkProvider links, IProductProvider products)
    {
      _orderData = orderData;
      _currency = currency;
      _links = links;
      _products = products;
    }

    #region ItemText Functions - Blue Razor
    public void BuildItemsText_BR_PlainText(StringBuilder itemsTextBuilder, bool debug)
    {
      BuildItemsText_GD_PlainText(itemsTextBuilder, debug);
    }
    public void BuildItemsText_BR_Html(StringBuilder itemTextBuilder)
    {
      itemTextBuilder.Append("<table cellspacing='2' cellpadding='2' border='0' style='font-family:Arial;font-size:11px'>");
      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td align='right' valign='bottom' class='bodyText'><u>qty</u></td>");
      itemTextBuilder.Append("    <td align='center' valign='bottom' class='bodyText'><u>item</u></td>");
      itemTextBuilder.Append("    <td align='right' valign='bottom' class='bodyText'><u>price</u></td>");
      itemTextBuilder.Append("  </tr>");

      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string isDisplayedInCart = itemElement.GetAttribute("isdisplayedincart");
        bool shouldDisplay = isDisplayedInCart != "0";
        if (shouldDisplay)
        {
          itemTextBuilder.Append("  <tr>");
          itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" +
                                 itemElement.GetAttribute("quantity") + "</td>");
          itemTextBuilder.Append("    <td valign='top' class='bodyText'>" + itemElement.GetAttribute("name"));

          //check for domain nodes in the item's CUSTOMXML 
          XmlNodeList domainNodes = itemElement.SelectNodes("./CUSTOMXML/*/domain");
          foreach (XmlNode domainNode in domainNodes)
          {
            //concatenating to string that holds email text.
            itemTextBuilder.Append("<br />" + domainNode.Attributes["sld"].Value + "." +
                                   domainNode.Attributes["tld"].Value);
            XmlAttribute intlDomainNameAttrib = domainNode.Attributes["intlDomainName"];
            string intlDomainName = null;
            if (intlDomainNameAttrib != null)
            {
              intlDomainName = intlDomainNameAttrib.Value;
            }
            if (!string.IsNullOrEmpty(intlDomainName))
            {
              string intlSld, intlTld;
              BasketHelpers.GetDomainParts(intlDomainName, out intlSld, out intlTld);

              itemTextBuilder.Append("<br />(" + HttpUtility.HtmlEncode(intlSld) + "." +
                                     domainNode.Attributes["tld"].Value + ")");
            }
          }

          //use the domain attribute if the customxml was not used.
          if (domainNodes.Count == 0 && !string.IsNullOrEmpty(itemElement.GetAttribute("domain")))
          {
            itemTextBuilder.Append("<br />" + itemElement.GetAttribute("domain"));
          }

          itemTextBuilder.Append("</td>");

          int itemPrice;
          int.TryParse(itemElement.GetAttribute("_oadjust_adjustedprice"), out itemPrice);

          ICurrencyPrice itemCurrencyPrice = _currency.NewCurrencyPrice(itemPrice, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
          string itemPriceText = _currency.PriceText(itemCurrencyPrice, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses);

          itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" + itemPriceText + "</td>");
          itemTextBuilder.Append("  </tr>");
        }
      }

      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td colspan='3' align='right' class='bodyText'><hr></td>");
      itemTextBuilder.Append("  </tr>");
      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td colspan='2' align='right' valign='top' class='bodyText'>[%%LCST.REQ.SUBTOTAL%%]:</td>");
      itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" + _currency.PriceText(_orderData.SubTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td>");
      itemTextBuilder.Append("  </tr>");
      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td colspan='2' align='right' valign='top' class='bodyText'>[%%LCST.REQ.SHIPPING_AND_HANDLING_PLAIN_TEXT%%]:</td>");
      itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" + _currency.PriceText(_orderData.TotalShipping) + "</td>");
      itemTextBuilder.Append("  </tr>");
      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td colspan='2' align='right' valign='top' class='bodyText'>[%%LCST.REQ.TAX%%]:</td>");
      itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" + _currency.PriceText(_orderData.TotalTax) + "</td>");
      itemTextBuilder.Append("  </tr>");
      itemTextBuilder.Append("  <tr>");
      itemTextBuilder.Append("    <td colspan='2' align='right' valign='top' class='bodyText'>[%%LCST.REQ.TOTAL%%]:</td>");
      itemTextBuilder.Append("    <td align='right' valign='top' class='bodyText'>" + _currency.PriceText(_orderData.TotalTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td>");
      itemTextBuilder.Append("  </tr>");
      itemTextBuilder.Append("</table>");
    }
    #endregion

    #region ItemText Functions - Private Label
    public string BuildPadStringFunction(string tagValue, int columnWidth, HorizontalAlign justification)
    {
      System.Text.StringBuilder functionBuilder = new StringBuilder(200);
      functionBuilder.Append("[%%FUNC.REQ.PAD.");
      functionBuilder.Append(columnWidth.ToString());
      functionBuilder.Append(". .");
      switch (justification)
      {
        case HorizontalAlign.Center:
          functionBuilder.Append("2");
          break;
        case HorizontalAlign.Justify:
        case HorizontalAlign.Left:
        default:
          functionBuilder.Append("0");
          break;
        case HorizontalAlign.Right:
          functionBuilder.Append("1");
          break;
      }
      functionBuilder.Append(".1.");
      functionBuilder.Append(tagValue);
      functionBuilder.Append("%%]");
      return functionBuilder.ToString();
    }

    public void BuildItemsText_PL_PlainText(StringBuilder itemsTextBuilder, bool debug)
    {
      itemsTextBuilder.AppendLine(
          BuildPadStringFunction("[%%LCST.REQ.QTY%%]", PlainTextPadding.QTY_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + BuildPadStringFunction("[%%LCST.REQ.ITEM%%]", PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + string.Empty.PadLeft(PlainTextPadding.DOLLAR_COL_WIDTH)
        + BuildPadStringFunction("[%%LCST.REQ.PRICE%%]", PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Center)
        );
      if (debug) itemsTextBuilder.Append("<br/>");
      itemsTextBuilder.AppendLine(string.Empty.PadLeft(PlainTextPadding.TOT_COLS_WIDTH, '-') + " ");
      if (debug) itemsTextBuilder.Append("<br/>");


      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        int itemPrice;
        int.TryParse(itemElement.GetAttribute("_oadjust_adjustedprice"), out itemPrice);

        itemsTextBuilder.AppendLine(
          PadStringForColumn(itemElement.GetAttribute("quantity"), PlainTextPadding.QTY_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(itemElement.GetAttribute("name"), PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_currency.NewCurrencyPrice(itemPrice, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional), PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Center)
        );

        if (debug) itemsTextBuilder.Append("<br/>");

        //check for domain nodes in the item's CUSTOMXML 
        XmlNodeList domainNodes = itemElement.SelectNodes("./CUSTOMXML/*/domain");
        foreach (XmlNode domainNode in domainNodes)
        {
          //concatenating to string that holds email text.
          itemsTextBuilder.AppendLine(
              string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH)
            + domainNode.Attributes["sld"].Value + "." + domainNode.Attributes["tld"].Value);
          if (debug) itemsTextBuilder.Append("<br/>");
        }

        //use the domain attribute if the customxml was not used.
        if (domainNodes.Count == 0 && !string.IsNullOrEmpty(itemElement.GetAttribute("domain")))
        {
          itemsTextBuilder.AppendLine(
            string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH)
          + itemElement.GetAttribute("domain"));
          if (debug) itemsTextBuilder.Append("<br/>");
        }
      }

      itemsTextBuilder.AppendLine(string.Empty.PadLeft(PlainTextPadding.TOT_COLS_WIDTH, '-') + " ");
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.SUBTOTAL%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.SubTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.SHIPPING_AND_HANDLING_PLAIN_TEXT%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalShipping), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.TAX%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalTax), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.TOTAL%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");
    }

    public void BuildItemsText_PL_Html(StringBuilder itemTextBuilder)
    {
      itemTextBuilder.Append("<table width='270' cellspacing='0' cellpadding='0' border='0'><tr><td align='left' class='bodyText'><u>QTY</u></td>");
      itemTextBuilder.Append("<td align='center' class='bodyText'><u>ITEM</u></td>");
      itemTextBuilder.Append("<td align='right' class='bodyText'><u>PRICE</u></td></tr></table>");

      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string isDisplayedInCart = itemElement.GetAttribute("isdisplayedincart");
        bool shouldDisplay = isDisplayedInCart != "0";
        if (shouldDisplay)
        {
          int itemPrice;
          int.TryParse(itemElement.GetAttribute("_oadjust_adjustedprice"), out itemPrice);

          itemTextBuilder.Append(
            "<table width='270' cellspacing='2' cellpadding='2' border='0'><tr><td align='left' class='bodyText'>" +
            itemElement.GetAttribute("quantity") + "</td>");
          itemTextBuilder.Append("<td align='center' class='bodyText'>" + itemElement.GetAttribute("name") + "</td>");
          itemTextBuilder.Append("<td align='right' class='bodyText'>" +
                                 _currency.PriceText(new CurrencyPrice(itemPrice, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional), PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) +
                                 "</td></tr></table>");

          //check for domain nodes in the item's CUSTOMXML 
          XmlNodeList domainNodes = itemElement.SelectNodes("./CUSTOMXML/*/domain");
          foreach (XmlNode domainNode in domainNodes)
          {
            //concatenating to string that holds email text.
            itemTextBuilder.Append(
              "<table width='270' cellspacing='0' cellpadding='0' border='0'><tr><td class='bodyText' colspan='3' align='center'>" +
              domainNode.Attributes["sld"].Value + "." + domainNode.Attributes["tld"].Value);
            XmlAttribute intlDomainNameAttrib = domainNode.Attributes["intlDomainName"];
            string intlDomainName = null;
            if (intlDomainNameAttrib != null)
            {
              intlDomainName = intlDomainNameAttrib.Value;
            }
            if (!string.IsNullOrEmpty(intlDomainName))
            {
              string intlSld, intlTld;
              BasketHelpers.GetDomainParts(intlDomainName, out intlSld, out intlTld);

              itemTextBuilder.Append("<br />(" + HttpUtility.HtmlEncode(intlSld) + "." +
                                     domainNode.Attributes["tld"].Value + ")");
            }
            itemTextBuilder.Append("</td></tr></table>");
          }

          //use the domain attribute if the customxml was not used.
          if (domainNodes.Count == 0 && !string.IsNullOrEmpty(itemElement.GetAttribute("domain")))
          {
            itemTextBuilder.Append(
              "<table width='270' cellspacing='0' cellpadding='0' border='0'><tr><td class='bodyText' colspan='3' align='center'>" +
              itemElement.GetAttribute("domain"));
            itemTextBuilder.Append("</td></tr></table>");
          }
        }
      }
      itemTextBuilder.Append("<table width='270' cellspacing='0' cellpadding='0' border='0'><tr><td colspan='3' align='center' class='bodyText'><hr></td></tr>");
      itemTextBuilder.Append("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.SUBTOTAL%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.SubTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td></tr>");
      itemTextBuilder.Append("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.SHIPPING_AND_HANDLING_PLAIN_TEXT%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalShipping) + "</td></tr>");
      itemTextBuilder.Append("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.TAX%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalTax) + "</td></tr>");
      itemTextBuilder.Append("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.TOTAL%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td></tr>");
      itemTextBuilder.Append("</table>");
    }
    #endregion

    #region ItemText Functions - Godaddy
    public void BuildItemsText_GD_PlainText(StringBuilder itemsTextBuilder, bool debug)
    {
      itemsTextBuilder.AppendLine(
          BuildPadStringFunction("[%%LCST.REQ.QTY%%]", PlainTextPadding.QTY_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + BuildPadStringFunction("[%%LCST.REQ.ITEM%%]", PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Center)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + string.Empty.PadLeft(PlainTextPadding.DOLLAR_COL_WIDTH)
        + BuildPadStringFunction("[%%LCST.REQ.PRICE%%]", PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Center)
        );

      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(string.Empty.PadLeft(PlainTextPadding.TOT_COLS_WIDTH, '-'));

      if (debug) itemsTextBuilder.Append("<br/>");

      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string periodDescription = GetItemPeriodDuration(itemElement);
        string itemPrice = GetItemPrice(itemElement);
        string nameWithPeriodDescription = itemElement.GetAttribute("name");

        if (!string.IsNullOrEmpty(periodDescription))
          nameWithPeriodDescription += (", " + periodDescription);

        itemsTextBuilder.AppendLine(
          PadStringForColumn(itemElement.GetAttribute("quantity"), PlainTextPadding.QTY_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(nameWithPeriodDescription, nameWithPeriodDescription.Length, HorizontalAlign.Left)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(itemPrice, PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right)
        );
        if (debug) itemsTextBuilder.Append("<br/>");

        //check for domain nodes in the item's CUSTOMXML 
        XmlNodeList domainNodes = itemElement.SelectNodes("./CUSTOMXML/*/domain");
        foreach (XmlNode domainNode in domainNodes)
        {
          //concatenating to string that holds email text.
          itemsTextBuilder.AppendLine(
              string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH)
            + domainNode.Attributes["sld"].Value + "." + domainNode.Attributes["tld"].Value);
          if (debug) itemsTextBuilder.Append("<br/>");
        }

        if (_orderData.Detail.GetAttribute("basket_type") == "marketplace")
        {
          string merchantShopId = itemElement.GetAttribute("mp_shop_id");
          MerchantInfo merchantInfo = new MerchantInfo(merchantShopId);

          itemsTextBuilder.AppendLine(Environment.NewLine + merchantInfo.MarketPlaceName + Environment.NewLine);
          if (debug) itemsTextBuilder.Append("<br/>");
          itemsTextBuilder.AppendLine("[%%LCST.REQ.UTOS_PHONE%%]: " + merchantInfo.SupportPhone + Environment.NewLine);
          if (debug) itemsTextBuilder.Append("<br/>");
          itemsTextBuilder.AppendLine(merchantInfo.SupportEmailAddress + Environment.NewLine);
          if (debug) itemsTextBuilder.Append("<br/>");
        }

        //use the domain attribute if the customxml was not used.
        if (domainNodes.Count == 0 && !string.IsNullOrEmpty(itemElement.GetAttribute("domain")))
        {
          itemsTextBuilder.AppendLine(
            string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH)
          + itemElement.GetAttribute("domain"));
          if (debug) itemsTextBuilder.Append("<br/>");
        }
      }

      itemsTextBuilder.AppendLine(string.Empty.PadLeft(PlainTextPadding.TOT_COLS_WIDTH, '-') + " ");
      if (debug) itemsTextBuilder.Append("<br/>");

      if (_orderData.OrderDiscountAmount.Price != 0)
      {
        itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.SPECIAL_SAVINGS%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
          + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
          + PadStringForColumn(_currency.PriceText(_orderData.OrderDiscountAmount), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
        if (debug) itemsTextBuilder.Append("<br/>");
      }

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.SUBTOTAL%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.SubTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.SHIPPING_AND_HANDLING_PLAIN_TEXT%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalShipping), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.TAX%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalTax), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");

      itemsTextBuilder.AppendLine(BuildPadStringFunction("[%%LCST.REQ.TOTAL%%]:", PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.QTY_COL_WIDTH + PlainTextPadding.NAME_COL_WIDTH, HorizontalAlign.Right)
        + string.Empty.PadLeft(PlainTextPadding.SPACER_COL_WIDTH)
        + PadStringForColumn(_currency.PriceText(_orderData.TotalTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses), PlainTextPadding.PRICE_COL_WIDTH, HorizontalAlign.Right));
      if (debug) itemsTextBuilder.Append("<br/>");
    }

    public void BuildItemsText_GD_Html(StringBuilder itemsTextBuilder)
    {
      itemsTextBuilder.Append("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td align='left' class='bodyText'><u>QTY</u></td>");
      itemsTextBuilder.Append("<td align='center' class='bodyText'><u>[%%LCST.REQ.ITEM%%]</u></td>");
      itemsTextBuilder.Append("<td align='right' class='bodyText' style='padding-right:4px'><u>[%%LCST.REQ.PRICE%%]</u></td></tr></table>");

      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string periodDescription = GetItemPeriodDuration(itemElement);
        string itemPrice = GetItemPrice(itemElement);

        //if (periodDescription.Length > 0) itemsTextBuilder.Append("<br/>" + periodDescription);

        itemsTextBuilder.Append("<table width='300' cellspacing='2' cellpadding='2' border='0'><tr><td valign='top' align='left' class='bodyText' width='14px'>" + itemElement.GetAttribute("quantity") + "</td>");
        itemsTextBuilder.Append("<td valign='top' align='left' class='bodyText' style='padding-left:12px'>" + itemElement.GetAttribute("name").HtmlWrapWithOutBreakingAnyWords(30) + "<br/>" + periodDescription + "</td>");
        itemsTextBuilder.Append("<td valign='top' align='right' class='bodyText'>" + itemPrice + "</td></tr></table>");

        //check for domain nodes in the item's CUSTOMXML 
        XmlNodeList domainNodes = itemElement.SelectNodes("./CUSTOMXML/*/domain");
        foreach (XmlNode domainNode in domainNodes)
        {
          itemsTextBuilder.Append("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td class='bodyText' colspan='3' align='left' style='padding-left:26px'>" + domainNode.Attributes["sld"].Value + "." + domainNode.Attributes["tld"].Value);
          XmlAttribute intlDomainNameAttrib = domainNode.Attributes["intlDomainName"];
          string intlDomainName = null;
          if (intlDomainNameAttrib != null)
          {
            intlDomainName = intlDomainNameAttrib.Value;
          }
          if (!string.IsNullOrEmpty(intlDomainName))
          {
            string intlSld, intlTld;
            BasketHelpers.GetDomainParts(intlDomainName, out intlSld, out intlTld);
            itemsTextBuilder.Append("<br />(" + HttpUtility.HtmlEncode(intlSld + "." + domainNode.Attributes["tld"].Value + ")"));

          }
          itemsTextBuilder.Append("</td></tr></table>");
        }

        if (_orderData.Detail.GetAttribute("basket_type") == "marketplace")
        {
          string merchantShopId = itemElement.GetAttribute("mp_shop_id");
          MerchantInfo merchantInfo = new MerchantInfo(merchantShopId);

          itemsTextBuilder.AppendLine("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td class='bodyText' colspan='3' align='left' style='padding-left:26px'>");
          itemsTextBuilder.AppendLine(merchantInfo.MarketPlaceName);
          itemsTextBuilder.AppendLine("<br />[%%LCST.REQ.UTOS_PHONE%%]: " + merchantInfo.SupportPhone);
          itemsTextBuilder.AppendLine("<br />" + merchantInfo.SupportEmailAddress);
          itemsTextBuilder.AppendLine("</td></tr></table>");
        }

        itemsTextBuilder.AppendLine("<table cellpadding=0 cellspacing=0 border=0><tr><td style='line-height:4px'>&nbsp;</td></tr></table>");

        //use the domain attribute if the customxml was not used.
        if (domainNodes.Count == 0 && !string.IsNullOrEmpty(itemElement.GetAttribute("domain")))
        {
          itemsTextBuilder.AppendLine("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td class='bodyText' colspan='3' align='center'>" + itemElement.GetAttribute("domain") + "</td></tr></table>");
        }
      }

      itemsTextBuilder.AppendLine("<table width='300' cellspacing='0' cellpadding='0' border='0'><tr><td colspan='3' align='center' class='bodyText'><hr></td></tr>");
      if (_orderData.OrderDiscountAmount.Price != 0)
      {
        itemsTextBuilder.AppendLine("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.SPECIAL_SAVINGS%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.OrderDiscountAmount) + "</td></tr>");
      }
      itemsTextBuilder.AppendLine("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.SUBTOTAL%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.SubTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td></tr>");
      itemsTextBuilder.AppendLine("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.SHIPPING_AND_HANDLING_PLAIN_TEXT%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalShipping) + "</td></tr>");
      itemsTextBuilder.AppendLine("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.TAX%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalTax) + "</td></tr>");
      itemsTextBuilder.AppendLine("<tr><td colspan='3' align='right' class='bodyText'>[%%LCST.REQ.TOTAL%%]:&nbsp;&nbsp;&nbsp;" + _currency.PriceText(_orderData.TotalTotal, PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses) + "</td></tr>");
      itemsTextBuilder.AppendLine("</table>");
    }

    public string GetItemPeriodDuration(XmlElement item)
    {
      string returnValue = string.Empty;

      string duration = item.GetAttribute("duration");
      string origDescription = item.GetAttribute("period_description");

      if (!string.IsNullOrEmpty(duration) && string.IsNullOrEmpty(item.GetAttribute("parent_bundle_id")))
      {
        string description = null;
        if (string.IsNullOrEmpty(origDescription))
        {
          if (item.GetAttribute("recurring_payment") == "monthly")
          {
            description = "month(s)";
          }
          else
          {
            if (item.GetAttribute("recurring_payment") == "annual")
            {
              description = "year(s)";
            }
          }
          if (!string.IsNullOrEmpty(description))
          {
            returnValue = string.Format("Length: {0} {1}", duration, description);
          }
        }
        else
        {
          returnValue = string.Format("Length: {0} {1}", duration, origDescription);
        }
      }

      return returnValue;
    }

    public string GetItemPrice(XmlElement item)
    {
      string adjustedOrderPrice = item.GetAttribute("_oadjust_adjustedprice");

      if ((adjustedOrderPrice != null) && string.IsNullOrEmpty(item.GetAttribute("parent_bundle_id")))
      {
        int itemPrice;
        int.TryParse(adjustedOrderPrice, out itemPrice);
        return _currency.PriceText(new CurrencyPrice(itemPrice, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional), PriceTextOptions.AllowNegativePrice, PriceFormatOptions.NegativeParentheses);
      }
      else
        return string.Empty;
    }

    #endregion

    #region ItemsText Functions - Plain Text related
    public string PadStringForColumn(string text, int colWidth, HorizontalAlign justification)
    {
      string paddedString;

      if (text.Length > colWidth)
      {
        paddedString = text.Substring(0, colWidth);
      }
      else
      {
        if (text.Length == colWidth)
        {
          paddedString = text;
        }
        else
        {
          int paddingSize = colWidth - text.Length;
          switch (justification)
          {
            case HorizontalAlign.Center:
              int reminder = (paddingSize % 2);
              if (reminder == 0) //even
              {
                paddedString = text.PadLeft(reminder + text.Length);
                paddedString = paddedString.PadRight(reminder + paddedString.Length);
              }
              else //odd
              {
                paddedString = text.PadLeft(reminder + 1 + text.Length);
                paddedString = paddedString.PadRight(reminder + paddedString.Length);
              }
              break;
            case HorizontalAlign.Right:
              paddedString = text.PadRight(paddingSize + text.Length);
              break;
            case HorizontalAlign.Left:
            default:
              paddedString = text.PadLeft(paddingSize + text.Length);
              break;
          }
        }
      }

      return paddedString;
    }
    public static class PlainTextPadding
    {
      public const int SPACER_COL_WIDTH = 2;
      public const int QTY_COL_WIDTH = 4;
      public const int NAME_COL_WIDTH = 42;
      public const int DOLLAR_COL_WIDTH = 1;
      public const int PRICE_COL_WIDTH = 11;
      public const int TOT_COLS_WIDTH = 62;
    }
    #endregion

    #region EULA Text Functions

    public void BuildEULAText(EulaProvider eulaProvider, StringBuilder itemsTextBuilder, bool debug, string iscCode)
    {
      if (eulaProvider.ConfiguredEULA.Count > 0)
      {
        itemsTextBuilder.AppendLine(Environment.NewLine + Environment.NewLine + "[%%LCST.REQ.UTOS_IMPORTANT_INFO%%]:");
        if (debug) itemsTextBuilder.Append("<br/>");

        if (eulaProvider.ConfiguredEULA.Contains(eulaProvider.GetEULAData(EULARuleType.GiftCard)))
        {
          EULAItem eulaData = eulaProvider.GetEULAData(EULARuleType.GiftCard);
          if (eulaData != null)
          {
            itemsTextBuilder.Append("[%%LCST.REQ.UTOS_GIFT_EMAIL%%] ");
            itemsTextBuilder.Append("[%%LCST.REQ.UTOS_GIFT_RECIP%%]");
          }
        }
        foreach (EULAItem eulaData in eulaProvider.ConfiguredEULA)
        {
          string legalInfoURL = eulaData.LegalAgreementURL.Replace("%7bisc%7d", iscCode);

          if (eulaData != null)
          {
            string productName = eulaData.ProductName;
            string productInfoURL = eulaData.ProductInfoURL.Replace("%7bisc%7d", iscCode);

            if (!string.IsNullOrEmpty(productName))
            {
              itemsTextBuilder.AppendLine(Environment.NewLine + productName + " ");
            }
            if (!string.IsNullOrEmpty(productInfoURL))
            {
              itemsTextBuilder.AppendLine("[%%LCST.REQ.UTOS_PRODUCT_INFO%%]: " + productInfoURL);
            }
            if (!string.IsNullOrEmpty(legalInfoURL))
            {
              itemsTextBuilder.AppendLine(GetAgreementTypeText(eulaData.AgreementType) + ": " + legalInfoURL);
            }
            if (debug) itemsTextBuilder.Append("<br/><br/>");
          }
        }
      }
    }

    public void BuildEULAHTML(EulaProvider eulaProvider, StringBuilder itemsTextBuilder, string hostingConcierge, string iscCode)
    {
      if (eulaProvider.ConfiguredEULA.Count > 0)
      {
        itemsTextBuilder.Append("<br /><br /><table cellspacing='1' cellpadding='3' border='0' class='bodyText' bgcolor='#EEEEEE' ");
        itemsTextBuilder.Append("style=\"font-size: 12px; color: black; font-family: arial,sans serif;width:100%;\">");
        itemsTextBuilder.Append("<tr><td colspan='3' style='line-height:5px'>&nbsp;</td></tr>");
        itemsTextBuilder.Append("<tr><td colspan='3' class='bodyText'><b>[%%LCST.REQ.UTOS_IMPORTANT_INFO%%]:</b></td></tr>");
        if (eulaProvider.ConfiguredEULA.Contains(eulaProvider.GetEULAData(EULARuleType.GiftCard)))
        {
          EULAItem eulaData = eulaProvider.GetEULAData(EULARuleType.GiftCard);
          if (eulaData != null)
          {
            itemsTextBuilder.Append("<tr><td colspan='3' class='bodyText'>");
            itemsTextBuilder.Append("[%%LCST.REQ.UTOS_GIFT_EMAIL%%] ");
            itemsTextBuilder.Append("[%%LCST.REQ.UTOS_GIFT_RECIP%%]");
            itemsTextBuilder.Append("</td></tr>");
          }
        }
        itemsTextBuilder.Append("<tr><td style='line-height:5px;width:30%;'>&nbsp;</td></tr>");


        bool hostingConciergeHasShown = false;
        foreach (EULAItem eulaData in eulaProvider.ConfiguredEULA)
        {
          string legalInfoURL = eulaData.LegalAgreementURL.Replace("%7bisc%7d", iscCode);

          if (eulaData != null)
          {
            string productName = eulaData.ProductName;
            string productInfoURL = eulaData.ProductInfoURL.Replace("%7bisc%7d", iscCode);

            itemsTextBuilder.Append("<tr>");
            if (!string.IsNullOrEmpty(productName))
            {
              itemsTextBuilder.AppendFormat("<td class='bodyText' style='padding-left:3pxwidth:30%;'>{0} </td>", productName);
            }
            if (!string.IsNullOrEmpty(productInfoURL))
            {
              itemsTextBuilder.AppendFormat("<td style='width:30%;' class='bodyText'><a href='{0}'>[%%LCST.REQ.UTOS_PRODUCT_INFO%%]</a></td>", productInfoURL);
            }
            if (!string.IsNullOrEmpty(legalInfoURL))
            {
              itemsTextBuilder.AppendFormat("<td style='width:30%;' class='bodyText'><a href='{0}'>{1}</a></td>", legalInfoURL, GetAgreementTypeText(eulaData.AgreementType));
            }
            itemsTextBuilder.Append("</tr>");

            if (!String.IsNullOrEmpty(hostingConcierge) && !hostingConciergeHasShown)
            {
              switch (eulaData.RuleType)
              {
                case EULARuleType.Unknown:
                case EULARuleType.Hosting:
                case EULARuleType.DedHosting:
                case EULARuleType.DedVirtHosting:
                  {
                    hostingConciergeHasShown = true;
                    itemsTextBuilder.Append("<tr><td colspan='3' class='bodyText' style='padding-left:3px'>");
                    itemsTextBuilder.Append(hostingConcierge);
                    itemsTextBuilder.Append("</td></tr>");
                  }
                  break;
              }
            }
            itemsTextBuilder.Append("<tr><td width='30%' style='line-height:5px'>&nbsp;</td></tr>");

          }
        }

        itemsTextBuilder.Append("</table>");
      }
    }

    private string GetAgreementTypeText(EULAType agreementType)
    {
      string text = string.Empty;
      switch (agreementType)
      {
        case EULAType.Legal:
          text = "[%%LCST.REQ.UTOS_LEGAL_AGREEMENT%%]";
          break;
        case EULAType.Service:
          text = "[%%LCST.REQ.UTOS_SERVICE_AGREEMENT%%]";
          break;
        case EULAType.Membership:
          text = "[%%LCST.REQ.UTOS_MEMBERSHIP_AGREEMENT%%]";
          break;
      }
      return text;
    }
    #endregion

    #region CrossSell Items Functions
    public void BuildCrossSellText(List<CrossSellConfigProductId> crossSellProductIdList, StringBuilder itemsTextBuilder, int ciCode, bool debug, string iscCode)
    {
      CrossSellData xsellProductProvider = new CrossSellData(_orderData, _links, _currency, _products, ciCode);
      int index = 0;

      foreach (CrossSellConfigProductId productId in crossSellProductIdList)
      {
        CrossSellProduct product = xsellProductProvider.GetCrossSellProduct(productId, iscCode);
        if (product != null)
        {
          if (++index > 1)
          {
            itemsTextBuilder.AppendLine("==============================================================");
          }

          if (!string.IsNullOrEmpty(product.ProductName))
          {
            itemsTextBuilder.AppendLine(product.ProductName.ToUpper());
          }
          if (!string.IsNullOrEmpty(product.PriceText))
          {
            itemsTextBuilder.Append(product.PriceText);
          }
          if (product.Id == CrossSellConfigProductId.TRAFFICB || product.Id == CrossSellConfigProductId.HOSTING || product.Id == CrossSellConfigProductId.CART)
          {
            if (!string.IsNullOrEmpty(product.SavingsText))
            {
              itemsTextBuilder.Append(" " + product.SavingsText);
            }
            if (!string.IsNullOrEmpty(product.ProductDescription))
            {
              itemsTextBuilder.Append(" " + product.ProductDescription);
            }
          }
          else
          {
            if (!string.IsNullOrEmpty(product.ProductDescription))
            {
              itemsTextBuilder.Append(" " + product.ProductDescription);
            }
            if (!string.IsNullOrEmpty(product.SavingsText))
            {
              itemsTextBuilder.Append(" " + product.SavingsText);
            }
          }
          if (!string.IsNullOrEmpty(product.ProductUrl))
          {
            itemsTextBuilder.AppendLine();
            itemsTextBuilder.AppendLine("Go to: " + product.ProductUrl);
          }

          if (debug) itemsTextBuilder.Append("<br/><br/>");
        }
      }
    }
    public void BuildCrossSellHTML(List<CrossSellConfigProductId> crossSellProductIdList, StringBuilder itemsTextBuilder, int ciCode, string iscCode)
    {
      bool tableAdded = false;
      CrossSellData xsellProductProvider = new CrossSellData(_orderData, _links, _currency, _products, ciCode);
      int index = 0;
      foreach (CrossSellConfigProductId productId in crossSellProductIdList)
      {
        CrossSellProduct product = xsellProductProvider.GetCrossSellProduct(productId, iscCode);
        if (product != null && (!string.IsNullOrEmpty(product.ProductName)
            || !string.IsNullOrEmpty(product.ProductDescription)))
        {
          if (!tableAdded)
          {
            tableAdded = true;
            itemsTextBuilder.Append("<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" " +
                 "style=\"border: 1px solid #999999\" bgcolor=\"#FDFFDA\" width=\"100%\">");
          }

          if (++index > 1)
          {
            itemsTextBuilder.AppendLine("<tr><td style=\"padding:6px\"><hr size=\"1\"></td></tr>");
          }
          itemsTextBuilder.Append("<tr><td style=\"padding:6px;font-size:10px;line-height:14px;font-family:verdana, sans serif\">");
          if (!string.IsNullOrEmpty(product.ProductUrl) && !string.IsNullOrEmpty(product.ProductName))
          {
            itemsTextBuilder.AppendFormat("<a href=\"{0}\">{1}</a>", product.ProductUrl, product.ProductName);
          }
          if (!string.IsNullOrEmpty(product.PriceText))
          {
            itemsTextBuilder.AppendFormat(" - <span style=\"color:#CC0000;\">{0}</span>", product.PriceText);
          }
          if (product.Id == CrossSellConfigProductId.TRAFFICB || product.Id == CrossSellConfigProductId.HOSTING || product.Id == CrossSellConfigProductId.CART)
          {
            if (!string.IsNullOrEmpty(product.SavingsText))
            {
              itemsTextBuilder.AppendFormat(" <span style=\"color:#CC0000;\">{0}</span>", product.SavingsText);
            }
            if (!string.IsNullOrEmpty(product.ProductDescription))
            {
              itemsTextBuilder.Append(" " + product.ProductDescription);
            }
          }
          else
          {
            if (!string.IsNullOrEmpty(product.ProductDescription))
            {
              itemsTextBuilder.Append(" " + product.ProductDescription);
            }
            if (!string.IsNullOrEmpty(product.SavingsText))
            {
              itemsTextBuilder.AppendFormat(" <span style=\"color:#CC0000;\">{0}</span>", product.SavingsText);
            }
          }

          itemsTextBuilder.AppendLine("</td></tr>");
        }
      }

      if (tableAdded)
      {
        itemsTextBuilder.AppendLine("</table>");
      }
    }
    #endregion
  }
}
