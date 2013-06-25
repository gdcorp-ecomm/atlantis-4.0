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

    [IgnoreDataMember]
    public int GtldCount
    {
      get { return GtldsObject.GtldCount; }
    }

    [IgnoreDataMember]
    public IEnumerable<IDotTypeEoiGtld> Gtlds
    {
      get { return GtldIdDictionary.Values; }
    }

    private IDictionary<int, IDotTypeEoiGtld> _gtldIdDictionary;
    [IgnoreDataMember]
    public IDictionary<int, IDotTypeEoiGtld> GtldIdDictionary
    {
      get
      {
        if (_gtldIdDictionary == null)
        {
          if (GtldsObject != null && GtldsObject.GtldList != null)
          {
            _gtldIdDictionary = new Dictionary<int, IDotTypeEoiGtld>(GtldsObject.GtldList.Count);
            foreach (var gtld in GtldsObject.GtldList)
            {
              _gtldIdDictionary[gtld.Id] = gtld;
            }
          }
          else
          {
            _gtldIdDictionary = new Dictionary<int, IDotTypeEoiGtld>(0);
          }
        }

        return _gtldIdDictionary;
      }
    }
  }
}
