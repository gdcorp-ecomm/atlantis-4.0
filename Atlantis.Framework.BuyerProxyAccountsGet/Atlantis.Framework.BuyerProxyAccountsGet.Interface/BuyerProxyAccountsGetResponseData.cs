using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProxyAccountsGet.Interface
{
  public class BuyerProxyAccountsGetResponseData : IResponseData
  {
    private AtlantisException _atlException;
    private DataSet DBPAccounts { get; set; }
    public bool IsSuccess { get; private set;} 
   
    public BuyerProxyAccountsGetResponseData(DataSet _dbpAccounts)
    {
      IsSuccess = true;
      DBPAccounts = _dbpAccounts;
    }

    public BuyerProxyAccountsGetResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProxyAccountsGetResponseData", ex.Message, string.Empty);
    }

    private List<string> _getDBPShopperIdList;
    public List<string> GetDBPShopperIdList
    {
      get
      {
        if (_getDBPShopperIdList == null)
        {
          List<string> shopperList = new List<string>();
          DataTable dpbTableSet = null;
          if (DBPAccounts != null)
          {
            dpbTableSet = DBPAccounts.Tables[0];
            if (dpbTableSet != null && dpbTableSet.Rows.Count > 0)
            {
              foreach (DataRow dr in dpbTableSet.Rows)
              {
                string shopperId = dr["dbp_shopper_id"].ToString();
                shopperList.Add(shopperId);
              }
            }
          }
          _getDBPShopperIdList = shopperList;
        }
        return _getDBPShopperIdList;
      }
    }
    
    #region Implementation of IResponseData

    public string ToXML()
    {
      string result = string.Empty;
      if (_getDBPShopperIdList.Count > 0)
      {
        result = _getDBPShopperIdList.ToString();
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    #endregion
  }

}
