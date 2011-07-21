using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaIsMirageCurrent.Interface
{
  public class MyaIsMirageCurrentResponseData : IResponseData
  {
    public MyaIsMirageCurrentResponseData(bool isCurrent)
    {
      IsCurrent = isCurrent;
      RequestException = null;
    }

    public MyaIsMirageCurrentResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + Environment.NewLine + ex.StackTrace;
      RequestException = new AtlantisException(requestData, "MyaIsMirageCurrentResponseData", message, requestData.ToXML(), ex);
      IsCurrent = true;
    }

    public AtlantisException RequestException { get; private set; }
    public bool IsCurrent { get; private set; }

    #region IResponseData Members

    public string ToXML()
    {
      XElement rootElement = new XElement("MyaIsMirageCurrentResponseData", new XElement("IsCurrent", IsCurrent.ToString()));
      XDocument xmlDoc = new XDocument(rootElement);

      if (RequestException != null)
      {
        XElement errorElement = new XElement("Error", RequestException.ErrorDescription);
        rootElement.Add(errorElement);
      }

      return xmlDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
