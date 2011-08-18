using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.MktgSetShopperCommDoubleOpt.Interface
{
  public class MktgSetShopperCommDoubleOptResponseData: IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;

    public MktgSetShopperCommDoubleOptResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;
      if (!string.IsNullOrEmpty(_responseXml))
      {
        XmlDocument _xmlDoc = new XmlDocument();
        _xmlDoc.LoadXml(_responseXml);
        string output = _xmlDoc.InnerText;
        result = output == "SUCCESS" ? true : false;
      }
      return result;
    }

    public MktgSetShopperCommDoubleOptResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public MktgSetShopperCommDoubleOptResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgSetShopperCommDoubleOptResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
