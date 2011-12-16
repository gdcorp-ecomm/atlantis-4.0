using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAction.Interface
{
  public class MyaActionRequestData : RequestData
  {

    public string ActionXml { get; set; }
    public string ActionArgs { get; set; }
    
    public MyaActionRequestData(string shopperId,
                                string sourceUrl,
                                string orderId,
                                string pathway,
                                int pageCount,
                                string actionXml,
                                string actionArgs)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ActionXml = actionXml;
      ActionArgs = actionArgs;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MyaActionRequestData");
    }

    public override string ToXML()
    {
      return string.Format("<MyaActionRequestData>{0}{1}</MyaActionRequestData>", base.ToXML(), ActionXml);
    }
  }
}