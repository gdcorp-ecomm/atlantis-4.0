using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSGetDomainShopper.Interface
{
  public class DBSGetDomainShopperResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private string _returnData;

    private string _sellerShopperId;
    public string SellerShopperId { get { return _sellerShopperId; } }

    public DBSGetDomainShopperResponseData(string returnData)
    {
      _returnData = returnData;
      _sellerShopperId = ParseForSellerShopperId(returnData);
    }

    public DBSGetDomainShopperResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public DBSGetDomainShopperResponseData(string returnData, RequestData oRequestData, Exception ex)
    {
      _returnData = returnData;
      _exAtlantis = new AtlantisException(oRequestData, "DBSGetDomainShopperResponseData", ex.Message, string.Empty);
    }

    public string ReturnData
    {
      get
      {
        return _returnData;
      }
    }   
    
    public string ParseForSellerShopperId(string returnData)
    {
      string val = string.Empty;
      if (!string.IsNullOrEmpty(returnData))
      {
        val = returnData;
      }
      return val;
    } 

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion
  }

}
