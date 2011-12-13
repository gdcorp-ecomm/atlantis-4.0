using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace Atlantis.Framework.InstantStoreSetup.Interface
{
  public class InstantStoreSetupResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public InstantStoreSetupResponseData()
    {
      _success = true;
    }

    public InstantStoreSetupResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public InstantStoreSetupResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "InstantStoreSetupResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
