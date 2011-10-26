
namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EULAItem
  {
    string _productName;
    string _productInfoURL;
    string _legalAgreementURL;
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
    }
    public EULAType AgreementType
    {
      get { return _agreementType; }
      set { _agreementType = value; }
    }

    public EULAItem(string productName, string productInfoURL, string legalAgreementURL)
    {
      _productName = productName;
      _productInfoURL = productInfoURL;
      _legalAgreementURL = legalAgreementURL;
    }
  }
}
