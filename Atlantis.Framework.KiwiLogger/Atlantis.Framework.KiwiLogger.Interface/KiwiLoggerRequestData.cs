using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.KiwiLogger.Interface
{
  public enum KiwiLoggerFacilities
  {
    fac_kernel = 0,
    fac_user_level = 1,
    fac_mail_system = 2,
    fac_system_daemon = 3,
    fac_security_auth = 4,
    fac_internal_syslog = 5,
    fac_printer = 6,
    fac_network_news = 7,
    fac_UUCP = 8,
    fac_clock = 9,
    fac_security_auth_2 = 10,
    fac_ftp = 11,
    fac_NTP = 12,
    fac_logaudit = 13,
    fac_logalert = 14,
    fac_clock_2 = 15,
    fac_user1 = 16,
    fac_user2 = 17,
    fac_user3 = 18,
    fac_user4 = 19,
    fac_user5 = 20,
    fac_user6 = 21,
    fac_user7 = 22
  }
  public enum KiwiLoggerLevels
  {
    emergency=0,
    alert=1,
    critical=2,
    error=3,
    warning=4,
    notice=5,
    information=6,
    debug=7
  }

  public class KiwiLoggerRequestData : RequestData
  {
    private readonly List<KiwiLoggerParameters> _loggingParameters = new List<KiwiLoggerParameters>();
    private const string LOCALHOST = "127.0.0.1";

    public KiwiLoggerFacilities MessageFacility { get; set; }
    public KiwiLoggerLevels MessageLevel { get; set; }
    public ProtocolType SocketProtocol { get; set; }
    public int ProtocolPort { get; set; }
    public string ServerIPAddress { get; set; }
    public string MessagePrefix { get; set; }
    public string MessageSuffix { get; set; }
    public int MessagePriority
    {
      get
      {
        int calculatedPriority = (int)MessageFacility * 8 + (int)MessageLevel;
        return calculatedPriority;
      }
      set
      {
        //Determine Facility
        int level = value % 8;
        int facility = (value - level) / 8;
        MessageLevel = (KiwiLoggerLevels) level;
        MessageFacility = (KiwiLoggerFacilities) facility;
      }
    }

    public KiwiLoggerRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      MessageFacility = KiwiLoggerFacilities.fac_internal_syslog;
      MessageLevel = KiwiLoggerLevels.notice;
      MessagePrefix = string.Empty;
      MessageSuffix = string.Empty;
      ServerIPAddress = LOCALHOST;
      ProtocolPort = 514;
      SocketProtocol = ProtocolType.Udp;
    }

    public KiwiLoggerRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                               IEnumerable<KiwiLoggerParameters> itemKeys)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      MessageFacility = KiwiLoggerFacilities.fac_internal_syslog;
      MessageLevel = KiwiLoggerLevels.notice;
      MessagePrefix = string.Empty;
      MessageSuffix = string.Empty;
      ServerIPAddress = LOCALHOST;
      ProtocolPort = 514;
      SocketProtocol = ProtocolType.Udp;
      _loggingParameters.AddRange(itemKeys);
    }

    public KiwiLoggerRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                                IEnumerable<KiwiLoggerParameters> itemKeys,
                                int port,
                                string ipAddress
                                )
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      MessageFacility = KiwiLoggerFacilities.fac_internal_syslog;
      MessageLevel = KiwiLoggerLevels.notice;
      MessagePrefix = string.Empty;
      MessageSuffix = string.Empty;
      SocketProtocol = ProtocolType.Udp;
      _loggingParameters.AddRange(itemKeys);
      ProtocolPort = port;
      ServerIPAddress = ipAddress;
    }
    public KiwiLoggerRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                                IEnumerable<KiwiLoggerParameters> itemKeys,
                                int port,
                                string ipAddress,
                                int priority
                                )
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      MessageFacility = KiwiLoggerFacilities.fac_internal_syslog;
      MessageLevel = KiwiLoggerLevels.notice;
      MessagePrefix = string.Empty;
      MessageSuffix = string.Empty;
      SocketProtocol = ProtocolType.Udp;
      _loggingParameters.AddRange(itemKeys);
      _loggingParameters.AddRange(itemKeys);
      ProtocolPort = port;
      ServerIPAddress = ipAddress;
      MessagePriority = priority;
    }

    public void AddItem(string fieldName, string fieldValue)
    {
      _loggingParameters.Add(new KiwiLoggerParameters(fieldName, fieldValue));
    }

    public void AddItems(IEnumerable<KiwiLoggerParameters> itemKeys)
    {
      _loggingParameters.AddRange(itemKeys);
    }

    public string ItemParameters
    {
      get
      {
        string result = string.Empty;
        List<string> itemParms = _loggingParameters.ConvertAll(item => item.ToString());
        result = string.Join(" ", itemParms.ToArray());
        return result;
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("KiwiLogger is not a cacheable request.");
    }
  }
}
