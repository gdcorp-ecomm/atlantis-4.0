using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptIn.Interface
{
  public class EEMResellerOptInRequestData : RequestData
  { 
    public EEMResellerOptInRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, int emailTypeId, string emailAddress, string shopperFirstName, string shopperLastName) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _privateLabelId = privateLabelId;
      _emailTypeId = emailTypeId;
      _emailAddress = emailAddress;
      _shopperFirstName = shopperFirstName;
      _shopperLastName = shopperLastName;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    #region Properties

    private int _privateLabelId;
    public int PrivateLabelId
    {
      get { return _privateLabelId; }
      set { _privateLabelId = value; }
    }

    private int _emailTypeId;
    public int EmailTypeId
    {
      get { return _emailTypeId; }
      set { _emailTypeId = value; }
    }

    private string _emailAddress;
    public string EmailAddress
    {
      get { return _emailAddress; }
      set { _emailAddress = value; }
    }

    private string _shopperFirstName;
    public string ShopperFirstName
    {
      get { return _shopperFirstName; }
      set { _shopperFirstName = value; }
    }

    private string _shopperLastName;
    public string ShopperLastName
    {
      get { return _shopperLastName; }
      set { _shopperLastName = value; }
    }
    #endregion
 

    #region Overrides of RequestData

    public override string ToXML()
    {
        XmlDocument document = new XmlDocument();
        document.AppendChild(document.CreateElement("Subscriber"));
        XmlNode newChild = document.CreateElement("reseller_private_label_id");
        newChild.InnerText = _privateLabelId.ToString();
        if (document.DocumentElement != null)
        {
          document.DocumentElement.AppendChild(newChild);
          XmlNode node2 = document.CreateElement("email_format");
          node2.InnerText = _emailTypeId.ToString();
          document.DocumentElement.AppendChild(node2);
          XmlNode node3 = document.CreateElement("email");
          node3.InnerText = _emailAddress;
          document.DocumentElement.AppendChild(node3);
          XmlNode node4 = document.CreateElement("first_name");
          node4.InnerText = _shopperFirstName;
          document.DocumentElement.AppendChild(node4);
          XmlNode node5 = document.CreateElement("last_name");
          node5.InnerText = _shopperLastName;
          document.DocumentElement.AppendChild(node5);
          XmlNode node6 = document.CreateElement("date_subscribed");
          node6.InnerText = DateTime.Now.ToShortDateString();
          document.DocumentElement.AppendChild(node6);
        }
      return document.InnerXml;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EEMResellerOptIn is not a cachable request.");
    }

    #endregion
  }
}
