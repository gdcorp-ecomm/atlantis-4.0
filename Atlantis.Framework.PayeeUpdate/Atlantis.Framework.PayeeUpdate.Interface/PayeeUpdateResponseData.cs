using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeUpdate.Interface
{
  public class PayeeUpdateResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public PayeeUpdateResponseData(string responseXml)
    {
      ProcessResponseXml(responseXml);
    }

    public PayeeUpdateResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PayeeUpdateResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "PayeeUpdateResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    private void ProcessResponseXml(string responseXml)
    {
      try
      {
        XDocument xDoc = XDocument.Parse(responseXml);
        XElement success = xDoc.Element("RESPONSE").Element("MESSAGE");
        if (success.Value.ToLowerInvariant() != "success")
        {
          _exception = new AtlantisException("PayeeUpdateResponseData::ProcessResponseXml", "0", "Payee Update Unsuccessful", responseXml, null, null);
        }
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException("PayeeUpdateResponseData::ProcessResponseXml", "0", ex.Message, responseXml, null, null);
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
