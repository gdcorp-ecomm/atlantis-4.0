using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAppCount.Interface
{
  public class HCCAppCountRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    private const string CACHE_KEY = "HccAppCount";

    public HCCAppCountRequestData(string shopperId, string sourceUrl, string orderId, string sPathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, sPathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
    }
    
    public override string GetCacheMD5()
    {
      return CACHE_KEY;
    }

    public override string ToXML()
    {
      return string.Empty;
    }
  }
}
