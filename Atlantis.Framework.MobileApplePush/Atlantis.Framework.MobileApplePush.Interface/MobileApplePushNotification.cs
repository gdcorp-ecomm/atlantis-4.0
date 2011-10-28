using System;
using System.Net;
using System.Text;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushNotification
  {
    private const int DEVICE_TOKEN_BINARY_SIZE = 32;
    private const int DEVICE_TOKEN_STRING_SIZE = 64;
    private const int MAX_PAYLOAD_SIZE = 256;

    public string DeviceToken { get; private set; }

    public MobileApplePushNotificationPayload Payload { get; private set; }

    public bool IsValidDeviceToken
    {
      get { return !string.IsNullOrEmpty(DeviceToken) && DeviceToken.Length == DEVICE_TOKEN_STRING_SIZE; }
    }

    private bool GetDeviceTokenBytes(out byte[] deviceTokenBytes)
    {
      deviceTokenBytes = new byte[DeviceToken.Length / 2];
      for (int i = 0; i < deviceTokenBytes.Length; i++)
      {
        deviceTokenBytes[i] = byte.Parse(DeviceToken.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
      }

      return deviceTokenBytes.Length == DEVICE_TOKEN_BINARY_SIZE;
    }

    public bool GetBytes(out byte[] bytes, out string errorMessage)
    {
      bool success = false;
      bytes = null;
      errorMessage = string.Empty;

      byte[] deviceTokenBytes;

      if (!GetDeviceTokenBytes(out deviceTokenBytes))
      {
        errorMessage = string.Format("Bad device token: {0}", DeviceToken);
      }
      else
      {
        byte[] deviceTokenSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(deviceTokenBytes.Length)));
        byte[] payloadBytes = Encoding.UTF8.GetBytes(Payload.ToJson());

        if (payloadBytes.Length > MAX_PAYLOAD_SIZE)
        {
          errorMessage = string.Format("Payload is too large. Payload: {0}", Payload.ToJson());
        }
        else
        {
          byte[] payloadSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(payloadBytes.Length)));

          int bufferSize = sizeof(Byte) + deviceTokenSize.Length + deviceTokenBytes.Length + payloadSize.Length + payloadBytes.Length;
          byte[] buffer = new byte[bufferSize];
          buffer[0] = 0x00;

          Buffer.BlockCopy(deviceTokenSize, 0, buffer, sizeof(Byte), deviceTokenSize.Length);
          Buffer.BlockCopy(deviceTokenBytes, 0, buffer, sizeof(Byte) + deviceTokenSize.Length, deviceTokenBytes.Length);
          Buffer.BlockCopy(payloadSize, 0, buffer, sizeof(Byte) + deviceTokenSize.Length + deviceTokenBytes.Length, payloadSize.Length);
          Buffer.BlockCopy(payloadBytes, 0, buffer, sizeof(Byte) + deviceTokenSize.Length + deviceTokenBytes.Length + payloadSize.Length, payloadBytes.Length);

          bytes = buffer;
          success = true;
        }
      }

      return success;
    }

    public MobileApplePushNotification(string deviceToken, string message)
    {
      DeviceToken = deviceToken;
      Payload = new MobileApplePushNotificationPayload(message);
    }
  }
}
