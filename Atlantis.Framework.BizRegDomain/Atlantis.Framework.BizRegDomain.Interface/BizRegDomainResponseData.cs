using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegDomain.Interface
{
  public class BizRegDomainResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _ex;
    private LocalBusiness _local_business;

    public LocalBusiness Business
    {
      get { return _local_business; }

    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public BizRegDomainResponseData(LocalBusiness bd)
    {
      _local_business = bd;
      _success = true;
    }

    public BizRegDomainResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public BizRegDomainResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData, "BizRegDomainResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponse methods

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion IResponse methods
  }
}
