using System;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class CrossSellData
  {
    OrderData _orderData;
    int _ciCode;
    ILinkProvider _links;
    ICurrencyProvider _currency;
    IProductProvider _products;

    public CrossSellData(OrderData orderData, ILinkProvider links, ICurrencyProvider currency, IProductProvider products, int ciCode)
    {
      _orderData = orderData;
      _ciCode = ciCode;
      _links = links;
      _currency = currency;
      _products = products;
    }

    public CrossSellProduct GetCrossSellProduct(CrossSellConfigProductId productId, string iscCode)
    {
        string[] queryStringArgs = new string[] { "isc", iscCode, "ci", _ciCode.ToString(), "prog_id", _orderData.ProgId };

      const string FORMAT_PRICETEXT = "Just {0}/mo for 12 months!";

      string productUrl = null;
      string productName = null;
      string priceText = null;
      string productDescription = null;
      string savingsText = null;
      switch (productId)
      {
        case CrossSellConfigProductId.HOSTING:
          IProductView hosting_asp_d_1year = _products.NewProductView(_products.GetProduct(ProductIds.Hosting_Asp_D_1year));
          IProductView hosting_cgi_d_1year = _products.NewProductView(_products.GetProduct(ProductIds.Hosting_Cgi_D_1year));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/hosting/shared.asp",QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Deluxe Hosting";
          if (hosting_asp_d_1year.MonthlyCurrentPrice.Price != hosting_cgi_d_1year.MonthlyCurrentPrice.Price)
          {
            priceText = string.Format("Just {0}/mo Windows or {1}/mo Linux for 12 months!", _currency.PriceText(hosting_asp_d_1year.MonthlyCurrentPrice, false), _currency.PriceText(hosting_cgi_d_1year.MonthlyCurrentPrice, false));
          }
          else
          {
            priceText = string.Format(FORMAT_PRICETEXT, _currency.PriceText(hosting_asp_d_1year.MonthlyCurrentPrice, false));
          }
          savingsText = GetSavingsText(hosting_asp_d_1year, _products.GetProduct(ProductIds.Hosting_Asp_D_Monthly));
          productDescription = "Host with 50,000MB space, 500GB transfer, FREE software & more!";
          break;
        case CrossSellConfigProductId.WST:
          IProductView wst_d_1year = _products.NewProductView(_products.GetProduct(ProductIds.WST_D_1year));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/hosting/hosting_build_website.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = ProductNames.WebsiteCreatorName + " Deluxe";
          priceText = string.Format(FORMAT_PRICETEXT, _currency.PriceText(wst_d_1year.MonthlyCurrentPrice, false));
          savingsText = GetSavingsText(wst_d_1year, _products.GetProduct(ProductIds.website10pg));
          productDescription = "Build your site online in minutes!";
          break;
        case CrossSellConfigProductId.CART:
          IProductView cart_E_1year = _products.NewProductView(_products.GetProduct(ProductIds.Cart_E_1year));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/ecommerce/cart.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Quick Shopping Cart&#174; Economy";
          priceText = string.Format("Just {0}/mo for 12 months.", _currency.PriceText(cart_E_1year.MonthlyCurrentPrice, false));
          savingsText = GetSavingsText(cart_E_1year, _products.GetProduct(ProductIds.Cart_E_Monthly));
          productDescription = "Build a Web store, accept credit cards and more. Now works w/ QuickBooks&#174;!";
          break;
        case CrossSellConfigProductId.COLD_FUSION:
          IProductView coldFusion = _products.NewProductView(_products.GetProduct(ProductIds.ColdFusion));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/jump_pages/coldfusion.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "ColdFusion MX7";
          priceText = string.Format(FORMAT_PRICETEXT, _currency.PriceText(coldFusion.MonthlyCurrentPrice, false));
          productDescription = "Build powerful Internet applications with ease! (3 month min. purchase)";
          break;
        case CrossSellConfigProductId.TRAFFIC_FACTS:
          IProductView trafficFacts = _products.NewProductView(_products.GetProduct(ProductIds.TrafficStats_D));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/hosting/stats_Landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Site Analytics";
          priceText = string.Format(FORMAT_PRICETEXT, _currency.PriceText(trafficFacts.MonthlyCurrentPrice, false));
          productDescription = "Analyze Web site traffic and more! (3 month min. purchase)";
          break;
        case CrossSellConfigProductId.TRAFFICB:
          IProductView trafficBlazor_D = _products.NewProductView(_products.GetProduct(ProductIds.TrafficBlazor_D), false, 1,PriceRoundingType.RoundFractionsUpProperly,SavingsRoundingType.FloorSavingsProperly);
          IProductView trafficBlazor_1year = _products.NewProductView(_products.GetProduct(ProductIds.TrafficBlazor_1year), false, 1, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
          IProductView campaignBlazor_Tier1 = _products.NewProductView(_products.GetProduct(ProductIds.CampaignBlazer_Tier1), false, 1, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/traffic_blazer/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Traffic Blazer&#174; Deluxe";
          priceText = string.Format("Just {0}/yr.", _currency.PriceText(trafficBlazor_D.UnitPrice, false));
          int savingsPrice = (trafficBlazor_1year.UnitPrice.Price + campaignBlazor_Tier1.UnitPrice.Price) - trafficBlazor_D.UnitPrice.Price;
          if (savingsPrice > 0)
          {
            savingsText = string.Format("Save nearly {0}!", _currency.PriceText(new CurrencyPrice(savingsPrice, _currency.SelectedDisplayCurrencyInfo, CurrencyPriceType.Transactional), false));
          }
          productDescription = "Easily optimize your site for Google&#153;, Yahoo&#174; and other top search engines!";
          break;
        case CrossSellConfigProductId.EMAIL:
          IProductView email_D = _products.NewProductView(_products.GetProduct(ProductIds.Email_D));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/email.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Deluxe Email";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(email_D.YearlyCurrentPrice, false));
          productDescription = "Advertising-free and protected from fraud, spam & viruses";
          break;
        case CrossSellConfigProductId.OFF:
          IProductView off_D = _products.NewProductView(_products.GetProduct(ProductIds.OFF_D));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/email/vsdb_landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Online File Folder Deluxe";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(off_D.YearlyCurrentPrice, false));
          productDescription = "Access your files from any computer!";
          break;
        case CrossSellConfigProductId.FAXTHRU_EMAIL:
          IProductView fte_local_E_12_months = _products.NewProductView(_products.GetProduct(ProductIds.FTE_Local_E), false, 12, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/email/fte_landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Fax Thru Email Deluxe";
          IProductView fte_local_E = _products.NewProductView(_products.GetProduct(ProductIds.FTE_Local_E));
          decimal discount = 1;
          if (_orderData.ContextId != ContextIds.WildWestDomains)
          {
            discount = Math.Round(Convert.ToDecimal(fte_local_E_12_months.UnitPrice.Price) / Convert.ToDecimal(fte_local_E.UnitPrice.Price), 2);
          }
          if (discount < 1)
          {
            savingsText = string.Format("Save {0:G0}%!", Math.Round(((1-discount)*100),0));
          }
          productDescription = "Send & receive faxes whenever you have email/Internet access!";
          break;
        case CrossSellConfigProductId.EEM:
          IProductView eem_Tier1 = _products.NewProductView(_products.GetProduct(ProductIds.EEM_Tier1));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/blazers/cb_landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Express Email Marketing" + "&#174;";
          priceText = string.Format("Just {0}/mo!", _currency.PriceText(eem_Tier1.MonthlyCurrentPrice, false));
          productDescription = "Market your business the spam-free and legal way!";
          break;
        case CrossSellConfigProductId.GROUP_CALENDAR:
          IProductView ogc_D = _products.NewProductView(_products.GetProduct(ProductIds.OGC_D));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/calendar/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Calendar";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(ogc_D.YearlyCurrentPrice, false));
          productDescription = "View, edit & share your calendar - anytime, anywhere!";
          break;
        case CrossSellConfigProductId.SSL_TURBO:
          IProductView ssl_1year_turbo = _products.NewProductView(_products.GetProduct(ProductIds.SSL_1Year_Turbo));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/ssl/ssl.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Turbo SSL&#174; Secure Certificates";
          priceText = string.Format("Just {0}/yr", _currency.PriceText(ssl_1year_turbo.YearlyCurrentPrice, false));
          productDescription = "Secure your data and transactions!";
          break;
        case CrossSellConfigProductId.SSL_HIGH:
          IProductView ssl_1year_cert = _products.NewProductView(_products.GetProduct(ProductIds.SSL_1Year_Cert));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/ssl/ssl.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "High Assurance SSL Certificate";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(ssl_1year_cert.YearlyCurrentPrice, false));
          productDescription = "Full company validation & 256-bit encryption!";
          break;
        case CrossSellConfigProductId.CSITE:
          IProductView copyRight = _products.NewProductView(_products.GetProduct(ProductIds.Copyright));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/copyright/landing_choice.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "c-Site&#174;";
          priceText = string.Format("Just {0}!", _currency.PriceText(copyRight.UnitPrice, false));
          productDescription = "Protect your Web site, images, music and more with a federal copyright!";
          break;
        case CrossSellConfigProductId.DNA:
          IProductView dna = _products.NewProductView(_products.GetProduct(ProductIds.DNA_SubscriptionMonitorBundle));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/dna/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "The Domain Name Aftermarket";
          priceText = string.Format("Memberships Just {0}/yr!", _currency.PriceText(dna.UnitPrice, false));
          productDescription = "Your smart choice for buying/selling domains!";
          break;
        case CrossSellConfigProductId.PRO_RESELLER:
          IProductView resellerPro = _products.NewProductView(_products.GetProduct(ProductIds.ResellerPro));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/reseller/instant.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Pro Reseller";
          priceText = string.Format("Just {0}!", _currency.PriceText(resellerPro.UnitPrice, false));
          productDescription = "The best buy rates and products. Great for monetizers!";
          break;
        case CrossSellConfigProductId.SUPER_RESELLER:
          IProductView resellerSuper = _products.NewProductView(_products.GetProduct(ProductIds.ResellerSuper));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/reseller/super.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Super Reseller";
          priceText = string.Format("Just {0}!", _currency.PriceText(resellerSuper.UnitPrice, false));
          productDescription = "Multiple revenue streams selling Basic & Pro Reseller plans!";
          break;
        case CrossSellConfigProductId.MERCH_ACCT:
          int merchanAccountProductId;
          if ((_orderData.ContextId == ContextIds.GoDaddy) || (_orderData.ContextId == ContextIds.BlueRazor))
            merchanAccountProductId = ProductIds.MerchAcctStd;
          else
            merchanAccountProductId = ProductIds.MerchPQ;
          IProductView merchantAccount = _products.NewProductView(_products.GetProduct(merchanAccountProductId));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/ecommerce/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Merchant Accounts";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(merchantAccount.YearlyCurrentPrice, false));
          productDescription = "Accept credit cards on your site!";
          break;
        case CrossSellConfigProductId.PRIVATE_DOMAINS:
          IProductView privateDomains = _products.NewProductView(_products.GetProduct(ProductIds.DBP));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/dbp/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Private Registration";
          priceText = string.Format("Just {0}!", _currency.PriceText(privateDomains.UnitPrice, false));
          productDescription = "Keep your name, address, email & phone number private!";
          break;
        case CrossSellConfigProductId.DELUXE_WHOIS:
          IProductView whoIs = _products.NewProductView(_products.GetProduct(ProductIds.WhoIs));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/deluxe_whois/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Business Registration";
          priceText = string.Format("Just {0}/yr!", _currency.PriceText(whoIs.YearlyCurrentPrice, false));
          productDescription = "Put your vital advertising information online!";
          break;
        case CrossSellConfigProductId.APPRAISAL:
          IProductView appraisal = _products.NewProductView(_products.GetProduct(ProductIds.domainAlertDept));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/dna/appraisal.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Express Domain Appraisal";
          priceText = string.Format("Just {0}!", _currency.PriceText(appraisal.UnitPrice, false));
          productDescription = "Fast, affordable, expert domain name evaluations!";
          break;
        case CrossSellConfigProductId.LOGODESIGN:
          IProductView logoDesign = _products.NewProductView(_products.GetProduct(ProductIds.LogoDesign));
          productUrl = _links.GetUrl(LinkTypes.SiteRoot, "gdshop/logo/landing.asp", QueryParamMode.CommonParameters, true, queryStringArgs);
          productName = "Logo Design";
          priceText = string.Format("Just {0}!", _currency.PriceText(logoDesign.UnitPrice, false));
          productDescription = "Brand your business or organization name with a professional logo!";
          break;
        default:
          //do nothing
          break;
      }
      CrossSellProduct product = new CrossSellProduct(productId, productUrl, productName, priceText, productDescription, savingsText);

      return product;
    }

    private string GetSavingsText(IProductView productView, IProduct baseProduct)
    {
      const string FORMAT_SAVINGSTEXT = "Save {0}%!";
      string savingsText = null;
      productView.CalculateSavings(baseProduct);
      if (productView.SavingsPercentage > 0)
      {
        savingsText = string.Format(FORMAT_SAVINGSTEXT, productView.SavingsPercentage);
      }
      return savingsText;
    }
  }
}
