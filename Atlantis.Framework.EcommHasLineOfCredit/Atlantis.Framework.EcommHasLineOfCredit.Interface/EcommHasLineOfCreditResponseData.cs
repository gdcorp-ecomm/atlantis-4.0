using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommHasLineOfCredit.Interface
{
  public class EcommHasLineOfCreditResponseData : IResponseData
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

    private readonly bool _hasLOC;
    public bool HasLineOfCredit
    {
      get
      {
        return _hasLOC;
      }
    }

    public EcommHasLineOfCreditResponseData(bool hasLoc, bool success)
    {
      _hasLOC = hasLoc;
      _success = success;
    }

    public EcommHasLineOfCreditResponseData(AtlantisException aex)
    {
      _success = false;
      _exception = aex;
    }

    public EcommHasLineOfCreditResponseData(RequestData request, Exception ex)
    {
      _success = false;
      _exception = new AtlantisException(request, "EcommHasLineOfCreditResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members
    public string ToXML()
    {
      XmlSerializer serializer = new XmlSerializer(typeof(EcommHasLineOfCreditResponseData));
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
