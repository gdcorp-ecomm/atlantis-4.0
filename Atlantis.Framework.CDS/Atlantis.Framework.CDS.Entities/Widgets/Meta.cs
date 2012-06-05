using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Meta : IWidgetModel
  {
    public Meta() { }
    public string Title { get; set; }
    public string Canonical { get; set; }
    public string ShortCutIcon { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public bool Cache { get; set; }
    public bool Robots { get; set; }
    public SocialMediaData SocialData { get; set; }
  }

  public class SocialMediaData
  {
    public string Description { get; set; }
    public string Title { get; set; }
    public string CanonicalUrl { get; set; }
    public string ItemType { get; set; }
    public string ImageUrl { get; set; }
    public string FacebookAPIKey { get; set; }
    public string FacebookAdminKey { get; set; }
  }
}
