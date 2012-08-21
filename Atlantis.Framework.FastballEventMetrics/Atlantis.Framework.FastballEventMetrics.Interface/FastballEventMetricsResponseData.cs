using System;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballEventMetrics.Interface
{
  public class FastballEventMetricsResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private object _stateObject;


    public void setStateObject(object o)
    {
      this._stateObject = o;
    }

    public object StateObject { get { return _stateObject; } }


    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public string ResponseXML
    {
      get
      {
        return _resultXML;
      }
    }

    public FastballEventMetricsResponseData(XmlDocument responseDoc)
    {
      this._success = true;
      if (responseDoc != null)
      {
        this._resultXML = responseDoc.OuterXml;
      }
     
    }

    public FastballEventMetricsResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;      
    }

    public FastballEventMetricsResponseData(string responseXML, AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      this._resultXML = responseXML;
    }

    public FastballEventMetricsResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "FastballEventMetricsResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
