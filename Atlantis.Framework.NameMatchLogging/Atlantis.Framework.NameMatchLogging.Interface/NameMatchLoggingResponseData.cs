using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.NameMatchLogging.Interface
{
  public class NameMatchLoggingResponseData : IResponseData
  {
    private bool _hasErrors = false;
    public bool HasError
    {
      get
      {
        return _hasErrors;
      }
      set
      {
        _hasErrors = value;
      }
    }

    private AtlantisException _aex = null;
    private RequestData oRequestData;
    private Exception ex;

    public NameMatchLoggingResponseData(bool completed)
    {
      _hasErrors = completed;
    }

    public NameMatchLoggingResponseData(AtlantisException exAtlantis)
    {
      _aex = exAtlantis;
      _hasErrors = true;
    }

    public NameMatchLoggingResponseData(RequestData oRequestData, Exception ex)
    {
      _hasErrors = true;
      this.oRequestData = oRequestData;
      this.ex = ex;
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _aex;
    }

    public string ToXML()
    {
      StringBuilder result = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(result));

      xtwResult.WriteStartElement("Response");
      xtwResult.WriteAttributeString("value", "no_reponse");
      xtwResult.WriteEndElement();

      return result.ToString();
    }

    #endregion
  }
}
