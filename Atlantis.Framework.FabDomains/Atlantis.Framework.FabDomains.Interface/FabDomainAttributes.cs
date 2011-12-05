
namespace Atlantis.Framework.FabDomains.Interface
{
  public class FabDomainAttributes
  {
    /******************************************************************************/

    string m_sDomainName;
    string m_sOwnerType;
    float m_fPrice;
    float m_fWaterlinePrice;
    float m_fNegotiablePrice;
    float m_fCommissionPct;
    float m_fCommissionBonusPct;

    /******************************************************************************/

    public FabDomainAttributes(string sDomainName, string sOwnerType, float fPrice, float fWaterlinePrice,
      float fNegotiablePrice, float fCommissionPct, float fCommissionBonusPct)
    {
      m_sDomainName = sDomainName;
      m_sOwnerType = sOwnerType;
      m_fWaterlinePrice = fWaterlinePrice;
      m_fNegotiablePrice = fNegotiablePrice;
      m_fPrice = fPrice;
      m_fCommissionPct = fCommissionPct;
      m_fCommissionBonusPct = fCommissionBonusPct;
    }

    /******************************************************************************/

    public float Price
    {
      get { return m_fPrice; }
    }

    /******************************************************************************/

    public float CommissionPct
    {
      get { return m_fCommissionPct; }
    }

    /******************************************************************************/

    public float CommissionBonusPct
    {
      get { return m_fCommissionBonusPct; }
    }

    /******************************************************************************/

    public string DomainName
    {
      get { return m_sDomainName; }
    }

    /******************************************************************************/

    public string OwnerType
    {
      get { return m_sOwnerType; }
    }

    /******************************************************************************/

    public float WaterlinePrice
    {
      get { return m_fWaterlinePrice; }
    }

    /******************************************************************************/

    public float NegotiablePrice
    {
      get { return m_fNegotiablePrice; }
    }

    /******************************************************************************/

  }
}
