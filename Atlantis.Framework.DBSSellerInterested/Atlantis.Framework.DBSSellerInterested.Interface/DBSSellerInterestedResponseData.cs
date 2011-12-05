using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerInterested.Interface
{
  public class DBSSellerInterestedResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private string _returnData;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public DBSSellerInterestedResponseData(string returnData)
    {
      _returnData = returnData;
      _success = EvaluatedResponse(returnData);
    }

    public DBSSellerInterestedResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public DBSSellerInterestedResponseData(string returnData, RequestData oRequestData, Exception ex)
    {
      _returnData = returnData;
      _exAtlantis = new AtlantisException(oRequestData, "DBSSellerInterestedResponseData", ex.Message, string.Empty);
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
      XmlNode lnode = doc.SelectSingleNode("//legalStatusResult");
      XmlNode dnode = doc.SelectSingleNode("//dbsStatusResult");
      if ((lnode != null) && (dnode != null))
      {
        if ((lnode.InnerText.ToUpper() == "SUCCESS") && (dnode.InnerText.ToUpper() == "SUCCESS"))
        {
          rtn = true;
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
