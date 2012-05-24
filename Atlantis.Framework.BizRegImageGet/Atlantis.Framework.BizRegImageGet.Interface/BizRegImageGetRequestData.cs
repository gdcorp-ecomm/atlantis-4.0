using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegImageGet.Interface
{
  public class BizRegImageGetRequestData : RequestData
  {
    private int businessDataIDValue;
    private string businessDataImageTypeValue;
    private int timeoutValue;

    public BizRegImageGetRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int dataId, string imageType, int timeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      businessDataIDValue = dataId;
      businessDataImageTypeValue = imageType;
      timeoutValue = timeout;
    }

    public int BusinessDataID
    {
      get { return businessDataIDValue; }
    }

    public string BusinessDataImageType
    {
      get { return businessDataImageTypeValue; }
    }

    public int Timeout
    {
      get { return timeoutValue; }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("BizRegImageGet is not a cacheable request.");
    }

  }
}
