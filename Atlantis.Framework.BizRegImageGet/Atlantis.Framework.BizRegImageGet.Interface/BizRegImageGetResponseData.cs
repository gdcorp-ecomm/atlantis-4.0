using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegImageGet.Interface
{
  public class BizRegImageGetResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _ex;
    private LocalURL _local_url;

    public LocalURL URL
    {
      get { return _local_url; }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public BizRegImageGetResponseData(LocalURL url)
    {
      _local_url = url;
      _success = true;
    }

    public BizRegImageGetResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public BizRegImageGetResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData, "BizRegImageGetResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponse methods

    public string ToXML()
    {
      throw new NotFiniteNumberException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion IResponse methods


  }
}
