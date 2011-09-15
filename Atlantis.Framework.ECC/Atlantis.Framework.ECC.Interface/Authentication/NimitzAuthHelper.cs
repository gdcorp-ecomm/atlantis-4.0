using System.Xml;

namespace Atlantis.Framework.Ecc.Interface.Authentication
{
  public static class NimitzAuthHelper
  {
    public static bool GetConnectionCredentials(string nimitzAuthXml, out string requestKey, out string authName, out string authToken)
    {
      bool success = false;
      requestKey = string.Empty;
      authName = string.Empty;
      authToken = string.Empty;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(nimitzAuthXml);

      XmlNode requestKeyNode = xdoc.SelectSingleNode("Connect/requestKey");
      XmlNode authNameNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode authTokenNode = xdoc.SelectSingleNode("Connect/Password");

      if (requestKeyNode != null &&
         authNameNode != null &&
         authTokenNode != null)
      {
        requestKey = requestKeyNode.FirstChild.Value;
        authName = authNameNode.FirstChild.Value;
        authToken = authTokenNode.FirstChild.Value;

        success = true;
      }

      return success;
    }
  }
}
