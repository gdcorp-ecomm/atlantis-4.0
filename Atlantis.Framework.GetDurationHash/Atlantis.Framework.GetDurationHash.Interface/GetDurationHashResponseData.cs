using System;
using System.IO;
using System.Text;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetDurationHash.Interface
{
  public class GetDurationHashResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    public string Hash { get; private set; }

    public GetDurationHashResponseData(string sHash)
    {
      Hash = sHash;
    }

    public GetDurationHashResponseData(string sHash, AtlantisException exAtlantis)
    {
      Hash = sHash;
      m_ex = exAtlantis;
    }

    public GetDurationHashResponseData(string sHash, RequestData oRequestData, Exception ex)
    {
      Hash = sHash;
      m_ex = new AtlantisException(oRequestData,
                                   "GetDurationHashResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
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

      xtwResult.WriteStartElement("DurationHash");
      if(Hash != null)
        xtwResult.WriteValue(Hash);
      xtwResult.WriteEndElement();

      return sbResult.ToString();
    }

    #endregion
  }
}
