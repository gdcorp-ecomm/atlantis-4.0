
namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class RawTokenData
  {
    public string Header;
    public string Payload;
    public string Signature;

    public RawTokenData()
    {
      Header = string.Empty;
      Payload = string.Empty;
      Signature = string.Empty;
    }

    public override string ToString()
    {
      return string.Concat(Header, ".", Payload, ".", Signature);
    }
  }
}
