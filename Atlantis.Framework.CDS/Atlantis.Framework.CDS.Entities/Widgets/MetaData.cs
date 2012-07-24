using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class MetaData : IWidgetModel
  {    
    public MetaData() { Tags = new List<Tag>(); }
    public List<Tag> Tags { get; set; }
    public string Title { get; set; } 
  }
}
