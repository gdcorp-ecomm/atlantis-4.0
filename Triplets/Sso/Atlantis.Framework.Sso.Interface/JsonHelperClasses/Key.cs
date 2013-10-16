
namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class Key: AuthApiResponseBase
  {
    public KeyData data { get; set; }

    public Key()
    {
      data = new KeyData();
    }
  }
}
