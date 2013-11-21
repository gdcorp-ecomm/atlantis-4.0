
namespace Atlantis.Framework.ShopperValidator.Interface.RuleConstants
{
  public static class EngineRequestValues
  {
    public static int ShopperSearch { get; set; }
    public static int ValidateField { get; set; }
    public static int  AuthValidatePassword { get; set; }

    static EngineRequestValues()
    {
      ShopperSearch = 740;
      ValidateField = 507;
      AuthValidatePassword = 517;
    }
  }
}
