
namespace Atlantis.Framework.DomainCheck.Interface
{
  public class DomainAttributes
  {
    int m_iAvailableCode;
    int m_iSyntaxCode;
    string m_sSyntaxDescription;
    bool m_wasTyped;

    public DomainAttributes(int iAvailableCode,
      int iSyntaxCode, string sSyntaxDescription, bool wasTyped)
    {
      m_iAvailableCode = iAvailableCode;
      m_iSyntaxCode = iSyntaxCode;
      m_sSyntaxDescription = sSyntaxDescription;
      m_wasTyped = wasTyped;
    }

    public int AvailableCode
    {
      get { return m_iAvailableCode; }
    }

    public int SyntaxCode
    {
      get { return m_iSyntaxCode; }
    }

    public string SyntaxDescription
    {
      get { return m_sSyntaxDescription; }
    }

    public bool WasTyped
    {
      get { return m_wasTyped; }
    }
  }
}

