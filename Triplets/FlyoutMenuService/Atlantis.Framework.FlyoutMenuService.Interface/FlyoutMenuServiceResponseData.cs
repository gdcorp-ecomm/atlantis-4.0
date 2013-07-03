using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FlyoutMenuService.Interface
{
  public class FlyoutMenuServiceResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    public DataSet MenuDataSet { get; private set; }

    public FlyoutMenuServiceResponseData(DataSet ds)
    {
      MenuDataSet = ds;
    }

    public FlyoutMenuServiceResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public FlyoutMenuServiceResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "FlyoutMenuServiceResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return MenuDataSet.GetXml();
    }

    #endregion

  }
}
