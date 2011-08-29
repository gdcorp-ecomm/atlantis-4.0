
namespace Atlantis.Framework.NameserverCheck.Interface
{
  public class NameserverAttributes
  {
    private int _availableCode;
    public int AvailableCode
    {
      get { return _availableCode; }
    }

    private int _syntaxCode;
    public int SyntaxCode
    {
      get { return _syntaxCode; }
    }

    private string _syntaxDescription;
    public string SyntaxDescription
    {
      get { return _syntaxDescription; }
    }

    public NameserverAttributes(int availableCode, int syntaxCode, string syntaxDescription)
    {
      _availableCode = availableCode;
      _syntaxCode = syntaxCode;
      _syntaxDescription = syntaxDescription;
    }
  }
}
