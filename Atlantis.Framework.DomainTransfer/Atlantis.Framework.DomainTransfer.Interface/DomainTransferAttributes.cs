using System;

namespace Atlantis.Framework.DomainTransfer.Interface
{
  public class DomainTransferAttributes
  {
    private int _databaseCode;
    private int _availableCode;
    private int _syntaxCode;
    private string _syntaxDescription;
    private int _whoIsCode;
    private string _whoIsDescription;
    private string _whoIsStatus;
    private string _whoIsRegistrar;
    private DateTime _whoIsExpiration = DateTime.MinValue;

    public DomainTransferAttributes(int availableCode, int syntaxCode, string syntaxDescription, int databaseCode,
      int whoIsCode, string whoIsDescription, string whoIsStatus, string whoIsRegistrar, DateTime whoIsExpiration)
    {
      _availableCode = availableCode;
      _syntaxCode = syntaxCode;
      _syntaxDescription = syntaxDescription;
      _databaseCode = databaseCode;
      _whoIsCode = whoIsCode;
      _whoIsDescription = whoIsDescription;
      _whoIsRegistrar = whoIsRegistrar;
      _whoIsStatus = whoIsStatus;
      _whoIsExpiration = whoIsExpiration;
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

    public int WhoIsCode
    {
      get { return _whoIsCode; }
    }

    public string WhoIsDescription
    {
      get { return _whoIsDescription; }
    }

    public string WhoIsRegistrar
    {
      get { return _whoIsRegistrar; }
    }

    public string WhoIsStatus
    {
      get { return _whoIsStatus; }
    }

    public DateTime WhoIsExpiration
    {
      get { return _whoIsExpiration; }
    }

    public int DatabaseCode
    {
      get { return _databaseCode; }
    }

  }
}
