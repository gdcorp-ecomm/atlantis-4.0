
namespace Atlantis.Framework.BuyDomains.Interface
{
  public class BuyDomainAttributes
  {
    /******************************************************************************/

    string m_sDomainName;
    bool m_bIsFastTransfer;
    float m_fPrice;

    /******************************************************************************/

    public BuyDomainAttributes(string sDomainName, float fPrice, bool bIsFastTransfer)
    {
      m_sDomainName = sDomainName;
      m_fPrice = fPrice;
      m_bIsFastTransfer = bIsFastTransfer;
    }

    /******************************************************************************/

    public bool IsFastTransfer
    {
      get { return m_bIsFastTransfer; }
    }

    /******************************************************************************/

    public float Price
    {
      get { return m_fPrice; }
    }

    /******************************************************************************/

    public string DomainName
    {
      get { return m_sDomainName; }
    }

    /******************************************************************************/
  }
}
