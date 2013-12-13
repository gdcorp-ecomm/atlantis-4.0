namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeResponse
  {
    public string NameWithoutExtension { get; set; }
    public string Tld { get; set; }
    public bool TrusteeEnabled { get; set; }
    public bool LocalPresence { get; set; }
    public string TuiFormType { get; set; }
    public string VendorId { get; set; }
    public string Error { get; set; }
  }
}
