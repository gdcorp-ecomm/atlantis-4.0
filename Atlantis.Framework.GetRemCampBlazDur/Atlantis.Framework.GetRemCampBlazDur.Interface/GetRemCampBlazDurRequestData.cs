using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRemCampBlazDur.Interface
{
  public class GetRemCampBlazDurRequestData : RequestData
  {
    #region Properties

    public int EntityId { get; set; }
   
    #endregion

    public GetRemCampBlazDurRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  int entityId)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      EntityId = entityId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in GetRemCampBlazDurRequestData");     
    }
  }
}
