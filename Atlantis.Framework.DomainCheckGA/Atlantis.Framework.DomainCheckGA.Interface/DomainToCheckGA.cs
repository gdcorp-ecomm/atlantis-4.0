
namespace Atlantis.Framework.DomainCheckGA.Interface
{
  public class DomainToCheckGA
  {
    private string _domainName = string.Empty;
    private string _languageTag = string.Empty;
    private bool _wasTyped = false;

    public DomainToCheckGA(string domainName, bool wasTyped)
    {
      _domainName = domainName.ToUpperInvariant();
      _wasTyped = wasTyped;
    }

    public DomainToCheckGA(string domainName, bool wasTyped, string languageTag)
      : this(domainName, wasTyped)
    {
      _languageTag = languageTag;
    }

    public string DomainName
    {
      get { return _domainName; }
    }

    public string LanguageTag
    {
      get { return _languageTag; }
    }

    public bool WasTyped
    {
      get { return _wasTyped; }
    }
  }
}
