using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CreateIncidentInIRIS.Interface
{
  public class CreateIncidentInIRISResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _ex;
    private long _irisResult;

    

    public CreateIncidentInIRISResponseData(long irisResult)
    {
       _irisResult = irisResult;
      _success = true;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public long IRISResult
    {
      get { return _irisResult; }
    }

    public CreateIncidentInIRISResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public CreateIncidentInIRISResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData, "CreateIncidentInIRISResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
