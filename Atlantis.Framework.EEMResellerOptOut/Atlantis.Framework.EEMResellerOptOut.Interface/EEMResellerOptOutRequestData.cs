using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptOut.Interface
{
  public class EEMResellerOptOutRequestData : RequestData 
  {

    private int _privateLabelId;


    public EEMResellerOptOutRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string emailAddress) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _privateLabelId = privateLabelId;
      _emailAddress = emailAddress;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
      set { _privateLabelId = value; }
    }

    private string _emailAddress;
    public string EmailAddress
    {
      get { return _emailAddress; }
      set { _emailAddress = value; }
    }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("EEMResellerOptOut is not a cachable request.");
    }

    public override string ToXML()
    {
      XmlDocument document = new XmlDocument();
      document.AppendChild(document.CreateElement("Subscriber"));
      XmlNode newChild = document.CreateElement("reseller_private_label_id");
      newChild.InnerText = _privateLabelId.ToString();
      document.DocumentElement.AppendChild(newChild);
      XmlNode node2 = document.CreateElement("email");
      node2.InnerText = _emailAddress;
      document.DocumentElement.AppendChild(node2);

      return document.InnerXml;
    }

    #endregion
  }
}
