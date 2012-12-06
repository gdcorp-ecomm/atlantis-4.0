using System;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.GetOverrideHash.Interface
{
  public class GetOverrideHashResponseData : IResponseData
  {
    private readonly string _hash;
    private readonly AtlantisException _exception;

    public GetOverrideHashResponseData(string hash)
    {
      _hash = hash;
    }

    public GetOverrideHashResponseData(string hash, RequestData requestData, Exception ex)
    {
      _hash = hash;
      _exception = new AtlantisException(requestData, "GetOverrideHashResponseData", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    public string Hash
    {
      get { return _hash; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      XElement element = new XElement("OverrideHash", _hash);
      return element.ToString();
    }

    #endregion
  }
}
