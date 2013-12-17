namespace Atlantis.Framework.DomainContactValidation.Interface
{
  public class DomainContactValidationError
  {
    public string Attribute { get; set; }
    public int Code { get; set; }
    public string Description { get; set; }
    public string DisplayString { get; set; }
    public string Tld { get; set; }
    public int ContactType { get; set; }

    public DomainContactValidationError(string attribute, int code, string description, string displayString, 
                                        string tld, int contactType)
    {
      Attribute = attribute;
      Code = code;
      Description = description;
      DisplayString = displayString;
      Tld = tld;
      ContactType = contactType;
    }
  }
}
