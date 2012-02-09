using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDUpdBandwidthOverProt.Interface
{
  public class HDVDUpdBandwidthOverProtResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HDVDUpdBandwidthOverProtResponseData(string xml)
    {

    }

     public HDVDUpdBandwidthOverProtResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDUpdBandwidthOverProtResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDUpdBandwidthOverProtResponseData",
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
