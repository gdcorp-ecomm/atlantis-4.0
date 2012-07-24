using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class CelebPhotos : IWidgetModel
  {
    public CelebPhotos()
    {
      Images = new List<string>();
    }
    public string TwitterText { get; set; }
    public string FacebookText { get; set; }
    public string PlayerBottomBackground { get; set; }
    public string ShareButtonsSprite { get; set; }
    public List<string> Images { get; set; }
  }
}
