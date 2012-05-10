using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class MetaData : IWidgetModel
  {    
    public MetaData() { Tags = new List<Tag>(); }
    public List<Tag> Tags { get; set; }
    public string Title { get; set; } 
  }
}
