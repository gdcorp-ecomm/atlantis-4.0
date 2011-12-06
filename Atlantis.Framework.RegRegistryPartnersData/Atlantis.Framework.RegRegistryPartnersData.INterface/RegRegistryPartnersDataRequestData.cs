using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegRegistryPartnersData.Interface
{
  public class RegRegistryPartnersDataRequestData : RequestData
  {
    public RegRegistryPartnersDataRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    private string _bronsonSourceId = "1";
    public string BronsonSourceId
    {
      get
      {
        return _bronsonSourceId;
      }
      set
      {
        _bronsonSourceId = value;
      }
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("REQUEST");
      xtwRequest.WriteStartElement("BRONSONSOURCE");
      xtwRequest.WriteAttributeString("id", BronsonSourceId);
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion
  }
}
