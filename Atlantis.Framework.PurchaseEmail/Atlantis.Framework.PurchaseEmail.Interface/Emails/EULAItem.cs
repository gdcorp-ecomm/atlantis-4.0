
namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  public class EULAItem
  {
    string _productName;
    string _productInfoURL;
    string _legalAgreementURL;
    string _pageId = string.Empty;
    string _agrement_url_type = string.Empty;

    EULARuleType _eulaRuleType = EULARuleType.Unknown;
    EULAType _agreementType = EULAType.Legal;

    public string ProductName
    {
      get { return _productName; }
    }
    public string ProductInfoURL
    {
      get { return _productInfoURL; }
    }
    public string LegalAgreementURL
    {
      get { return _legalAgreementURL; }
      set { _legalAgreementURL = value; }
    }
    public EULAType AgreementType
    {
      get { return _agreementType; }
      set { _agreementType = value; }
    }

    public string PageId
    {
      get { return _pageId; }
      set { _pageId = value; }
    }

    public EULARuleType RuleType
    {
      get { return _eulaRuleType; }
      set { _eulaRuleType = value; }
    }

    public string Agreement_Url_Type
    {
      get { return _agrement_url_type; }
      set { _agrement_url_type = value; }
    }

    public EULAItem(string productName, string productInfoURL, string legalAgreementURL)
    {
      _productName = productName;
      _productInfoURL = productInfoURL;
      _legalAgreementURL = legalAgreementURL;
    }
  }
}
