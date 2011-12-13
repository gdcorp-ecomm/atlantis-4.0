using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRemCampBlazDur.Interface
{
  public class GetRemCampBlazDurResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public Decimal Duration { get; set; }

    public GetRemCampBlazDurResponseData(string xml)
    {

    }

    public GetRemCampBlazDurResponseData(decimal duration)
    {
      Duration = duration;
      IsSuccess = true;
    }

    public GetRemCampBlazDurResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GetRemCampBlazDurResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "GetRemCampBlazDurResponseData",
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
