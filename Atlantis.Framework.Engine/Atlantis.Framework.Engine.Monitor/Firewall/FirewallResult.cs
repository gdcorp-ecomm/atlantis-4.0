using System.Xml.Linq;

namespace Atlantis.Framework.Engine.Monitor.Firewall
{
  internal class FirewallResult
  {
    public int RequestType { get; set; }
    public string Name { get; set; }
    public string ServiceUrl { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public string Result { get; set; }
    public int ResultCode { get; set; }

    public FirewallResult(int requestType, string name, string serviceUrl, string ipAddress, int port, int resultCode, string result)
    {
      RequestType = requestType;
      Name = name;
      ServiceUrl = serviceUrl;
      IPAddress = ipAddress;
      Port = port;
      Result = result;
      ResultCode = resultCode;
    }

    public XElement ToXml()
    {
      XElement result = new XElement("ConfigElement");

      result.Add(new XAttribute("requesttype", RequestType));
      result.Add(new XAttribute("name", Name));
      result.Add(new XAttribute("resultcode", ResultCode.ToString()));
      result.Add(new XElement("serviceurl", new XCData(ServiceUrl)));
      result.Add(new XElement("ipaddress", new XCData(IPAddress)));
      result.Add(new XElement("port", new XCData(Port.ToString())));
      result.Add(new XElement("result", new XCData(Result)));

      return result;
    }
  }
}
