using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballCICreate.Interface
{
  public class FastballCICreateResponseData : IResponseData
  {
    private string _xml = string.Empty;
    public string CICodeXMLOutput
    {
      get { return _xml; }
      set { _xml = value; }
    }
    private AtlantisException _exception = null;

    public FastballCICreateResponseData()
    {
    }

    public FastballCICreateResponseData(RequestData requestData, string ciCodeXMLOutput)
    {
      CICodeXMLOutput = ciCodeXMLOutput;
    }

    public FastballCICreateResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballCICreate", ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public bool IsSuccess { get; set; }

    #region IResponseData Members

    public string ToXML()
    {
      return _xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}