using Atlantis.Framework.Interface;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class ValidateNonOrderRequestData : RequestData
  {
    private static Regex _validCharsEx = new Regex("^[0-9A-Z]*$", RegexOptions.Singleline | RegexOptions.Compiled);
    const int _MAXLENGTH = 20;

    public int PrivateLabelId { get; private set; }
    public string PromoCode { get; private set; }

    public ValidateNonOrderRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string promoCode)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      PromoCode = PreValidatePromoCode(promoCode);
    }

    private string PreValidatePromoCode(string promoCode)
    {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(promoCode))
      {
        promoCode = promoCode.ToUpperInvariant();
        if (_validCharsEx.IsMatch(promoCode))
        {
          result = promoCode;
          if (result.Length > _MAXLENGTH)
          {
            result = result.Substring(0, _MAXLENGTH);
          }
        }
      }
      return result;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(PrivateLabelId.ToString(), ".", PromoCode);
    }

    public override string ToXML()
    {
      XElement element = new XElement("ValidateNonOrderRequestData");
      element.Add(
        new XAttribute("privatelabelid", PrivateLabelId.ToString()),
        new XAttribute("promocode", PromoCode));

      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
