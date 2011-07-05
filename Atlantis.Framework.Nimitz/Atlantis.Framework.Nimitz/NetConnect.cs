using System;
using Atlantis.Framework.Interface;
using netConnect;

namespace Atlantis.Framework.Nimitz
{
  public static class NetConnect
  {
    /// <summary>
    /// Looks up connection string from Nimitz
    /// </summary>
    /// <param name="config">Atlantis Config element that contains config values named: DataSourceName,CertificateName,ApplicationName</param>
    /// <returns>connection string</returns>
    public static string LookupConnectInfo(ConfigElement config)
    {
      return LookupConnectInfo(config, ConnectLookupType.NetConnectionString);
    }

    /// <summary>
    /// Looks up connection string from Nimitz
    /// </summary>
    /// <param name="config">Atlantis Config element that contains config values named: DataSourceName,CertificateName,ApplicationName</param>
    /// <param name="lookupType">Connect lookup type</param>
    /// <returns>connection string</returns>
    public static string LookupConnectInfo(ConfigElement config, ConnectLookupType lookupType)
    {
      string dataSourceName = config.GetConfigValue("DataSourceName");
      string certificateName = config.GetConfigValue("CertificateName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string contextInfo = config.ProgID;
      return LookupConnectInfoInt(dataSourceName, certificateName, applicationName, contextInfo, lookupType);
    }

    /// <summary>
    /// Looks up connection string from Nimitz. If possible use the overload with the ConfigElement instead.
    /// </summary>
    /// <param name="dataSourceName">data source name</param>
    /// <param name="certficateName">certificate name</param>
    /// <param name="applicationName">application name</param>
    /// <param name="callingFunction">name of calling function for logging purposes</param>
    /// <param name="lookupType">connect lookup type</param>
    /// <returns>connection string</returns>
    public static string LookupConnectInfo(string dataSourceName, string certficateName, string applicationName, string callingFunction, ConnectLookupType lookupType)
    {
      return LookupConnectInfoInt(dataSourceName, certficateName, applicationName, callingFunction, lookupType);
    }

    private static string LookupConnectInfoInt(string dataSourceName, string certficateName, string applicationName, string contextInfo, ConnectLookupType lookupType)
    {
      string result = string.Empty;

      DateTime startNimitz = DateTime.Now;
      netConnect.Info connectInfo = new netConnect.Info();
      result = connectInfo.Get(dataSourceName, applicationName, certficateName, GetConnectType(lookupType));
      DateTime endNimitz = DateTime.Now;

      if (endNimitz > startNimitz.AddSeconds(2))
      {
        TimeSpan nimitzTime = endNimitz.Subtract(startNimitz);
        string message = "Timer Exception: Nimitz Get took longer than 2 seconds.";
        string data = dataSourceName + ":" + applicationName + ":" + certficateName + ":" + nimitzTime.TotalSeconds.ToString();
        AtlantisException exception = new AtlantisException(contextInfo, string.Empty, "100", message, data, string.Empty, string.Empty, string.Empty, string.Empty, 0);
        Engine.Engine.LogAtlantisException(exception);
      }

      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        string message = "Nimitz database connection string lookup failed.";
        string data = "No ConnectionFound For:" + dataSourceName + ":" + applicationName + ":" + certficateName;
        AtlantisException exception = new AtlantisException(contextInfo, string.Empty, "0", message, data, string.Empty, string.Empty, string.Empty, string.Empty, 0);
        throw exception;
      }

      return result;
    }

    private static ConnectTypeEnum GetConnectType(ConnectLookupType lookupType)
    {
      ConnectTypeEnum result = ConnectTypeEnum.CONNECT_TYPE_NET;
      switch (lookupType)
      {
        case ConnectLookupType.NetConnectionString:
          result = ConnectTypeEnum.CONNECT_TYPE_NET;
          break;
        case ConnectLookupType.WebService:
          result = ConnectTypeEnum.CONNECT_TYPE_WEB_SERVICE;
          break;
        case ConnectLookupType.Xml:
          result = ConnectTypeEnum.CONNECT_TYPE_XML;
          break;
        case ConnectLookupType.Delimited:
          result = ConnectTypeEnum.CONNECT_TYPE_DELIMITED;
          break;
      }

      return result;
    }
  }
}
