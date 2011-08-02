using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Atlantis.Framework.Interface;
using Atlantis.Framework.KiwiLogger.Interface;

namespace Atlantis.Framework.KiwiLogger.Impl
{
  public class KiwiLoggerRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      KiwiLoggerResponseData oResponseData;

      try
      {
        var oKiwiLoggerRequestData = (KiwiLoggerRequestData)oRequestData;
        //Create Connection via UDP to SysLogger
        int protocolPort = oKiwiLoggerRequestData.ProtocolPort;

        using (var sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, oKiwiLoggerRequestData.SocketProtocol))
        {
          var sendTo = IPAddress.Parse(oKiwiLoggerRequestData.ServerIPAddress);
          var sendEndPoint = new IPEndPoint(sendTo, protocolPort);
          var newMessage = new StringBuilder();
          newMessage.Append("<");
          newMessage.Append(oKiwiLoggerRequestData.MessagePriority);
          newMessage.Append(">");
          newMessage.Append(oKiwiLoggerRequestData.MessagePrefix);
          newMessage.Append(oKiwiLoggerRequestData.ItemParameters);
          newMessage.Append(oKiwiLoggerRequestData.MessageSuffix);
          //Message is of the format
          //<priority> FieldName=FieldValue FieldName1=FieldValue1 ... etc
          Byte[] messageBuffer = Encoding.UTF8.GetBytes(newMessage.ToString());
          sendSocket.SendTo(messageBuffer, messageBuffer.Length, SocketFlags.None, sendEndPoint);
        }

        oResponseData = new KiwiLoggerResponseData("<Result>Success</Result>");
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new KiwiLoggerResponseData(string.Empty, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new KiwiLoggerResponseData(string.Empty, oRequestData, ex);
      }

      return oResponseData;
      
    }

    #endregion
  }
}
