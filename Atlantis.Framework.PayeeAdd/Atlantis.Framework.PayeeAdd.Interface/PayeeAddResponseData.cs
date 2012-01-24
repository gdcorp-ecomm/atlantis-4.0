using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeAdd.Interface
{
  public class PayeeAddResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public string CapId { get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public PayeeAddResponseData(string responseXml)
    {
      ProcessResponseXml(responseXml);
    }

     public PayeeAddResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PayeeAddResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData
        , "PayeeAddResponseData"
        , ex.Message
        , requestData.ToXML());
    }

    private void ProcessResponseXml(string responseXml)
    {
      try
      {
        XDocument xDoc = XDocument.Parse(responseXml);
        XElement success = xDoc.Element("RESPONSE").Element("MESSAGE");
        if (success.Value.ToLowerInvariant() == "success")
        {
          XElement capId = xDoc.Element("RESPONSE").Element("ACCOUNT").Element("capID");
          if (capId != null)
          {
            CapId = capId.Value;
          }
        }
        else
        {
          _exception = new AtlantisException("PayeeAddResponseData::ProcessResponseXml", "0", "Payee Add Unsuccessful", responseXml, null, null);
        }
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException("PayeeAddResponseData::ProcessResponseXml", "0", ex.Message, responseXml, null, null);
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
