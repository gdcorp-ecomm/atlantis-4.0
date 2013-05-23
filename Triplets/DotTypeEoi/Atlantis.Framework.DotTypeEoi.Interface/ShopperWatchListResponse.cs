using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class ShopperWatchListResponse : IShopperWatchListResponse
  {
    [DataMember(Name = "gTlds")]
    public DotTypeEoiGtlds GtldsObject { get; set; }

    [IgnoreDataMember()]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        IList<IDotTypeEoiGtld> gtlds;
        if (GtldsObject != null)
        {
          gtlds = (IList<IDotTypeEoiGtld>) GtldsObject.GtldList;
        }
        else
        {
          gtlds = new List<IDotTypeEoiGtld>();
        }

        return gtlds;
      }
      set { }
    }
  }
}
