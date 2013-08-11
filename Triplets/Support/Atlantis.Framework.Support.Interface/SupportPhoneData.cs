namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneData : ISupportPhoneData
  {
    public string Number { get; private set; }
    public bool IsInternational { get; private set; }

    public SupportPhoneData(string number, bool isInternational)
    {
      Number = number ?? string.Empty;
      IsInternational = isInternational;
    }

    public SupportPhoneData(string number)
    {
      Number = number ?? string.Empty;
      IsInternational = false;
    }
  }
}
