using System;
using System.Xml;
using System.Text;
using System.IO;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LegalLastModifiedDate.Interface
{
  public class LegalLastModifiedDateResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    private readonly bool _success;
    private readonly DateTime _lastModifiedDate = new DateTime(2001,1,1);

    public DateTime LastModifiedDate 
    { 
      get { return _lastModifiedDate; }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public LegalLastModifiedDateResponseData(DateTime inLastModifiedDate)
    {
      if (inLastModifiedDate != DateTime.MinValue && inLastModifiedDate != DateTime.MaxValue)
      {
        _lastModifiedDate = inLastModifiedDate;
        _success = true;
      }
    }

    public LegalLastModifiedDateResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public LegalLastModifiedDateResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData, "LegalLastModifiedDateResponseData", ex.Message, oRequestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("LegalLastModifiedDate", Convert.ToString(_lastModifiedDate));
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
