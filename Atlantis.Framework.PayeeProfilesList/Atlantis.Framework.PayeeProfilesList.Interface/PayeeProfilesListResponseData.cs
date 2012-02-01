using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeProfilesList.Interface
{
  public class PayeeProfilesListResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public List<PayeeProfileListData> PayeeList { get; private set; }

    public PayeeProfilesListResponseData(string xml)
    {
      PayeeList = new List<PayeeProfileListData>();
      _resultXML = xml;
      ProcessResponseXml(xml);
    }

     public PayeeProfilesListResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PayeeProfilesListResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "PayeeProfilesListResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    private void ProcessResponseXml(string xml)
    {
      try
      {
        XDocument xDoc = XDocument.Parse(xml);
        XElement msg = xDoc.Element("RESPONSE").Element("MESSAGE");
        if (msg.Value.ToLowerInvariant() != "success")
        {
          _exception = new AtlantisException("PayeeProfilesListResponseData::ProcessResponseXml", "0", "Payee GetAccountListForShopper Unsuccessful", xml, null, null);
        }
        else
        {
          XElement accounts = xDoc.Element("RESPONSE").Element("ACCOUNTS");
          if (accounts != null)
          {
            foreach (XElement account in accounts.Elements())
            {
              XElement capId = account.Element("capID");
              XElement friendlyName = account.Element("friendlyName");

              if (capId != null && friendlyName != null)
              {
                PayeeProfileListData ppld = new PayeeProfileListData(capId.Value, friendlyName.Value);
                PayeeList.Add(ppld);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException("PayeeProfilesListResponseData::ProcessResponseXml", "0", ex.Message, xml, null, null);
      }
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
