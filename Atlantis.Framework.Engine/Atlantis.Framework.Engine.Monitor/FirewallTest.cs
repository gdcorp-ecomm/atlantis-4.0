using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using Atlantis.Framework.Engine.Monitor.Firewall;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Monitor
{
  internal class FirewallTest : IMonitor
  {
    public XDocument GetMonitorData()
    {
      XDocument result = new XDocument();
      XElement root = new XElement("ConfigElements");

      root.Add(GetMachineName(), GetMachineIP());

      try
      {
        List<FirewallResult> results = RunFirewallTest();
        foreach (FirewallResult fwr in results)
        {
          root.Add(fwr.ToXml());
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      result.Add(root);
      return result;
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("machinename", Environment.MachineName);
    }

    private XAttribute GetMachineIP()
    {
      string machineIP = "undetermined";
      try
      {
        IPAddress[] addresses = System.Net.Dns.GetHostAddresses(Environment.MachineName);
        if ((addresses != null) && (addresses.Length > 0))
        {
          machineIP = addresses[0].ToString();
        }
      }
      catch (Exception ex)
      {
        machineIP = ex.Message;
      }
      return new XAttribute("machineip", machineIP);
    }

    private static List<FirewallResult> RunFirewallTest()
    {
      List<FirewallResult> outputList = new List<FirewallResult>();

      IList<ConfigElement> configs = Engine.GetConfigElements();
      foreach (ConfigElement config in configs)
      {
        WsConfigElement wsConfig = config as WsConfigElement;
        if (wsConfig != null)
        {
          string name = wsConfig.ProgID.Replace("Atlantis.Framework.", "A.F.");
          Uri serviceUrl;
          IPAddress ipAddress;
          string result;
          int port;
          if (Uri.TryCreate(wsConfig.WSURL, UriKind.Absolute, out serviceUrl))
          {
            bool success = TestConnect(serviceUrl, out ipAddress, out port, out result);
            string ipAddressResult = ipAddress == null ? string.Empty : ipAddress.ToString();
            int resultCode = success ? 0 : -1;
            FirewallResult outputItem = new FirewallResult(wsConfig.RequestType, name, serviceUrl.ToString(), ipAddressResult, port, resultCode, result);
            outputList.Add(outputItem);
          }
        }
      }

      return outputList;
    }

    private static bool TestConnect(Uri serviceUrl, out IPAddress ipAddress, out int port, out string result)
    {
      port = 80;
      bool success = false;
      ipAddress = null;
      try
      {
        IPAddress[] addresses = Dns.GetHostAddresses(serviceUrl.Host);
        if ((addresses == null) || (addresses.Length <= 0))
        {
          result = "No addresses for host " + serviceUrl.Host;
        }
        else
        {
          ipAddress = addresses[0];
          if (serviceUrl.ToString().StartsWith("https:", StringComparison.InvariantCultureIgnoreCase))
          {
            port = 443;
          }

          TcpClient socket = null;
          try
          {
            TimeSpan oneSecond = TimeSpan.FromSeconds(1);
            socket = TimeOutSocket.Connect(serviceUrl.Host, port, oneSecond);
            if (socket.Connected)
            {
              result = "Success!";
              success = true;
            }
            else
            {
              result = "Failed! Socket not connected?";
            }
          }
          finally
          {
            if ((socket != null) && (socket.Client != null))
            {
              socket.Client.Dispose();
            }
          }

        }
      }
      catch (Exception ex)
      {
        result = ex.Message;
      }
      return success;
    }

  }
}
