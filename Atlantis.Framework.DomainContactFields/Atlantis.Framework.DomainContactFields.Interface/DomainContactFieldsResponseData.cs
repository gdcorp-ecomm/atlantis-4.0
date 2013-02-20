using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactFields.Interface
{
  public class DomainContactFieldsResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public DomainContactFieldsResponseData()
    {
    }

    public DomainContactFieldsResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public DomainContactFieldsResponseData(AtlantisException aex)
    {
      _exception = aex;
    }

    public string DomainContactFields { get; private set; }

    public DomainContactFieldsResponseData(string contactFieldsXml)
    {
      DomainContactFields = string.Empty;

      if (!string.IsNullOrEmpty(contactFieldsXml))
      {
        DomainContactFields = contactFieldsXml;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return DomainContactFields;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
