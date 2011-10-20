using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.SAGetDomainListByShopper.Interface
{
  public class SAGetDomainListByShopperResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception;
    private DomainListResponseData _response;
    private bool _success;

    public List<string> Domains
    {
      get { return _response.DomainList; }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public SAGetDomainListByShopperResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public SAGetDomainListByShopperResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "SAGetDomainListByShopperResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public SAGetDomainListByShopperResponseData(DomainListResponseData data)
    {
      _response = data;
      _success = data.ReturnCode == "0";
    }

    #region IResponseData Members

    public string ToXML()
    {
      var sb = new StringBuilder();
      sb.Append("<DomainListByShopper>");
      sb.AppendFormat("<ReturnCode>{0}</ReturnCode>", _response.ReturnCode);
      sb.AppendFormat("<ReturnMessage>{0}</ReturnMessage>", _response.ReturnMessage);
      sb.AppendFormat("<DomainCount>{0}</DomainCount>", _response.DomainCount);
      sb.Append("<DomainList>");
      foreach (var domain in _response.DomainList)
      {
        sb.AppendFormat("<Domain>{0}</Domain>", domain);
      }
      sb.Append("</DomainList>");
      sb.Append("</DomainListByShopper>");

      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      var sb = new StringBuilder();
      XmlWriter xmlWriter;
      xmlWriter = XmlWriter.Create(sb);

      var ser = new DataContractSerializer(_response.GetType());
      ser.WriteObject(xmlWriter, _response);

      xmlWriter.Flush();
      xmlWriter.Close();

      return sb.ToString();
    }

    public void DeserializeSessionData(string sessionData)
    {
      var ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
      DataContractSerializer ser;

      try
      {
        ser = new DataContractSerializer(typeof(DomainListResponseData));
        _response = ser.ReadObject(ms) as DomainListResponseData;
        if (_response != null)
        {
          _success = (_response.ReturnCode == "0" ? true : false);
        }
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }
    }


    #endregion
  }
}
