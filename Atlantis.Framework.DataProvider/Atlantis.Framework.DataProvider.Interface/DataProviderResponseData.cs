using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataProvider.Interface
{
  public class DataProviderResponseData : IResponseData
  {
    private Dictionary<string, object> _outputParameters = new Dictionary<string, object>();
    private string m_sResponseXML = string.Empty;
    private AtlantisException m_ex;
    private object m_ResponseObj;

    public DataProviderResponseData(object response, Dictionary<string, object> outputParameters)
    {
      m_ex = null;
      m_ResponseObj = response;
      if (outputParameters != null)
        _outputParameters = new Dictionary<string, object>(outputParameters);
    }

    public DataProviderResponseData(object response, AtlantisException exAtlantis)
    {
      m_ResponseObj = response;
      m_ex = exAtlantis;
    }

    public DataProviderResponseData(object response, RequestData oRequestData, Exception ex)
    {
      m_ResponseObj = response;
      m_ex = new AtlantisException(oRequestData,
                                   "DataProviderResponseData",
                                   ex.Message,
                                   string.Empty);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return m_sResponseXML;
    }

    public object GetResponseObject()
    {
      return m_ResponseObj;
    }

    public AtlantisException GetException()
    {
      return m_ex;
    }

    public object GetOutputParameter(string parameterName)
    {
      object result = null;
      _outputParameters.TryGetValue(parameterName, out result);
      return result;
    }

    #endregion

  }
}
