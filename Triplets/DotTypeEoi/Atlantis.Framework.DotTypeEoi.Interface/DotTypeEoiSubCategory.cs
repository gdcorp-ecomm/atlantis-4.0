using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiSubCategory : IDotTypeEoiSubCategory
  {
    [DataMember(Name = "@id")]
    public int SubCategoryId { get; set; }

    [DataMember(Name = "@name")]
    public string Name { get; set; }

    [DataMember(Name = "@displayName")]
    public string DisplayName { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }

    [DataMember(Name = "@showSelectAll")]
    public int ShowSelectAll { get; set; }

    [DataMember(Name = "gTlds")]
    public DotTypeEoiGtlds GtldsObject { get; set; }

    private IList<IDotTypeEoiGtld> _gtlds;
    [IgnoreDataMember]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        if (_gtlds == null)
        {
          if (GtldsObject != null && GtldsObject.GtldList != null && GtldsObject.GtldList.Count > 0)
          {
            _gtlds = new List<IDotTypeEoiGtld>(GtldsObject.GtldList.Count);
            foreach (var gtld in GtldsObject.GtldList)
            {
              var gtldTemp = gtld;
              gtldTemp.IsFeatured = Name.ToLower() == "featured";
              _gtlds.Add(gtld);
            }
          }
          else
          {
            _gtlds = new List<IDotTypeEoiGtld>(0);
          }
        }

        return _gtlds;
      }
    }
  }
}
