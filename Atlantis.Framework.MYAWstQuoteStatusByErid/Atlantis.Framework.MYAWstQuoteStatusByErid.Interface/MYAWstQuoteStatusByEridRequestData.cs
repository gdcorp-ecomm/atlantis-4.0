using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAWstQuoteStatusByErid.Interface
{
  public class MYAWstQuoteStatusByEridRequestData : RequestData
  {
    #region Properties

    private string _externalResourceId;
    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 2);

    public string ExternalResourceId
    {
      get { return _externalResourceId; }
    }

    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }
    #endregion
 
    public MYAWstQuoteStatusByEridRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount, 
                                  string externalResourceId)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      _externalResourceId = externalResourceId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MYAWstQuoteStatusByEridRequestData");     
    }
  }
}
