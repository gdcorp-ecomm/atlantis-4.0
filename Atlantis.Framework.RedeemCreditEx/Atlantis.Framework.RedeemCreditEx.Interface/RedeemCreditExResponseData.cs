using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RedeemCreditEx.Interface
{
  public class RedeemCreditExResponseData : IResponseData
  {
    #region Properties

    public bool Success { get; private set; }
    public string ResourceId { get; private set; }
    public AtlantisException AtlException { get; set; }
    public string XML { get; set; }

    #endregion 

    public RedeemCreditExResponseData() { }

    public RedeemCreditExResponseData(string xml)
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(xml);
      XmlNode resourseNode = xDoc.SelectSingleNode("RESPONSE/CREDIT/ResourceID");
      if (resourseNode != null)
      {
        ResourceId = resourseNode.InnerText.Trim();
        Success = true;
      }
    }

    public RedeemCreditExResponseData(AtlantisException aex)
    {
      AtlException = aex;
      Success = false;
    }

    public AtlantisException GetException()
    {
      return AtlException;
    }

    public string ToXML()
    {
      return XML;
    }

  }
}
