using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSCreateTdnamAuction.Interface
{
  public class DBSCreateTdnamAuctionResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private string _returnData;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    private string _error;
    public string Error { get { return _error; } }

    public DBSCreateTdnamAuctionResponseData(string returnData)
    {
      _returnData = returnData;
      _success = EvaluatedResponse(returnData, out _error);
    }

    public DBSCreateTdnamAuctionResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public DBSCreateTdnamAuctionResponseData(string returnData, RequestData oRequestData, Exception ex)
    {
      _returnData = returnData;
      _exAtlantis = new AtlantisException(oRequestData, "DBSCreateTdnamAuctionResponseData", ex.Message, string.Empty);
    }

    public string ReturnData
    {
      get
      {
        return _returnData;
      }
    }   
    
    public bool EvaluatedResponse(string returnData, out string _error)
    {
      bool rtn = false;
      _error = string.Empty;
      
      if (!string.IsNullOrEmpty(returnData))
      {
        rtn = true;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(returnData);
        //check the Error node but at this time do not present the text, they are not all customer friendly
        XmlNode enode = doc.SelectSingleNode("//Result/Error");
        if (enode != null)
        {
          _error = enode.InnerText;
        }
      }
      return rtn;
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
