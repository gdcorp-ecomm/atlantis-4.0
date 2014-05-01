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
      _exception = new AtlantisException("PayeeProfilesListResponseData", 0, exception.Message, requestData.ToXML());
    }

    private void ProcessResponseXml(string xml)
    {
      try
      {
        XDocument xDoc = XDocument.Parse(xml);
        XElement responseElem = xDoc.Element("RESPONSE");
        XElement msg = responseElem != null ? responseElem.Element("MESSAGE") : null;
        if (msg != null)
        {
          if (msg.Value.ToLowerInvariant() != "success")
          {
            _exception = new AtlantisException("PayeeProfilesListResponseData::ProcessResponseXml", 0,
              "Payee GetAccountListForShopper Unsuccessful", xml);
          }
          else
          {
            XElement accounts = responseElem.Element("ACCOUNTS");
            if (accounts != null)
            {
              foreach (XElement account in accounts.Elements())
              {
                XElement capId = account.Element("capID");
                XElement friendlyName = account.Element("friendlyName");
                XElement isPayable = account.Element("isPayable");
                XElement nonPayableReason = account.Element("nonPayableReason");

                PayeeProfileListData ppld = new PayeeProfileListData(GetValueOrEmpty(capId), GetValueOrEmpty(friendlyName), GetValueOrEmpty(isPayable), GetValueOrEmpty(nonPayableReason));
                PayeeList.Add(ppld);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException("PayeeProfilesListResponseData::ProcessResponseXml", 0, ex.Message, xml);
      }
    }

    private string GetValueOrEmpty(XElement elem)
    {
      return elem != null ? elem.Value : string.Empty;
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
