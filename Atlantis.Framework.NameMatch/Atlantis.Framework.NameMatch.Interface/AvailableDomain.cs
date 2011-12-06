
namespace Atlantis.Framework.NameMatch.Interface
{
  public class AvailableDomain : NameMatchDomain
  {
    private string _anchorWord;
    public string AnchorWord
    {
      get
      {
        return this._anchorWord;
      }
      set
      {
        this._anchorWord = value;
      }
    }

    private string _searchMethod;
    public string SearchMethod
    {
      get
      {
        return this._searchMethod;
      }
      set
      {
        this._searchMethod = value;
      }
    }

    private bool _availCheckPerformed;
    public bool AvailCheckPerformed
    {
      get
      {
        return this._availCheckPerformed;
      }
      set
      {
        this._availCheckPerformed = value;
      }
    }

    private string _domainAvailable;
    public string DomainAvailable
    {
      get
      {
        return this._domainAvailable;
      }
      set
      {
        this._domainAvailable = value;
      }
    }

    private string _fullDomainName;
    public string FullDomainName
    {
      get
      {
        return this._fullDomainName;
      }
      set
      {
        this._fullDomainName = value;
      }
    }

    private string _tld;
    public string TLD
    {
      get
      {
        return this._tld;
      }
      set
      {
        this._tld = value;
      }
    }

    private string _sld;
    public string SLD
    {
      get
      {
        return this._sld;
      }
      set
      {
        this._sld = value;
      }
    }

  }
}
