using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Links.Interface
{
  public class LinkInfoRequestData : RequestData
  {
    public LinkInfoRequestData(int contextId)
    {
      ContextID = contextId;
    }

    public int ContextID { get; private set; }

    #region RequestData Members

    public override string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("LinkInfo");

      xtwResult.WriteStartElement("param");
      xtwResult.WriteAttributeString("name", "contextID");
      xtwResult.WriteAttributeString("value", ContextID.ToString(CultureInfo.InvariantCulture));
      xtwResult.WriteEndElement(); // param

      xtwResult.WriteEndElement(); // LinkInfo

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      return ContextID.ToString(CultureInfo.InvariantCulture);
    }

    #endregion

  }
}
