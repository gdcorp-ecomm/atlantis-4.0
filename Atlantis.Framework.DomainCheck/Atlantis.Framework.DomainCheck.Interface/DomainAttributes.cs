
namespace Atlantis.Framework.DomainCheck.Interface
{
  public class DomainAttributes
  {
    private readonly int _availableCode;
    private readonly int _syntaxCode;
    private readonly string _syntaxDescription;
    private readonly bool _wasTyped;
    private readonly string _punyCode;
    private readonly string _idnScript;
    private readonly string _languageId;

    public DomainAttributes(int availableCode, int syntaxCode, string syntaxDescription, bool wasTyped, string punyCode, string idnScript, string languageId)
    {
      _availableCode = availableCode;
      _syntaxCode = syntaxCode;
      _syntaxDescription = syntaxDescription;
      _wasTyped = wasTyped;
      _punyCode = punyCode;
      _idnScript = idnScript;
      _languageId = languageId;
    }

    public int AvailableCode
    {
      get { return _availableCode; }
    }

    public int SyntaxCode
    {
      get { return _syntaxCode; }
    }

    public string SyntaxDescription
    {
      get { return _syntaxDescription; }
    }

    public bool WasTyped
    {
      get { return _wasTyped; }
    }

    public string PunyCode
    {
      get { return _punyCode; }
    }

    public string LanguageId
    {
      get { return _languageId; }
    }

    public string IdnScript
    {
      get { return _idnScript; }
    }
  }
}

