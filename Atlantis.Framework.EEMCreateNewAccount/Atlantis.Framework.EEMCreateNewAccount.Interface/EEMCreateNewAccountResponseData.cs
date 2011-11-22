using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMCreateNewAccount.Interface
{
  public class EEMCreateNewAccountResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public int CustomerId { get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EEMCreateNewAccountResponseData(int customerId)
    {
      CustomerId = customerId;
    }

     public EEMCreateNewAccountResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EEMCreateNewAccountResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "EEMCreateNewAccountResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return new XElement("CustomerId", CustomerId.ToString()).ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
