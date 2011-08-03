using System;
using System.IO;
using System.Text;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetOverrideHash.Interface
{
  public class GetOverrideHashResponseData : IResponseData
  {
    private readonly string m_sHash;
    private readonly AtlantisException m_ex;

    public GetOverrideHashResponseData(string sHash)
    {
      m_sHash = sHash;
    }

    public GetOverrideHashResponseData(string sHash, AtlantisException exAtlantis)
    {
      m_sHash = sHash;
      m_ex = exAtlantis;
    }

    public GetOverrideHashResponseData(string sHash, RequestData oRequestData, Exception ex)
    {
      m_sHash = sHash;
      m_ex = new AtlantisException(oRequestData,
                                   "GetOverrideHashResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    public string Hash
    {
      get { return m_sHash; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return m_ex;
    }

    public string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("OverrideHash");
      if(m_sHash != null)
        xtwResult.WriteValue(m_sHash);
      xtwResult.WriteEndElement();

      return sbResult.ToString();
    }

    #endregion
  }
}
