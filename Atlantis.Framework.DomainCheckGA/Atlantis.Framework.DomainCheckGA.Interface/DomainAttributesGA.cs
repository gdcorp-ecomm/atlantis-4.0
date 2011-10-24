
namespace Atlantis.Framework.DomainCheckGA.Interface
{
  public class DomainAttributesGA
  {
    int m_iAvailableCode;
    int m_iSyntaxCode;
    string m_sSyntaxDescription;

    public DomainAttributesGA(int iAvailableCode,
      int iSyntaxCode, string sSyntaxDescription)
    {
      m_iAvailableCode = iAvailableCode;
      m_iSyntaxCode = iSyntaxCode;
      m_sSyntaxDescription = sSyntaxDescription;
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

  }
}

