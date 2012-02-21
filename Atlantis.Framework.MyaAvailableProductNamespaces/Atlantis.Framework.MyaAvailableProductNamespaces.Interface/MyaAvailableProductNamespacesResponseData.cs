using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Interface
{
  public class MyaAvailableProductNamespacesResponseData: IResponseData
  {
    private AtlantisException _exception;

    public DataTable Namespaces { get; private set; }
    public bool IsSuccess { get; private set; }

    public MyaAvailableProductNamespacesResponseData(DataTable _data)
    {
      Namespaces = _data;
      IsSuccess = true;
    }

    public MyaAvailableProductNamespacesResponseData(Exception ex, RequestData requestData)
    {
      _exception = new AtlantisException(requestData, "MyaAvailableProductNamespacesResponseData", ex.Message, string.Empty, ex);
    }

    public MyaAvailableProductNamespacesResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    public string ToXML()
    {
      var sw = new StringWriter();
      var serializer = new XmlSerializer(typeof(DataTable));
      serializer.Serialize(sw, Namespaces);
      return sw.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
