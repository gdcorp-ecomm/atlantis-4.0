using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRemainPLDur.Interface
{
  public class GetRemainPLDurResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public Decimal Duration { get; set; }

    public GetRemainPLDurResponseData(string xml)
    {

    }

    public GetRemainPLDurResponseData(decimal duration)
    {
      Duration = duration;
      IsSuccess = true;
    }

    public GetRemainPLDurResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GetRemainPLDurResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "GetRemainPLDurResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      var xdoc = new XDocument();
      var durationValue = new XElement("duration", Duration);
      xdoc.Add(durationValue);
      return xdoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
