
namespace Atlantis.Framework.Providers.Shopper.Interface
{
  public enum ShopperUpdateResultType
  {
    Success,
    UnknownError,
    ShopperNotFound,
    InvalidShopperXml,
    InvalidRequestField,
    InvalidShopperId,
    CountryMarketIdNotCompatible,
    LoginNameAlreadyExists,
    LoginNameNumericShouldBeCustomerNumber,
    PasswordUnacceptable,
    PinUnacceptable,
    HintMatchesPassword
  }
}
