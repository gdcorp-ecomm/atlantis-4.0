using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Headers
{
  public class HeadersSocialMedia
  {
    public string FacebookUrl { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string ItemType { get; set; }
    public string Title { get; set; }
    public string TweetText { get; set; }
    public string TweetUrl { get; set; }
    public string TweetRelated { get; set; }
    public bool UseFacebook { get; set; }
    public bool UseTwitter { get; set; }
    public bool UseGooglePlus { get; set; }
  }
}
