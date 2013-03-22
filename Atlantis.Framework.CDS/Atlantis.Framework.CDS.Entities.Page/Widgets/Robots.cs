using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Page.Widgets
{
  public class Robots : IWidgetModel
  {
    public List<Section> Sections { get; set; }
  }

  public class Section
  {
    [JsonProperty("User-agent")]
    [BsonElement("User-agent")]
    public string UserAgent { get; set; }
    public List<string> Disallow { get; set; }
  }
}
