using System;
using System.Data;
using System.IO;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.AuctionsAreaBySection.Interface
{
  public class AuctionsAreaBySectionResponseData : IResponseData, ISessionSerializableResponse
  {
    DataSet _responseDataSet = null;
    AtlantisException _exAtlantis = null;

    public DataSet Response { get { return _responseDataSet; } }

    public bool IsSuccess { get { return _exAtlantis == null; } }

    public AuctionsAreaBySectionResponseData()
    { }

    public AuctionsAreaBySectionResponseData(DataSet response)
    {
      _responseDataSet = response;
    }

    public AuctionsAreaBySectionResponseData(DataSet response, AtlantisException exAtlantis)
    {
      _responseDataSet = response;
      _exAtlantis = exAtlantis;
    }

    public AuctionsAreaBySectionResponseData(DataSet response, RequestData requestData, Exception ex)
    {
      _responseDataSet = response;
      _exAtlantis = new AtlantisException(requestData, 
                                          "AuctionsAreaBySectionResponseData", 
                                          ex.Message.ToString(), 
                                          requestData.ToString());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseDataSet.GetXml();
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        StringReader sr = new StringReader(sessionData);
        DataSet ds = new DataSet();
        ds.ReadXml(sr);
        _responseDataSet = ds;
      }
    }
    #endregion
  }
}
