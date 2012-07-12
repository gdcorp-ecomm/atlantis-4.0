using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLOCAccountDetails.Interface
{
  public class EcommLOCAccountDetailsResponseData : IResponseData
  {
    private AtlantisException _exception;

    private bool _success;
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    private string _responseXML;
    public string ResponseXML
    {
      get
      {
        return _responseXML;
      }
    }

    public EcommLOCAccountDetailsResponseData(bool success, string responseXML)
    {
      _responseXML = responseXML;
      _success = success;
    }

    public EcommLOCAccountDetailsResponseData(AtlantisException aex)
    {
      _success = false;
      _exception = aex;
    }

    public EcommLOCAccountDetailsResponseData(RequestData request, Exception ex)
    {
      _success = false;
      _exception = new AtlantisException(request, "EcommLOCAccountDetailsResponseData", ex.Message, string.Empty);
    }


    #region IResponseData Members
    public string ToXML()
    {
      XmlSerializer serializer = new XmlSerializer(typeof(EcommLOCAccountDetailsResponseData));
      StringWriter writer = new StringWriter();

      serializer.Serialize(writer, this);

      return writer.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
