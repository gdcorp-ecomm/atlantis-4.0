using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCICannConfirm.Interface;
using System.Xml;

namespace Atlantis.Framework.DCCICannConfirm.Impl
{
  public class DCCICannConfirmRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCICannConfirmResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        using (ICannDsweb.httpsBinding_IRegICANNConfirmWebSvc icannWeb = new ICannDsweb.httpsBinding_IRegICANNConfirmWebSvc())
        {
          DCCICannConfirmRequestData oRequest = (DCCICannConfirmRequestData)oRequestData;
          icannWeb.Url = ((WsConfigElement)oConfig).WSURL;
          icannWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          responseXml = icannWeb.GetComplianceEmail(oRequest.ToXML());
          string searchXml = GetSearchXmlFromComplianceXml(responseXml, oRequest);
          string searchResponseXml = icannWeb.GetIcannDomainData(searchXml);
          responseData = new DCCICannConfirmResponseData(searchResponseXml, null);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCICannConfirmResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCICannConfirmResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    private string GetSearchXmlFromComplianceXml(string responseXml, DCCICannConfirmRequestData oRequest)
    {
      //sample response xml
      //<success ts="10/11/2012 11:11:53 AM" server="G1TWDSWEB01" id="2dc47023-8e57-4d41-a265-5774bea37b54">
      //<emails guid="292fb49f-66e1-47a3-892c-30528c33c2f3"><email shopper_id="128874" toaddress="cgregory@godaddy.com" currentcampaign="1" privatelabelid="1" recipientname="WebHosting Pods" istestdata="1" isdbp="False" /></emails>
      //</success>
      if (!String.IsNullOrEmpty(responseXml))
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(responseXml);
        XmlNodeList emails = doc.SelectNodes("//emails/email");
        if (emails != null && emails.Count > 0)
        {
          StringBuilder sbRequest = new StringBuilder();
          XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

          //<request requestedby="test"><search shopperid="84495" adminemail="apassos@godaddy.com" recipientname="Matt Kelso" istest="1"/></request>
          xtwRequest.WriteStartElement("request");
          xtwRequest.WriteAttributeString("requestedby", oRequest.AppName);
          foreach (XmlNode email in emails)
          {
            xtwRequest.WriteStartElement("search");
            xtwRequest.WriteAttributeString("shopperid", email.Attributes["shopper_id"].Value);
            xtwRequest.WriteAttributeString("adminemail", email.Attributes["toaddress"].Value);
            xtwRequest.WriteAttributeString("recipientname", email.Attributes["recipientname"].Value);
            if (oRequest.Quantity > 0)
            {
              xtwRequest.WriteAttributeString("quantity", oRequest.Quantity.ToString());
            }
            if (oRequest.BoundaryRow.HasValue)
            {
              xtwRequest.WriteAttributeString("boundaryrow", oRequest.BoundaryRow.Value.ToString());
            }
            if (oRequest.PagingDirectionForward.HasValue)
            {
              xtwRequest.WriteAttributeString("pagingdirection", oRequest.PagingDirectionForward.Value ? "FORWARD" : "BACKWARD");
            }
            if (oRequest.SortDirectionAscending.HasValue)
            {
              xtwRequest.WriteAttributeString("sortdirection", oRequest.SortDirectionAscending.Value ? "ASC" : "DESC");
            }
            if (oRequest.RepeatRow.HasValue)
            {
              xtwRequest.WriteAttributeString("repeatrow", oRequest.RepeatRow.Value ? "1" : "0");
            }
            if (email.Attributes["istestdata"] != null)
            {
              xtwRequest.WriteAttributeString("istest", (email.Attributes["istestdata"].Value == "1" ? "1" : "0"));
            }
            xtwRequest.WriteEndElement();
          }
          xtwRequest.WriteEndElement();

          return sbRequest.ToString();
        }
      }
      return String.Empty;
    }
  }

}
