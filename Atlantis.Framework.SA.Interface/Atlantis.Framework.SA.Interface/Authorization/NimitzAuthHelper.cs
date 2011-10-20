using System.Xml;

namespace Atlantis.Framework.SA.Interface.Authorization
{
  public static class NimitzAuthHelper
  {
    public static bool GetConnectionCredentials(string nimitzAuthXml, out string authName, out string authToken)
    {
      bool success = false;
      authName = string.Empty;
      authToken = string.Empty;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(nimitzAuthXml);

      XmlNode authNameNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode authTokenNode = xdoc.SelectSingleNode("Connect/Password");

      if (
         authNameNode != null &&
         authTokenNode != null)
      {
        authName = authNameNode.FirstChild.Value;
        authToken = authTokenNode.FirstChild.Value;

        success = true;
      }

      return success;
    }
  }
}
