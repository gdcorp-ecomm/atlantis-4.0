
namespace Atlantis.Framework.NameMatch.Interface
{
  public class NameMatchDomain : Name
  {
    private string _extension;
    public string Extension
    {
      get
      {
        return this._extension;
      }
      set
      {
        this._extension = value;
      }
    }

    private string _domainName;
    public string DomainName
    {
      get
      {
        return this._domainName;
      }
      set
      {
        this._domainName = value;
      }
    }
  }
}
