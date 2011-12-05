using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.IrisGetServiceMappings.Interface
{
  public class IrisGetServiceMappingsResponseData : IResponseData
  {
    #region Constructors

    public IrisGetServiceMappingsResponseData(string response, IrisServiceMappings parsedResponse)
    {
      _response = response;
      _isSuccess = true;
      _irisServiceMappingsResponse = parsedResponse;
    }

    public IrisGetServiceMappingsResponseData(IrisGetServiceMappingsRequestData requestData, Exception ex)
    {
      _exception = BuildException(requestData, "Constructor", ex, String.Concat("requestData.RequestTimeout(ms)=", requestData.RequestTimeout.Milliseconds, ", requestData.ResellerId=", requestData.ResellerId));
    }

    public IrisGetServiceMappingsResponseData(RequestData requestData, Exception ex)
    {
      _exception = BuildException(requestData, "Constructor", ex, null);
    }

    #endregion

    #region Helpers

    private static AtlantisException BuildException(RequestData requestData, string sourceFunction, Exception ex, string data)
    {
      return new AtlantisException(requestData, "IrisGetServiceMappingsResponseData." + sourceFunction, ex.Message + Environment.NewLine + ex.StackTrace, data, ex);
    }

    #endregion

    private bool _isSuccess = false;
    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    private string _response;
    public string Response
    {
      get { return _response; }
    }

    private IrisServiceMappings _irisServiceMappingsResponse;
    public IrisServiceMappings IrisServiceMappingsResponse
    {
      get { return _irisServiceMappingsResponse; }
    }

    #region Return Data

    [DataContract()]
    public class IrisServiceMappings
    {
      [DataMember()]
      public IDictionary<string, IList<IrisServiceMapping>> Groupings { get; set; }
    }

    [DataContract()]
    public class IrisServiceMapping
    {
      [DataMember()]
      public int ServiceId { get; set; }

      [DataMember()]
      public string FriendlyName { get; set; }

      [DataMember(EmitDefaultValue = false, IsRequired = false)]
      public List<int> ProductGroup { get; set; }
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return _response;
    }

    private AtlantisException _exception;
    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
