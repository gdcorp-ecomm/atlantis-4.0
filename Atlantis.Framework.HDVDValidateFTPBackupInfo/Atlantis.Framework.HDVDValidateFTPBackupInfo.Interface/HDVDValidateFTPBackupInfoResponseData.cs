using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateFTPBackupInfo.Interface
{
  public class HDVDValidateFTPBackupInfoResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public AriesValidationResponse Response { get; private set; }

    public bool IsSuccess
    {
      get
      {
        bool bSuccess = false;
        if (Response != null)
        {
          bSuccess = (Response.StatusCode == 0);
        }

        return bSuccess;
      }
    }

     public HDVDValidateFTPBackupInfoResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDValidateFTPBackupInfoResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDValidateFTPBackupInfoResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public HDVDValidateFTPBackupInfoResponseData(AriesValidationResponse response)
    {
      Response = response;

      if (response.Status != "success" || response.HasErrors)
      {
        if (response.Errors != null)
        {
          Errors = response.Errors.ToList();
        }
        return;
      }
    }

    protected List<string> Errors { get; set; }

    #region IResponseData Members

    public string ToXML()
    {
      string xml = string.Empty;
      try
      {
        var serializer = new DataContractSerializer(this.Response.GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new System.Xml.XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this.Response);
          xml = backing.ToString();
        }
      }
      catch (Exception ex)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
