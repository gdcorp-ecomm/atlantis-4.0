using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCICannConfirm.Interface
{
  public class DCCICannConfirmResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    public string ResponseXml { get; set; }
    public bool IsSuccess { get; private set; }


    public DCCICannConfirmResponseData()
    {
      IsSuccess = true;
    }

    public DCCICannConfirmResponseData(string responseXml, AtlantisException exAtlantis)
    {
      ResponseXml = responseXml;
      _exception = exAtlantis;
      IsSuccess = (exAtlantis == null) ? true : false;
    }

    public DCCICannConfirmResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCICannConfirmResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
      IsSuccess = false;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return ResponseXml;
    }

  }
}
