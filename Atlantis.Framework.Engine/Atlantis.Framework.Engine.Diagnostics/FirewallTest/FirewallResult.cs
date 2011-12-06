
namespace Atlantis.Framework.Engine.Diagnostics.FirewallTest
{
  internal class FirewallResult
  {
    public string Name { get; set; }
    public string ServiceUrl { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public string Result { get; set; }

    public FirewallResult(string name, string serviceUrl, string ipAddress, int port, string result)
    {
      Name = name;
      ServiceUrl = serviceUrl;
      IPAddress = ipAddress;
      Port = port;
      Result = result;
    }
  }
}
