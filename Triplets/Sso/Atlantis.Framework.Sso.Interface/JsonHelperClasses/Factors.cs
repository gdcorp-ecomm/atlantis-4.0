namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class Factors
  {
    public string k_pw { get; set; }
    public string p_sms { get; set; }

    public Factors()
    {
      k_pw = string.Empty;
      p_sms = string.Empty;
    }
  }
}