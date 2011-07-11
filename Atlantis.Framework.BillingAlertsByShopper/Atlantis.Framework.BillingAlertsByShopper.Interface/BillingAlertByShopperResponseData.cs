using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.BillingAlertsByShopper.Interface
{
  public class BillingAlertsByShopperResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private bool _success = false;
    private List<BillingAlert> _billingAlerts;
   
    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<BillingAlert> BillingAlerts
    {
      get { return _billingAlerts; }
    }

    public BillingAlertsByShopperResponseData()
    { }

    public BillingAlertsByShopperResponseData(List<BillingAlert> billingAlerts)
    {
      _billingAlerts = billingAlerts;
      _success = true;
    }

    public BillingAlertsByShopperResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public BillingAlertsByShopperResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "BillingAlertsByShopperResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      using (XmlWriter writer = XmlWriter.Create(sb))
      {
        writer.WriteStartElement("billingAlerts");

        foreach (BillingAlert alert in _billingAlerts)
        {
          writer.WriteStartElement("billingAlert");

          writer.WriteAttributeString("productGroupId", alert.ProductGroupId.ToString());
          writer.WriteAttributeString("billingFailureResourceId", alert.BillingFailureResourceId.ToString());
          writer.WriteAttributeString("expiringResourceId", alert.ExpiringResourceId.ToString());
          writer.WriteAttributeString("setupResourceId", alert.SetupResourceId.ToString());         

          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }

      return sb.ToString();      
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _billingAlerts = new List<BillingAlert>();

      if (!string.IsNullOrEmpty(sessionData))
      {
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(sessionData);
        XmlNodeList alertNodes = xdoc.SelectNodes("billingAlerts/billingAlert");
        if (alertNodes != null)
        {
          foreach (XmlNode node in alertNodes)
          {
            int productGroupId = Convert.ToInt32(node.Attributes["productGroupId"].Value);
            int billingFailureResourceId = Convert.ToInt32(node.Attributes["billingFailureResourceId"].Value);
            int expiringResourceId = Convert.ToInt32(node.Attributes["expiringResourceId"].Value);
            int setupResourceId = Convert.ToInt32(node.Attributes["setupResourceId"].Value);

            BillingAlert alert = new BillingAlert(productGroupId
              , billingFailureResourceId
              , setupResourceId
              , expiringResourceId);

            _billingAlerts.Add(alert);
          }
        }
      }
      _success = true;
    }

    #endregion
  }
}
