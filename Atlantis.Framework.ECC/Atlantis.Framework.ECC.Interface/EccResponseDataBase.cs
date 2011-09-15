using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Ecc.Interface
{
  public abstract class EccResponseDataBase<T> : IResponseData where T : new()
  {
    public string ResultJson;
    public EccJsonResponse<T> Response;
    private AtlantisException _atlEx;
    private EccJsonException _jsonEx;
    protected EccJsonFault _fault;

    private bool _success = false;
    public bool IsSuccess
    {
      get
      {
        if (_fault != null || Response.Item.ResultCode != 0)
        {
          _success = false;
        }
        return _success;
      }
      set { _success = value; }
    }

    /// <summary>
    /// Parameterless base constructor is made available specifically for use  with ISessionSerializableResponse
    /// </summary>
    protected EccResponseDataBase()
    {
    }

    protected EccResponseDataBase(string jsonResponse)
    {
      ResultJson = jsonResponse;

      if (ResultJson.Contains("jsoap_fault"))
      {
        _fault = EccJsonResponseHandler.ParseJsonFault(ResultJson);
        IsSuccess = false;
        SetException(new EccJsonException(_fault.ResultCode, _fault.Message));
      }

      Response = EccJsonResponseHandler<T>.ParseJsonContent(ResultJson);
      if (Response.Item.ResultCode != 0)
      {
        IsSuccess = false;
        SetException(new EccJsonException(Response.Item.ResultCode, Response.Item.Message));
      }

      IsSuccess = Response.Item.ResultCode == 0;
    }

    protected EccResponseDataBase(RequestData requestData, Exception ex)
    {
      _atlEx = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    protected EccResponseDataBase(AtlantisException aex)
    {
      _atlEx = aex;
      IsSuccess = false;
    }

    private void SetException(EccJsonException jex)
    {
      _jsonEx = jex;
    }

    public virtual EccJsonException GetJsonException()
    {
      return _jsonEx;
    }


    #region Implementation of IResponseData


    public virtual string ToXML()
    {
      var sb = new StringBuilder();
      XmlWriter xmlWriter = XmlWriter.Create(sb);

      if (_fault != null)
      {
        var ser = new DataContractSerializer(_fault.GetType());
        ser.WriteObject(xmlWriter, _fault);
      }
      else
      {
        var ser = new DataContractSerializer(Response.GetType());
        ser.WriteObject(xmlWriter, Response);
      }

      xmlWriter.Flush();
      xmlWriter.Close();

      return sb.ToString().Replace("<response>", string.Empty).Replace("</response>", string.Empty);
    }

    public virtual AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
