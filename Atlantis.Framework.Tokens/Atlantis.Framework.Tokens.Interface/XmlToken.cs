using System.Xml.Linq;

namespace Atlantis.Framework.Tokens.Interface
{
  public class XmlToken : TokenBase<XElement>
  {
    public XmlToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
    }

    protected override XElement DeserializeTokenData(string data)
    {
      XElement result;

      if (string.IsNullOrEmpty(data))
      {
        result = new XElement("empty");
      }
      else
      {
        result = XElement.Parse(data);
      }

      return result;
    }
  }
}
