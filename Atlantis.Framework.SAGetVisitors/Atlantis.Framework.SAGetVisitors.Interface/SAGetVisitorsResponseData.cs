using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.SAGetVisitors.Interface
{
  public class SAGetVisitorsResponseData : IResponseData, ISessionSerializableResponse  
  {
    private AtlantisException _exception  ;
    private VisitorResponseData _response;
    private bool _success;

    public bool IsSuccess {
      get { return _success; }
    }

    public VisitorResponseData Response { get; set; }
    public string ShopperId { get; set; }

    public List<Visit> Visits {
      get { return _response.Visits; }
    }
   

    public SAGetVisitorsResponseData(VisitorResponseData  data)
    {
      _response = data;
      if (_response.ReturnCode == "0")
      {
        _success = true;
      }
      ShopperId = _response.ShopperId;

    }

    public SAGetVisitorsResponseData(AtlantisException atlantisException)
    {
      _success = false;
      _exception = atlantisException;
    }
    
    public SAGetVisitorsResponseData(RequestData requestData, Exception exception)
    {
      _success = false;
      _exception = new AtlantisException(requestData, exception.Source, exception.Message, string.Empty, exception);
    }

    #region Implementation of IResponseData

    public string ToXML()
    {

      var sb = new StringBuilder();
      sb.Append("<VisitorsResponseData>");
      sb.AppendFormat("<ReturnCode>{0}</ReturnCode>", _response.ReturnCode);
      sb.AppendFormat("<ReturnMessage>{0}</ReturnMessage>", _response.ReturnMessage);
      sb.AppendFormat("<ShopperId>{0}</ShopperId>", _response.ShopperId);
      sb.Append("<Visitors>");
      foreach (var visit in _response.Visits)
      {
        sb.AppendFormat("<Visitor statsdate=\"{0}\" uniquevisitors=\"{1}\" visitors=\"{2}\" />", visit.StatsDate, visit.UniqueVisitors, visit.Visitors);
      }
      sb.Append("</Visitors>");
      sb.Append("</VisitorsResponseData>");

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
        ser = new DataContractSerializer(typeof(VisitorResponseData));
        _response = ser.ReadObject(ms) as VisitorResponseData;
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
