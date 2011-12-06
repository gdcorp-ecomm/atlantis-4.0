using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerNotInterested.Interface
{
  public class DBSSellerNotInterestedResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private string _returnData;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public DBSSellerNotInterestedResponseData(string returnData)
    {
      _returnData = returnData;
      _success = EvaluatedResponse(returnData);
    }

    public DBSSellerNotInterestedResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public DBSSellerNotInterestedResponseData(string returnData, RequestData oRequestData, Exception ex)
    {
      _returnData = returnData;
      _exAtlantis = new AtlantisException(oRequestData, "DBSSellerNotInterestedResponseData", ex.Message, string.Empty);
    }

    public string ReturnData
    {
      get
      {
        return _returnData;
      }
    }

    public bool EvaluatedResponse(string returnData)
    {
      bool rtn = false;
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(returnData);
      XmlNode node = doc.SelectSingleNode("//Result/Error");
      if (node == null)
      {
        rtn = true;
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
