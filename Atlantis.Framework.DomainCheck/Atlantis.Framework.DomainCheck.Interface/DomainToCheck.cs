namespace Atlantis.Framework.DomainCheck.Interface
{
  public class DomainToCheck
  {
    private string _domainName = string.Empty;
    private string _languageTag = string.Empty;
    private string _typedDomainName = string.Empty;
    private string _tldChoice = string.Empty;
    private string _splitValue = string.Empty;
    private bool _wasTyped;
    private bool _wasTldSelected;
    private bool _specialCharsRemoved;

    public DomainToCheck(string domainName, bool wasTyped)
    {
      _domainName = domainName;
      _wasTyped = wasTyped;
    }

    public DomainToCheck(string domainName, bool wasTyped, string languageTag)
      : this(domainName, wasTyped)
    {
      _languageTag = languageTag;
    }

    public DomainToCheck(string domainName, bool wasTyped, string typedDomainName, string tldChoice, bool wasTldSelected, bool specialCharsRemoved, string splitValue) 
      : this (domainName, wasTyped)
    {
      _typedDomainName = typedDomainName;
      _tldChoice = tldChoice;
      _wasTldSelected = wasTldSelected;
      _specialCharsRemoved = specialCharsRemoved;
      _splitValue = splitValue;
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

    public bool WasTldSelected
    {
      get { return _wasTldSelected; }
    }

    public bool SpecialCharsRemoved
    {
      get { return _specialCharsRemoved; }
    }

    public string TypedDomainName
    {
      get { return _typedDomainName; }
    }

    public string TldChoice
    {
      get { return _tldChoice; }
    }

    public string SplitValue
    {
      get { return _splitValue; }
    }
  }
}