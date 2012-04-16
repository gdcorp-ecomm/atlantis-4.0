using System;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.DNSSECGetStatus.Interface
{
  public class DNSSECGetStatusResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _atlantisException = null;
    private string _resultXML = string.Empty;
    public int UsedDnsSec { get; private set; }
    public int TotalDnsSec { get; private set; }
    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public DNSSECGetStatusResponseData()
    { }

    public DNSSECGetStatusResponseData(int usedZones, int totalZones)
    {
      UsedDnsSec = usedZones;
      TotalDnsSec = totalZones;
      _resultXML =  new XElement("dnssecusage",
        new XAttribute("usedzones", UsedDnsSec),
        new XAttribute("totalzones", TotalDnsSec)
      ).ToString();
    }

    public DNSSECGetStatusResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    public DNSSECGetStatusResponseData(RequestData requestData, Exception exception)
    {
      _atlantisException = new AtlantisException(requestData,
                                   "DNSSECGetStatusResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultXML = sessionData;
      XmlDocument result = new XmlDocument();
      result.LoadXml(sessionData);

      XmlNode root = result.SelectSingleNode("dnssecusage");

      int worker;
      if (int.TryParse(root.Attributes["usedzones"].Value, out worker))
      {
        UsedDnsSec = worker;
      }
      if (int.TryParse(root.Attributes["totalzones"].Value, out worker))
      {
        TotalDnsSec = worker;
      }
    }
    #endregion
  }
}
