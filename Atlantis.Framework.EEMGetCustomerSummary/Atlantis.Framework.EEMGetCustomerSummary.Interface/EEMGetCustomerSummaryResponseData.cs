using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetCustomerSummary.Interface
{
  public class EEMGetCustomerSummaryResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public Dictionary<int, EEMCustomerSummary> ReplacementDataDictionary {get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EEMGetCustomerSummaryResponseData(Dictionary<int, EEMCustomerSummary> replacementDataDictionary)
    {
      ReplacementDataDictionary = replacementDataDictionary;
    }

     public EEMGetCustomerSummaryResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EEMGetCustomerSummaryResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "EEMGetCustomerSummaryResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in EEMGetCustomerSummaryResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
