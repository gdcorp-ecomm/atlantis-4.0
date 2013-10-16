using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using System.Xml.Linq;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoGetKeyResponseData : IResponseData
  {
    private AtlantisException _aex;
    private Token _token = new Token();

    public Key Key { get; private set; }
    public bool IsSuccess { get; private set; }

    public SsoGetKeyResponseData(Key key, Token token)
    {
      Key = key;
      _token = token;
      IsSuccess = true;
    }

    public SsoGetKeyResponseData(AtlantisException aex)
    {
      _aex = aex;
    }
    
    public string ToXML()
    {
      XElement element = new XElement("SsoGetKeyResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
