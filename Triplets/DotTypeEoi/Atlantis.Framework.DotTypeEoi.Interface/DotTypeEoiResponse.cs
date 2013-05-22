using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiResponse : IDotTypeEoiResponse
  {
    [DataMember(Name = "@displayTime")]
    public string DisplayTime { get; set; }

    [DataMember(Name = "categories")]
    public DotTypeEoiCategories CategoriesWrapper { get; set; }

    public IList<IDotTypeEoiCategory> Categories
    {
      get { return CategoriesWrapper.CategoryList; }
      set{}
    }
  }
}
