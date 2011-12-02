
namespace Atlantis.Framework.Backorder.Interface
{
  public class BackorderDomain
  {
    /******************************************************************************/

    string m_sFullName;
    int m_iResultCode;
    
    /******************************************************************************/

    public BackorderDomain(string sFullName, int iResultCode)
    {
      m_sFullName = sFullName;
      m_iResultCode = iResultCode;
    }

    /******************************************************************************/

    public string FullName
    {
      get { return m_sFullName; }
    }

    /******************************************************************************/


    /*
     *  Result code defintions
        1   = Can be backordered
        2   = Cannot be backordered
        3   = Cannot be backordered - expired domain auction open
        -1  = catastrophic failure
     */
    public int ResultCode
    {
      get { return m_iResultCode; }
    }

    /******************************************************************************/
  }
}
